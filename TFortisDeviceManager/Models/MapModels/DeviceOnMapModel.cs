using DocumentFormat.OpenXml.Wordprocessing;
using HandyControl.Tools.Extension;
using Stylet;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TFortisDeviceManager.Database;

namespace TFortisDeviceManager.Models
{
    public class DeviceOnMapModel : INotifyPropertyChanged, IEquatable<DeviceOnMapModel>
    {
        public BindableCollection<PortOnMapModel> Ports { get; set; }

        public double Width { get; set; } = 60;
        public double Height { get; set; } = 75;
        public string Model { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string Ip { get; set; }

        public PortOnMapModel? Uplink1 { get; set; }

        public PortOnMapModel? Uplink2 { get; set; }


        #region StateInputSensor1 property
        private SensorState stateInputSensor1 = SensorState.NotUse;
        public SensorState StateInputSensor1
        {
            get
            {
                return stateInputSensor1;
            }
            set
            {
                if (stateInputSensor1 == value)
                {
                    return;
                }

                stateInputSensor1 = value;

                OnPropertyChanged("StateInputSensor1");
            }
        }

        private bool _needRefresh = true;
        public bool NeedRefresh
        {
            get
            {
                return _needRefresh;
            }
            set
            {
                if (_needRefresh == value)
                {
                    return;
                }

                _needRefresh = value;

                OnPropertyChanged("NeedRefresh");
            }
        }
        #endregion

        #region StateInputSensor2 property
        private SensorState stateInputSensor2 = SensorState.NotUse;
        public SensorState StateInputSensor2
        {
            get
            {
                return stateInputSensor2;
            }
            set
            {
                if (stateInputSensor2 == value)
                {
                    return;
                }

                stateInputSensor2 = value;

                OnPropertyChanged("StateInputSensor2");
            }
        }
        #endregion

        #region StateInputTamper property
        private SensorState stateInputTamper = SensorState.NotUse;
        public SensorState StateInputTamper
        {
            get
            {
                return stateInputTamper;
            }
            set
            {
                if (stateInputTamper == value)
                {
                    return;
                }

                stateInputTamper = value;

                OnPropertyChanged("StateInputTamper");
            }
        }
        #endregion

        #region Ups property
        private SensorState upsState = SensorState.NotUse;
        public SensorState UpsState
        {
            get
            {
                return upsState;
            }
            set
            {
                if (upsState == value)
                {
                    return;
                }

                upsState = value;

                OnPropertyChanged("UpsState");
            }
        }
        #endregion

        public void ChangeSensor1State(string eventStatus)
        {
            if (eventStatus == EventState.Ok)
                StateInputSensor1 = SensorState.Ok;
            else
                StateInputSensor1 = SensorState.Bad;
        }
        public void ChangeSensor2State(string eventStatus)
        {
            if (eventStatus == EventState.Ok)
                StateInputSensor2 = SensorState.Ok;
            else
                StateInputSensor2 = SensorState.Bad;
        }

        public void ChangeTamperState(string eventStatus)
        {
            if (eventStatus == EventState.Ok)
                StateInputTamper = SensorState.Ok;
            else
                StateInputTamper = SensorState.Bad;
        }

        public void ChangeUpsState(string eventStatus)
        {
            if (eventStatus == EventState.Ok)
                UpsState = SensorState.Ok;
            else
                UpsState = SensorState.Bad;
        }



        #region X property
        private double x;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value)
                {
                    return;
                }

                x = value;

