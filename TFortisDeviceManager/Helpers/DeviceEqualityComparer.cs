using System;
using System.Collections.Generic;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager
{
    class DeviceEqualityComparer : IEqualityComparer<MonitoringDevice>
    {
        public bool Equals(MonitoringDevice? x, MonitoringDevice? y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Id == y.Id && x.Name == y.Name && x.IpAddress == y.IpAddress
                        && x.Netmask == y.Netmask && x.Gateway == y.Gateway
                        && x.Mac == y.Mac && x.SerialNumber == y.SerialNumber)
                return true;
            else
                return false;
        }

        public int GetHashCode(MonitoringDevice obj)
        {
            throw new NotImplementedException();
        }
    }
}
