using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace TFortisDeviceManager.Models
{
    public sealed class MonitoringDevice : IEquatable<NetworkDevice>, INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IpAddress { get; set; }
        public string Netmask { get; set; }
        public string Mac { get; set; } 
        public string SerialNumber { get; set; }
        public string Gateway { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public string Firmware { get; set; }
        public int PortsPoe { get; set; }
        public int Uptime { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; OnPropertyChanged(); }
        }

        private bool _onMap;
        public bool OnMap
        {
            get { return _onMap; }
            set { _onMap = value; OnPropertyChanged(); }
        }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set { _checked = value; OnPropertyChanged(); }
        }

        public string? ToolTip { get; set; } 

        private bool _avilable = true;

        public bool Available
        {
            get { return _avilable; }
            set
            {
                _avilable = value;
                OnPropertyChanged();
            }
        }

        private bool _state;
        public bool State
        {
            get { return _state; }
            set
            {
                _state = value;
                OnPropertyChanged();
            }
        }

        private DeviceState _deviceState;
        public DeviceState DeviceState
        {
            get { return _deviceState; }
            set
            {
                _deviceState = value;
                OnPropertyChanged();
            }
        }

        public MonitoringDevice(string name, string ip, string mac, string serialNumber, string? location, string? description)
        {
            Name = name;
            IpAddress = ip;
            Mac = mac;
            SerialNumber = serialNumber;
            Location = location;
            Description = description;
        }

        public NetworkDevice ToNetworkDevice()
        {
            NetworkDevice device = new()
            {
                Name = Name,
                IpAddress = IpAddress,
                Mac = Mac,
                SerialNumber = SerialNumber,
                Location = Location,
                Description = Description,
                NetworkMask = Netmask,
                Gateway = Gateway
            };

            return device;
        }

        public List<Camera> ListCameras { get; set; }

        public string GetDeviceInfo()
        {
            StringBuilder builder = new();
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.IpAddress}: {IpAddress}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Mac}: {Mac}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.SerialNumber}: {SerialNumber}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Location}: {Location}\n");
            builder.Append(CultureInfo.InvariantCulture, $"{Properties.Resources.Description}: {Description}\n");

            return builder.ToString();
        }

        public override string ToString()
        {
            return "Device: " + Name + " " + IpAddress;
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

        public bool Equals(NetworkDevice? other)
        {
            throw new NotImplementedException();
        }
    }
}
