using System;
using System.Collections.Generic;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Services
{
    public class ChangingSelectedDeviceEventArgs : EventArgs
    {
        public NetworkDevice Device { get; private set; }
        public List<NetworkDevice> FoundDevices { get; private set; } = new List<NetworkDevice>();


        public ChangingSelectedDeviceEventArgs(NetworkDevice device, List<NetworkDevice> foundDevices)
        {
            Device = device;
            FoundDevices = foundDevices;
        }
    }
}
