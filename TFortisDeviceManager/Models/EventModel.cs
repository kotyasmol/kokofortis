using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public class EventModel : INotifyPropertyChanged, IEquatable<EventModel>
    {
        private string time = "";
        private string ip = "";

        public int Id { get; set; }

        public string Time
        {
            get { return time; }
            set
            {
                if (value != time)
                {
                    time = value;
                    NotifyPropertyChanged(nameof(Time));
                }
            }
        }
        public string? DeviceName { get; set; }
        public string Ip
        {
            get { return ip; }
            set
            {
                if (value != ip)
                {
                    ip = value;
                    NotifyPropertyChanged(nameof(Ip));
                }
            }
        }
        public string SensorName { get; set; } = "";
        public string SensorValueText { get; set; } = "";
        public string Age { get; set; } = $"0{Properties.Resources.Minute}";
        public string Description { get; set; } = "";
        public string Status { get; set; } = "Ok";

        public string DeviceLocation { get; set; } = "";
        public string DeviceDescription { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FormPushNotification()
        {
            return $"{DeviceName}  |  {Ip}\n\n" +
                   $"{Description} - {Status}\n";
        }

        public string FormTelegramNotification()
        {
            return $"❌ <b>PROBLEM</b>\n\n" +
                   $"<b>{Description}</b>\n" +
                   $"{Time}\n\n" +
                   $"{Properties.Resources.Name}: {DeviceName}\n" +
                   $"{Properties.Resources.IpAddress}: {Ip}\n" +
                   $"{Properties.Resources.Description}: {DeviceDescription}\n" +
                   $"{Properties.Resources.Location}: {DeviceLocation}";
        }

        public override string ToString()
        {
            return Status switch
            {
                "Ok" => $"✅{Description}\n",
                "Info" => $"ℹ{Description}\n",
                "Problem" => $"❌{Description}\n",
                _ => $"{Status}\t{Description}\n"
            };
        }

        public override bool Equals(object obj)
        {
            if (obj is EventModel other)
            {
                return Equals(other);
            }
            return false;
        }

        public bool Equals(EventModel other)
        {
            return Id == other.Id && Time == other.Time && SensorName == other.SensorName && Age == other.Age;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Time, SensorName);
        }
    }
}
