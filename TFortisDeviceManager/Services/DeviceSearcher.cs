using Serilog;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using TFortisDeviceManager.Helpers;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Services
{

    public interface IDeviceSearcher
    {
        Task<IReadOnlyList<NetworkDevice>> SearchBroadcastAsync(CancellationToken cancelationToken);
        Task<IReadOnlyList<NetworkDevice>> SearchUnicastAsync(string startIp, string endIp, CancellationToken cancelationToken);
    }

    public class DeviceSearcher : IDeviceSearcher
    {
        private readonly DevicesFactory _devicesFactory = DevicesFactory.Instance;
        private readonly IApplicationSettings _appSettings;

        private readonly int _udpPort;
        private readonly byte _responseCode;
        private readonly byte[] _messageForSearch;

        public DeviceSearcher(IApplicationSettings appSettings)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));

            _udpPort = _appSettings.Port;
            _responseCode = _appSettings.SearchResponseCode;
            var requestCode = _appSettings.SearchRequestCode;
            var messageSize = _appSettings.MessageSize;

            _messageForSearch = new byte[messageSize];
            _messageForSearch[0] = requestCode;
        }

        public async Task<IReadOnlyList<NetworkDevice>> SearchBroadcastAsync(CancellationToken cancelationToken)
        {
            var rawDevices = await SearchBroadcastAsync(_udpPort, _responseCode, _messageForSearch, cancelationToken);
            
            return await CreateDevicesAsync(rawDevices);
        }        

        public Task<IReadOnlyList<NetworkDevice>> SearchUnicastAsync(string startIp, string endIp, CancellationToken cancelationToken)
        {
            if (string.IsNullOrEmpty(startIp))
            {
                throw new ArgumentException($"'{nameof(startIp)}' cannot be null or empty.", nameof(startIp));
            }

            if (string.IsNullOrEmpty(endIp))
            {
                throw new ArgumentException($"'{nameof(endIp)}' cannot be null or empty.", nameof(endIp));
            }

            Log.Information($"Запущен Unicast поиск в диапазоне от {startIp} до {endIp}");


            return SearchUnicastInternalAsync(startIp, endIp);
        }

        private async Task<IReadOnlyList<NetworkDevice>> CreateDevicesAsync(IReadOnlyList<byte[]> rawDevices)
        {
            List<NetworkDevice> devices = new();

            CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(8));
            CancellationToken internalToken = cancelTokenSource.Token;

            foreach (var rawDeviceArray in rawDevices)
            {
                var device = await _devicesFactory.TryCreateAsync(rawDeviceArray, internalToken);

                if (device != null && !devices.Contains(device))
                {
                    devices.Add(device);
                }
            }

            Log.Information($"Broadcast поиск завершен, устройств найдено: {devices.Count}");


            return devices;
        }

        #region SearchBroadcast        

        /// <summary>
        /// Широковещательный поиск усройств в сети
        /// </summary>         
        private async Task<IReadOnlyList<byte[]>> SearchBroadcastAsync(int port, byte responseCode, byte[] messageForSearch, CancellationToken сancellationToken)
        { 
            UdpClient udpClient = new(port);                   

            try
            {
                var taskReceive = Task.Run(() => UdpService.ReceiveDataArray(responseCode, udpClient, сancellationToken), сancellationToken);
                UdpService.SendBroadcastMessage(port, messageForSearch);
                Log.Information($"Запущен Broadcast поиск");

                return await taskReceive;
            }
            catch(Exception ex)
            {
                Log.Error(ex, "SearchBroadcastAsync error: {message}", ex.Message);
                return Array.Empty<byte[]>();
            }
            finally
            {
                udpClient.Close();
            }    
        }
        
        #endregion
        
        #region SearchUnicast
        

        private async Task<IReadOnlyList<NetworkDevice>> SearchUnicastInternalAsync(string startIp, string endIp)
        {
            var rawDevices = await SearchUnicastAsync(startIp, endIp);

            return await CreateDevicesAsync(rawDevices);
        }

        private async Task<IReadOnlyList<byte[]>> SearchUnicastAsync(string startIp, string endIp)
        { 
            UdpClient udpClient = new(_udpPort);            

            CancellationTokenSource cancelTokenSource = new();
            CancellationToken token = cancelTokenSource.Token;

            try
            {
                var taskReceive = Task.Run(() => UdpService.ReceiveDataArray(_responseCode, udpClient, token), token);

                var addressRange = IpAddressHelper.GetIpAddressList(startIp, endIp);

                await Task.Run(() => UdpService.SendUnicastMessage(addressRange, _udpPort, _messageForSearch));               

                await Task.Delay(1000);

                cancelTokenSource.Cancel();
                return await taskReceive;
            }            
            finally
            {
                udpClient.Close();
            }            
        }
        #endregion
    }
    #region StubSearcher
    public class StubDeviceSearcher : IDeviceSearcher
    {        
        private readonly DevicesFactory _devicesFactory = DevicesFactory.Instance;
        //private readonly ILogger _logger;

       /* public StubDeviceSearcher(ILogger logger )
        {
            _logger = logger ?? throw new ArgumentNullException(nameof( logger));
        }*/

        public async Task<IReadOnlyList<NetworkDevice>> SearchBroadcastAsync(CancellationToken cancelationToken)
        {
            Random rnd = new();
            List<NetworkDevice> devices = new();

            await Task.Run(async () =>
            {
                while (!cancelationToken.IsCancellationRequested)
                {
                    var model = rnd.Next(1, 9);
                    var ip = rnd.Next(2, 254);

                    PhysicalAddress mac = PhysicalAddress.TryParse("C0:11:A6:0C:0A:03", out var parsedMac) ? parsedMac : PhysicalAddress.None;
                    var rawDevice = new RawDevice((uint)model, $"192.168.0.{ip}", "255.255.255.0", mac.ToString())
                    {                       
                        Gateway = "192.168.0.1",                        
                        SerialNumber = "02563",
                        Description = "Описание",
                        Location = "Местоположение",
                        Firmware = "2.4"
                    };
                    var device = await _devicesFactory.TryCreateAsync(rawDevice, cancelationToken);
                                       
                    if(device != null)
                    {
                        device.ListCameras = new List<Camera>
                        {
                            new Camera { Port = 1, Ip = "192.168.10.11", Mac="11:22:33:44:55:66"},
                            new Camera { Port = 2, Ip = "192.168.10.12", Mac="11:22:33:44:55:67"},
                            new Camera { Port = 3, Ip = "192.168.10.13", Mac="11:22:33:44:55:68"},
                        };
                        devices.Add(device);
                    }

                    await Task.Delay(500);
                }
            }, cancelationToken);
            return devices;
        }

        public Task<IReadOnlyList<NetworkDevice>> SearchUnicastAsync(string startIp, string endIp, CancellationToken cancelationToken)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
