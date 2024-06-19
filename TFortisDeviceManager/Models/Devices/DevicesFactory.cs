using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using TFortisDeviceManager.Properties;

namespace TFortisDeviceManager.Models
{
    public class DevicesFactory
    {
        private static DevicesFactory? _instance;
        private List<JsonModelDescription>? _modelDescriptions;
        private readonly Dictionary<uint, NetworkDevice> _networkDevicesCache = new();

        public static DevicesFactory Instance => LazyInitializer.EnsureInitialized(ref _instance, () => new DevicesFactory());        

        private static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            WriteIndented = true,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)                
            }
        };

        private DevicesFactory()
        {
        }

        public Task<NetworkDevice?> TryCreateAsync([NotNull] byte[] rawDeviceArray, CancellationToken cancelationToken)
        {
            if (rawDeviceArray is null) throw new ArgumentNullException(nameof(rawDeviceArray));            

            return TryCreateInternalAsync(rawDeviceArray, cancelationToken);            
        }

        public async Task<NetworkDevice?> TryCreateAsync(
            RawDevice rawDevice,
            CancellationToken cancelationToken)
        {
            try
            {
                rawDevice.ValidateGateway();

                uint id = rawDevice.Id;
                string? ipAddress = rawDevice.IpAddress;
                string? networkMask = rawDevice.NetworkMask;
                string? gateway = rawDevice.Gateway;
                var mac = rawDevice.Mac;
                string? serialNumber = rawDevice.SerialNumber;
                string? description = rawDevice.Description;
                string? location = rawDevice.Location;
                string? firmware = rawDevice.Firmware;
                var cameras = rawDevice.ListCameras;
                string? uptime = rawDevice.Uptime;

                using FileStream openStream = File.OpenRead($"Models\\models_descriptions.json");

                _modelDescriptions ??= await JsonSerializer.DeserializeAsync<List<JsonModelDescription>>(openStream, _jsonSerializerOptions, cancelationToken);

                if (_modelDescriptions == null)
                    return default;

                var modelDescription = _modelDescriptions.FirstOrDefault(x => x.Id == id);

                if (modelDescription == null)
                {
                    return default;
                }

                var ports = new List<Port>();

                try
                {
                    foreach (var portDescription in modelDescription.Ports)
                    {
                        var portsForAdd = Enumerable.Repeat(new Port(portDescription.IsSfp, portDescription.Poe), (int)portDescription.NumberOf);
                        ports.AddRange(portsForAdd);
                    }
                }
                catch { }

                var contacts = new List<Contact>();
                foreach (var contactDescription in modelDescription.Contacts)
                {
                    var contactsForAdd = Enumerable.Repeat(new Contact { Type = contactDescription.Type }, (int)contactDescription.NumberOf);
                    contacts.AddRange(contactsForAdd);
                }

                NetworkDevice sample;
                if (!_networkDevicesCache.ContainsKey(id))
                {
                    sample = new NetworkDevice(id, modelDescription.Name, ports, contacts, modelDescription.HasUps);
                    _networkDevicesCache.Add(id, sample);
                }
                else
                {
                    sample = _networkDevicesCache[id];                    
                }

                var device = await sample.TryCreateDeepCopyAsync();
                if (device != null)
                {
                    device.IpAddress = ipAddress;
                    device.NetworkMask = networkMask;
                    device.Gateway = gateway;
                    device.Mac = mac;
                    device.Ports = ports;
                    device.SerialNumber = serialNumber;
                    device.Description = description;
                    device.Location = location;
                    device.Firmware = firmware;
                    device.ListCameras = cameras;
                    device.UpTime = uptime;
                    device.ToolTip = $"{Resources.Name}: {device.Name}\n" +
                                     $"{Resources.Description}: {description}\n" +
                                     $"{Resources.Location}: {location}";
                    return device;
                }

                return default;                
            }
            catch (Exception)
            {
                return default;
            }            
        }

        private async Task<NetworkDevice?> TryCreateInternalAsync([NotNull] byte[] rawDeviceArray, CancellationToken cancelationToken)
        {
            try
            {
                var rawDevice = ParseData(rawDeviceArray);
                return await TryCreateAsync(rawDevice, cancelationToken);
            }
            catch (Exception)
            {
                return null;
            }            
        }

        private static RawDevice ParseData([NotNull]byte[] rawDeviceArray)
        {
            uint deviceId = rawDeviceArray[1];

            string ipAddress = $"{rawDeviceArray[2]}.{rawDeviceArray[3]}.{rawDeviceArray[4]}.{rawDeviceArray[5]}";
            string netMask = $"{rawDeviceArray[276]}.{rawDeviceArray[277]}.{rawDeviceArray[278]}.{rawDeviceArray[279]}";
            string gateway = $"{rawDeviceArray[280]}.{rawDeviceArray[281]}.{rawDeviceArray[282]}.{rawDeviceArray[283]}";
            string macAddress = $"{rawDeviceArray[6]:X2}:{rawDeviceArray[7]:X2}:{rawDeviceArray[8]:X2}:{rawDeviceArray[9]:X2}:{rawDeviceArray[10]:X2}:{rawDeviceArray[11]:X2}";
            string serialNumber = Convert.ToInt32($"{rawDeviceArray[10]:X2}{rawDeviceArray[11]:X2}", 16).ToString();            

            byte[] descriptionByteArray = rawDeviceArray[12..128];
            string description = Uri.UnescapeDataString(Encoding.UTF8.GetString(descriptionByteArray).TrimEnd('\0'));

            byte[] locationByteArray = rawDeviceArray[140..268];
            string location = Uri.UnescapeDataString(Encoding.UTF8.GetString(locationByteArray).TrimEnd('\0'));
            string firmware = $"{rawDeviceArray[274]}.{rawDeviceArray[273]}.{rawDeviceArray[272]}";

            byte[] uptimeByteArray = rawDeviceArray[268..272];
            int uptime = BitConverter.ToInt32(uptimeByteArray);

            TimeSpan span = TimeSpan.FromSeconds(uptime);
            string resultUptime = $"{span.Days}{Resources.Day} {span.Hours}{Resources.Hour} {span.Minutes}{Properties.Resources.Minute}";
            if(span.Days == 0)
                resultUptime = $"{span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";
            if(span.Days == 0 && span.Hours == 0)
                resultUptime = $"{span.Minutes}{Resources.Minute}";

            var cameras = GetCameras(rawDeviceArray);
            
            return new RawDevice(deviceId, ipAddress, netMask, macAddress)
            {
                Gateway = gateway,
                SerialNumber = serialNumber,
                Description = description,
                Location = location,
                Firmware = firmware,
                ListCameras = cameras,
                Uptime = resultUptime
            };
        }

        private static IReadOnlyList<Camera> GetCameras(byte[] data)
        {
            List<Camera> camerasList = new();

            const int maxNumberPorts = 16;
            int startByteIp = 290;
            int startByteMac = 284;

            for (int nPort = 0; nPort < maxNumberPorts; nPort++)
            {
                byte[] ipByteArray = data[(startByteIp + 10 * nPort)..(4 + startByteIp + 10 * nPort)];
                byte[] macByteArray = data[(startByteMac + 10 * nPort)..(6 + startByteMac + 10 * nPort)];
                string ipAddress = string.Join(".", ipByteArray);
                string macAddress = string.Join(":", macByteArray.Select(b => b.ToString("X2")));

                var maxMacPart = macByteArray.Max();
                if (maxMacPart > 0)
                {
                    camerasList.Add(new Camera() { Port = nPort + 1, Ip = ipAddress, Mac = macAddress });
                }
            }

            return camerasList;
        }
    }    

    static class ExtensionMethods
    {
        public static Task<NetworkDevice?> TryCreateDeepCopyAsync<NetworkDevice>(this NetworkDevice self)
        {
            if (self is null)
                return Task.FromResult(default(NetworkDevice));

            var serializeOptions = new JsonSerializerOptions
            {
                Converters =
                {
                    new PhysicalAddressJsonConverter()
                }
            };

            return self.TryCreateDeepCopyInternalAsync(serializeOptions);
        }

        private static async Task<T?> TryCreateDeepCopyInternalAsync<T>(this T self, JsonSerializerOptions? serializeOptions = null)
        {            
            using var stream = new MemoryStream();
            await JsonSerializer.SerializeAsync(stream, self, serializeOptions);
            stream.Seek(0, SeekOrigin.Begin);
            return await JsonSerializer.DeserializeAsync<T>(stream, serializeOptions);
        }
    }

    public class PhysicalAddressJsonConverter : JsonConverter<PhysicalAddress>
    {
        public override PhysicalAddress Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return PhysicalAddress.TryParse(reader.GetString(), out var value) ? value : PhysicalAddress.None;
        }

        public override void Write(Utf8JsonWriter writer, PhysicalAddress value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
