using System;

namespace TFortisDeviceManager.Models
{
    public interface IDeviceState
    {
        event EventHandler<DeviceStateEventArgs> DeviceStateChanged;
        DeviceState DeviceState { get; }
    }

    public enum DeviceState
    {
        Unknown,
        Problem,
        Info,
        Ok
    }

    public class DeviceStateEventArgs : EventArgs
    {
        public DeviceState NewState { get; }
        public DeviceStateEventArgs(DeviceState deviceState)
        {
            NewState = deviceState;
        }
    }
}
