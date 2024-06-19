using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TFortisDeviceManager.Models
{
    public sealed class NetworkDevice : IEquatable<NetworkDevice>, INotifyPropertyChanged
    {   
        public uint Id { get; set; }
        private bool _isSelected = false;

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
        public string Name { get; set; }
        public IReadOnlyList<Port>? Ports { get; set; }
        public IReadOnlyList<Contact>? Contacts { get; }
        public bool HasUps { get; }
        public string IpAddress { get; set; }
        public string? NetworkMask { get; set; }
        public string? Gateway { get; set; }
        public string Mac { get; set; } 
        public string? SerialNumber { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string? Firmware { get; set; }
        public string? UpTime { get; set; }
        public bool InMonitoring { get; set; }
        public string? ToolTip { get; set; } 
        public IReadOnlyList<Camera>? ListCameras { get; set; }

        public NetworkDevice(uint id, string name, IReadOnlyList<Port> ports, IReadOnlyList<Contact> contacts, bool hasUps)
        {
            Id = id;
            Name = name;
            Ports = ports;
            HasUps = hasUps;
            Contacts = contacts;
        }
        public NetworkDevice()
        {
         
        }

        public void ValidateGateway()
        {
            if (Gateway == "")
                Gateway = "255.255.255.255";
        }

        public MonitoringDevice ToMonitoringDevice()
        {
            MonitoringDevice device = new(Name, IpAddress, Mac, SerialNumber, Location, Description);     

            return device;
        }

        public string GetDeviceInfo()
        {
            StringBuilder builder = new();

            builder.Append(CultureInfo.InvariantCulture, $"<b>{Name}</b>\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.IpAddress}: {IpAddress}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Mac}: {Mac}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.SerialNumber}: {SerialNumber}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Location}: {Location}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Description}: {Description}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.UpTime}: {UpTime}\n\n");

            return builder.ToString();
        }

        public override string ToString()
        {
            return "Device: " + Name + " " + IpAddress;
        }

        public bool Equals(NetworkDevice? other)
        {            
            return other != null && 
                IpAddress == other.IpAddress && 
                Mac == other.Mac;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as NetworkDevice);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IpAddress, Mac);
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the view model has changed.
        /// </summary>
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the view model has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}