                OnPropertyChanged("X");
            }
        }
        #endregion

        #region Y property
        private double y;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value)
                {
                    return;
                }

                y = value;

                OnPropertyChanged("Y");
            }
        }
        #endregion                

        #region Available property
        private bool isAvailable;
        public bool IsAvailable
        {
            get
            {
                return isAvailable;
            }
            set
            {
                if (isAvailable == value)
                {
                    return;
                }

                isAvailable = value;

                OnPropertyChanged("IsAvailable");
            }
        }
        #endregion

        private bool selected;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                if (selected == value)
                {
                    return;
                }

                selected = value;

                OnPropertyChanged("Selected");
            }
        }

        public void ChangeSensorState(string sensorName, string eventStatus)
        {
            switch (sensorName)
            {
                case "inputStateSensor#1": { ChangeSensor1State(eventStatus); return; }
                case "inputStateSensor#2": { ChangeSensor2State(eventStatus); return; }
                case "inputStateTamper": { ChangeTamperState(eventStatus); return; }
                case "upsPwrSource": { ChangeUpsState(eventStatus); return; }

            };

            if (sensorName.Contains("portPoeStatusState"))
            {
                int position = sensorName.IndexOf("#");
                if (position < 0)
                    return;

                string portId = sensorName[(position + 1)..];

                ChangePortPoeState(portId, eventStatus);
                return;
            }
            else if (sensorName.Contains("linkState"))
            {
                int position = sensorName.IndexOf("#");
                if (position < 0)
                    return;

                string portId = sensorName[(position + 1)..];

                ChangePortLinkState(portId, eventStatus);
                return;
            }
        }

        public void ChangePortPoeState(string portId, string eventStatus)
        {
            var port = Ports.FirstOrDefault(p => p.Id == portId);
            if (port == null)
                return;

            if (eventStatus == EventState.Problem)
            {
                port.Poe = PoeState.Down;
            }
            else if (eventStatus == EventState.Ok)
            {
                port.Poe = PoeState.Up;
            }
            else
            {
                port.Poe = PoeState.NotUse;
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Poe"));
        }

        public void ChangePortLinkState(string portId, string eventStatus)
        {
            var port = Ports.FirstOrDefault(p => p.Id == portId);
            // Если порт не Uplink
            if (port != null)
            {
                ChangeLinkState(eventStatus, Ports.FirstOrDefault(p => p.Id == portId));
            }

            if (Uplink1 != null && portId == Uplink1.Id)
            {
                ChangeLinkState(eventStatus, Uplink1);

            }

            if (Uplink2 != null && portId == Uplink2.Id)
            {
                ChangeLinkState(eventStatus, Uplink2);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Link"));

        }

        private static void ChangeLinkState(string eventStatus, PortOnMapModel port)
        {
            if (eventStatus == EventState.Problem)
            {
                port.Link = LinkState.Down;
            }
            else if (eventStatus == EventState.Ok)
            {
                port.Link = LinkState.Up;
            }
            else
            {
                port.Link = LinkState.NotUse;
            }
        }

        public DeviceOnMapModel(Point pointInMap, string model, string ip, int portsCount)
        {
            X = pointInMap.X;
            Y = pointInMap.Y;
            Model = model;
            Ip = ip;
            Uplink1 = new PortOnMapModel(this);
            Uplink2 = new PortOnMapModel(this);

            Ports = new BindableCollection<PortOnMapModel>();
            InitPorts(portsCount);
        }

        public DeviceOnMapModel(Point pointInMap, MonitoringDevice device, int portsCount)
        {
            X = pointInMap.X;
            Y = pointInMap.Y;
            Model = device.Name;
            Ip = device.IpAddress;
            Location = device.Location;
            Description = device.Description;
            Uplink1 = new PortOnMapModel(this);
            Uplink2 = new PortOnMapModel(this);

            Ports = new BindableCollection<PortOnMapModel>();
            InitPorts(portsCount);
        }

        public DeviceOnMapModel(MonitoringDevice device, Point positionOnMap)
        {
            if (device.Name == "SWU-16" || device.Name == "TELEPORT-1" || device.Name == "TELEPORT-2")
            {

                var portsCount = PGDataAccess.GetPortsCount(device.Id);

                X = positionOnMap.X;
                Y = positionOnMap.Y;
                Model = device.Name;
                Ip = device.IpAddress;
                Location = device.Location;
                Description = device.Description;
                Uplink1 = new PortOnMapModel(this);
                Uplink2 = new PortOnMapModel(this);

                Ports = new BindableCollection<PortOnMapModel>();
                InitPorts(portsCount);
            }else
            {
                var portsCount = device.PortsPoe;

                X = positionOnMap.X;
                Y = positionOnMap.Y;
                Model = device.Name;
                Ip = device.IpAddress;
                Location = device.Location;
                Description = device.Description;
                Uplink1 = new PortOnMapModel(this);
                Uplink2 = new PortOnMapModel(this);

                Ports = new BindableCollection<PortOnMapModel>();
                InitPorts(portsCount);

                var portsUplinkCount = PGDataAccess.GetPortsUplinkCount(device.Id);

                int portId = portsCount + 1;
                Uplink1.Id = portId.ToString(CultureInfo.InvariantCulture);

                portId++;
                Uplink2.Id = portId.ToString(CultureInfo.InvariantCulture);
            }   
        }

        public DeviceOnMapModel()
        {
          
        }

        public void InitPorts(int portsCount)
        {
            for (int i = 1; i <= portsCount; i++)
            {
                Ports.Add(new PortOnMapModel(i.ToString(CultureInfo.InvariantCulture), this));
            }
        }

        public void Move(Point deltaPoint)
        {
            X += deltaPoint.X;
            Y += deltaPoint.Y;
        }
        public bool Equals(DeviceOnMapModel? other)
        {
            if (other == null)
                return false;

            return (X == other.X) && (Y == other.Y);
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the view model has changed.
        /// </summary>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the view model has changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        public override bool Equals(object? obj)
        {
            return Equals(obj as DeviceOnMapModel);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public enum SensorState
    {
        NotUse,
        Ok,
        Bad
    }
}

