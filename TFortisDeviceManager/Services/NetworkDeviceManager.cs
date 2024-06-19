using System.Threading.Tasks;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using TFortisDeviceManager.Models;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.IO;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TFortisDeviceManager.Services;

public interface INetworkDeviceManager
{
    Task<NetworkDeviceManagerErrorCode> SetSettingsAsync(NetworkDevice device, string? login, string? password, bool isSnmpEnabled, ObservableCollection<PortSettings> ports, CancellationToken cancelationToken);
    Task<NetworkDeviceManagerErrorCode> SetSntpSettingsAsync(NetworkDevice device, string? login, string? password, string sntpServer, string timezone, string period, CancellationToken cancelationToken);
    Task<NetworkDeviceManagerErrorCode> SetGroupSettingsAsync(NetworkDevice device, string? login, string? password, bool isSnmpEnabled, bool isLldpEnabled, CancellationToken cancelationToken);
    Task<NetworkDeviceManagerErrorCode> RebootDeviceAsync(PhysicalAddress mac, string? login, string? password, CancellationToken cancelationToken); 
    Task<NetworkDeviceManagerErrorCode> SetDefaultSettingsAsync(PhysicalAddress mac, string? login, string? password, CancellationToken cancelationToken);
    Task<NetworkDeviceManagerErrorCode> SetSettingsFromFileAsync(PhysicalAddress mac, string? login, string? password, string filePath, CancellationToken cancelationToken);
}

public enum NetworkDeviceManagerErrorCode
{
    Accepted,
    AuthenticationError,
    ConfigError,
    FileIsBig,
    Timeout,
    UnknownError = 0x1234
}

public class NetworkDeviceManager : INetworkDeviceManager
{
    private readonly IApplicationSettings _applicationSettings;

    public NetworkDeviceManager(IApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
    }

    public Task<NetworkDeviceManagerErrorCode> SetSettingsAsync(NetworkDevice device, string? login, string? password, bool isSnmpEnabled, ObservableCollection<PortSettings> ports, CancellationToken cancelationToken)
    {  
        string settingsString = $"#IPADDRESS=[{device.IpAddress}]#NETMASK=[{device.NetworkMask}]#GATEWAY=[{device.Gateway}]#SYSTEM_NAME=[{device.Description}]#SYSTEM_LOCATION=[{device.Location}]";

        if (isSnmpEnabled)
        {
            settingsString += "#SNMP_STATE=[1]";
        }

        for(int i = 0; i < ports.Count; i++)
        {
            if (ports[i].EnablePort)
            {
                settingsString += $"#PORT{i + 1}_STATE=[1]";
            }

            if (ports[i].EnablePoe)
            {
                settingsString += $"#PORT{i + 1}_POE=[257]";
            }
        }



        return SendSettingsAsync(ParseMac(device.Mac), settingsString, login, password, cancelationToken);
    }

    public Task<NetworkDeviceManagerErrorCode> SetSntpSettingsAsync(NetworkDevice device, string? login, string? password, string sntpServer, string timezone, string period, CancellationToken cancelationToken)
    {
        string settingsString = "";

        settingsString += "#SNTP_STATE=[1]";

        settingsString += $"#SNTP_SETT_SERV=[{sntpServer}]";

        settingsString += $"#SNTP_TIMEZONE=[{timezone}]";

        settingsString += $"#SNTP_PERIOD=[{period}]";

        return SendSettingsAsync(ParseMac(device.Mac), settingsString, login, password, cancelationToken);
    }


    public Task<NetworkDeviceManagerErrorCode> SetGroupSettingsAsync(NetworkDevice device, string? login, string? password, bool isSnmpEnabled, bool isLldpEnabled, CancellationToken cancelationToken)
    {
        string settingsString = $"";

        if (isSnmpEnabled)
        {
            settingsString += "#SNMP_STATE=[1]";
        }

        if (isLldpEnabled)
        {
            settingsString += "#LLDP_STATE=[1]";
        }

        return SendSettingsAsync(ParseMac(device.Mac), settingsString, login, password, cancelationToken);
    }

    public Task<NetworkDeviceManagerErrorCode> RebootDeviceAsync(PhysicalAddress mac, string? login, string? password, CancellationToken cancelationToken)
    {
        string settingsString = $"#REBOOT_ALL=[1]";
        return SendSettingsAsync(mac, settingsString, login, password, cancelationToken);
    }

    public Task<NetworkDeviceManagerErrorCode> SetDefaultSettingsAsync(PhysicalAddress mac, string? login, string? password, CancellationToken cancelationToken)
    {
        string settingsString = $"#DEFAULT_ALL=[1]";
        return SendSettingsAsync(mac, settingsString, login, password, cancelationToken);
    }

