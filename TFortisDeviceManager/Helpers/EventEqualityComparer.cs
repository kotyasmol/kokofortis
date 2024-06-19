using System;
using System.Collections.Generic;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager
{
    class EventEqualityComparer : IEqualityComparer<EventModel>
    {
        public bool Equals(EventModel? x, EventModel? y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Age == y.Age && x.Description == y.Description && x.DeviceName == y.DeviceName
                        && x.Ip == y.Ip && x.SensorName == y.SensorName && x.SensorValueText == y.SensorValueText
                        && x.Status == y.Status && x.Time == y.Time)
                return true;
            else
                return false;
        }

        public bool Equals(List<EventModel> x, List<EventModel> y)
        {
            if (x == null && y == null)
                return true;
            else if (x == null || y == null)
                return false;
            else if (x.Count != y.Count)
                return false;

            for(int i = 0; i < x.Count; i++)
            {
                if (!Equals(x[i],y[i]))
                    return false;
            }
          
            return true;
        }

        public int GetHashCode(EventModel obj)
        {
            throw new NotImplementedException();
        }
    }
}
