using System.Collections.Generic;

namespace TFortisDeviceManager.Models
{
    public class RawDevice
    {
        public uint Id { get; }

        public RawDevice(uint id, string ipAddress, string networkMask, string mac)
        {
            Id = id;
            NetworkMask = networkMask;
            Mac = mac;
            IpAddress = ipAddress;
        }
        public string IpAddress { get; }
        public string NetworkMask { get; }        
        public string Mac { get; }
        public string? Gateway { get; set; }
        public string? SerialNumber { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Firmware { get; set; }
        public IReadOnlyList<Camera>? ListCameras { get; set; }
        public string Uptime { get; set; }

        public void ValidateGateway()
        {
            if (Gateway == "255.255.255.255")
                Gateway = "";
        }
    }
}