    public async Task<NetworkDeviceManagerErrorCode> SetSettingsFromFileAsync(PhysicalAddress mac, string? login, string? password, string filePath, CancellationToken cancelationToken)
    {
        NetworkDeviceManagerErrorCode errorCode;
        StringBuilder settingsString = new();       
        
        string[] fileContent = File.ReadAllLines(filePath);

        foreach (var line in fileContent)
        {
            if (!line.StartsWith("------"))
                settingsString.Append(line);
        }

        errorCode = await SendSettingsAsync(mac, settingsString.ToString(), login, password, cancelationToken);

        return errorCode;
    }

    private async Task<NetworkDeviceManagerErrorCode> SendSettingsAsync(PhysicalAddress mac, string settingsString, string? login, string? password, CancellationToken cancelationToken)
    {
        int port = _applicationSettings.Port;
        byte responseCode = _applicationSettings.SettingsResponseCode;
        byte requestCode = _applicationSettings.SettingsRequestCode;
        
        string macLoginPassword = $"{mac}+{login ?? string.Empty}+{password ?? string.Empty}";
        string md5 = GetMd5Hash(macLoginPassword);
        byte[] md5MacLoginPassword = Encoding.UTF8.GetBytes(md5);                  // Массив байтов md5 мак+логин+пароль

        byte[] settingsRequestBytes = { requestCode };                             // Массив байтов с кодом запроса                             
        byte[] macBytes = mac.GetAddressBytes();                                   // Массив байтов мак-адреса             
        byte[] settingsStringBytes = Encoding.UTF8.GetBytes(settingsString);       // Массив байтов строки с настройками

        if (settingsStringBytes.Length > 1024)
            return NetworkDeviceManagerErrorCode.FileIsBig;

        byte[] messageBytes = CreateMessageBytes(settingsRequestBytes, md5MacLoginPassword, macBytes, settingsStringBytes);

        var sendingResult = await SendSettingsMessageAsync(port, responseCode, messageBytes, cancelationToken);

        if (!Enum.IsDefined(typeof(NetworkDeviceManagerErrorCode), sendingResult))
            return NetworkDeviceManagerErrorCode.UnknownError;

        return (NetworkDeviceManagerErrorCode)sendingResult;
    }

    private static async Task<int> SendSettingsMessageAsync(int port, byte responseCode, byte[] messageWithSettings, CancellationToken cancelationToken)
    {
        UdpClient? udpClient = null;
        try
        {
            udpClient = new(port);

            Task<byte[]> taskReceive = Task.Run(() => UdpService.ReceiveSingleData(responseCode, udpClient, cancelationToken), cancelationToken);

            UdpService.SendBroadcastMessage(port, messageWithSettings);

            await taskReceive;

            if (taskReceive.Result.Length > 0)
            {
                return taskReceive.Result[7]; //Вынести в констранты
            }
        }
        catch (SocketException)
        {
            // ignore
        }
        catch (OperationCanceledException) when (cancelationToken.IsCancellationRequested)
        {
            return (int)NetworkDeviceManagerErrorCode.Timeout;
        }
        catch (OperationCanceledException)
        {
            // ignore
        }
        finally
        {
            udpClient?.Close();
        }

        return -1;
    }

    private static byte[] CreateMessageBytes(byte[] settingsRequestBytes, byte[] md5MacLoginPassword, byte[] macBytes, byte[] settingsStringBytes)
    {
        int MessageLength = settingsRequestBytes.Length + macBytes.Length + md5MacLoginPassword.Length + settingsStringBytes.Length;
        byte[] messageWithSettings = new byte[MessageLength];

        Buffer.BlockCopy(settingsRequestBytes, 0, messageWithSettings, 0, settingsRequestBytes.Length);
        Buffer.BlockCopy(macBytes, 0, messageWithSettings, settingsRequestBytes.Length, macBytes.Length);
        Buffer.BlockCopy(md5MacLoginPassword, 0, messageWithSettings, macBytes.Length + settingsRequestBytes.Length, md5MacLoginPassword.Length);
        Buffer.BlockCopy(settingsStringBytes, 0, messageWithSettings, macBytes.Length + md5MacLoginPassword.Length + settingsRequestBytes.Length, settingsStringBytes.Length);

        return messageWithSettings;
    }

    private static string GetMd5Hash(string input)
    {
        MD5 md5Hash = MD5.Create();

        byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder sBuilder = new();

        for (int i = 0; i < data.Length; i++)
        {
            sBuilder.Append(data[i].ToString("x2"));
        }

        return sBuilder.ToString();
    }

    public static PhysicalAddress ParseMac(string data)
    {
        var bytes = new List<byte>();
        foreach (var b in data.Split(':'))
        {
            bytes.Add(byte.Parse(b, System.Globalization.NumberStyles.HexNumber));
        };
        return new PhysicalAddress(bytes.ToArray());
    }
}