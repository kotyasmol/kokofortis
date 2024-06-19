using System.Collections;
using System.ComponentModel;
using System.Linq;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager
{
    public class SortIPAddress : IComparer
    {
        private readonly ListSortDirection direction;

        public SortIPAddress(ListSortDirection direction)
        {
            this.direction = direction;
        }

        int IComparer.Compare(object? x, object? y)
        {
            string xIp = "";
            string yIp = "";
            long nX = 0;
            long nY = 0;

            if (x is NetworkDevice && y is NetworkDevice)
            {
                xIp = ((NetworkDevice)x).IpAddress;
                yIp = ((NetworkDevice)y).IpAddress;
            }
            else if(x is MonitoringDevice && y is MonitoringDevice)
            {
                xIp = ((MonitoringDevice)x).IpAddress;
                yIp = ((MonitoringDevice)y).IpAddress;
            }
            else if (x is EventModel && y is EventModel)
            {
                xIp = ((EventModel)x).Ip;
                yIp = ((EventModel)y).Ip;
            }

            if (xIp != string.Empty && yIp != string.Empty)
            {
                string[] octetsX = xIp.Split('.');
                string[] octetsY = yIp.Split('.');

                if (octetsX.Count() == 4 && octetsY.Count() == 4)
                {
                    nX = long.Parse(octetsX[0]) * 256 * 256 * 256 + long.Parse(octetsX[1]) * 256 * 256 + long.Parse(octetsX[2]) * 256 + long.Parse(octetsX[3]);
                    nY = long.Parse(octetsY[0]) * 256 * 256 * 256 + long.Parse(octetsY[1]) * 256 * 256 + long.Parse(octetsY[2]) * 256 + long.Parse(octetsY[3]);
                }
            }

            if (direction == ListSortDirection.Ascending)
            {
                return MyCompare(nX, nY);
            }
            return MyCompare(nX, nY) * -1;
        }

        static int MyCompare(long x, long y)
        {
            if (x == y)
            {
                return 0;
            }
            if (x > y)
            {
                return 1;
            }
            return -1;
        }
    }
}
