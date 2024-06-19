using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Stylet;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;

namespace TFortisDeviceManager.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для MapView.xaml
    /// </summary>
    public class MapViewModel : Stylet.Screen, IDisposable
    {
        private readonly IUserSettings _userSettings;
        public static BindableCollection<string> MonitoringDevices { get; set; } = new BindableCollection<string>();
        public static BindableCollection<DeviceOnMapModel> MapDevices { get; set; } = new BindableCollection<DeviceOnMapModel>();
        public static BindableCollection<ConnectionOnMapModel> Connections { get; set; } = new BindableCollection<ConnectionOnMapModel>();
        public static BindableCollection<HolderOnMapModel> Holders { get; set; } = new BindableCollection<HolderOnMapModel>();
        public static BindableCollection<DotOnMapModel> DelLines { get; set; } = new BindableCollection<DotOnMapModel>();

        public static event EventHandler? SignalIfDeviceBoxChanged;

        private string? _BGImage;

        public string? BGImage
        {
            get
            {
                return _BGImage;
            }
            set
            {
                _BGImage = value;
                NotifyOfPropertyChange(nameof(BGImage));
            }
        }
        private double _scale = 1.75;

        public double Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scale = value;
                NotifyOfPropertyChange(nameof(_scale));
            }
        }

        public MapViewModel(IUserSettings userSettings)
        {
            DisplayName = Properties.Resources.MainMenuMap;

            MonitoringEventService.ChangingDeviceState += ChangingDeviceStateOnMap;
            MonitoringEventService.ChangingSensorProperties += ChangingSensorStateOnMap;

            MapDevices.CollectionChanged += (sender, e) => SignalIfDeviceBoxChanged?.Invoke(sender, e);

            Connections.CollectionChanged += ConnectionsChanged;
            MapDevices.CollectionChanged += DevicesForMapChanged;
            MonitoringEventService.UpdateMapSettings += ChangeSensorsSettingsOnMap;
            LoadDevicesForMapFromFile();
            _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings)); ;

            Scale = _userSettings.MapSettings.Scale;
            BGImage = _userSettings.MapSettings.BGImage;
        }

        public static void AddToMap(MonitoringDevice device)
        {
            System.Windows.Point positionOnMap = new(MapDevices.Count * 5, MapDevices.Count * 5);

            var deviceExistsOnMap = MapDevices.FirstOrDefault(dev => dev.Ip == device.IpAddress);
            if (deviceExistsOnMap != null)
            {
                Log.Information($"Была попытка добавить устройство, которое уже есть на карте. Ip {device.IpAddress}");

                string message = $"{Properties.Resources.DeviceExistsOnMapMessage}";
                HandyControl.Controls.MessageBox.Show(message, $"Ip: {device.IpAddress}", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var config = PGDataAccess.GetOidsForMonitoring(device.IpAddress);

            DeviceOnMapModel deviceForMap = new(device, positionOnMap);

          
            SetDeviceState(device, deviceForMap);

            SetSensorState(deviceForMap, config);

            MapDevices.Add(deviceForMap);   
        }

        public void ClearBackgroundCommand()
        {
            BGImage = string.Empty;
            _userSettings.MapSettings.BGImage = BGImage;
        }

        public void ClearDevicesCommand()
        {
            MapDevices.Clear();
            Connections.Clear();
            Holders.Clear();
        }

        public void LoadBackgroundCommand()
        {
            using var dialog = new System.Windows.Forms.OpenFileDialog
            {
                Title = Properties.Resources.SelectBackground,   
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _userSettings.MapSettings.BGImage = dialog.FileName;
                BGImage =  dialog.FileName;
            }
        }

        /// <summary>
        /// Вызывается, когда происходит новое событие на устрйстве, например, отпал линк
        /// </summary>        
        public void ChangingSensorStateOnMap(object? sender, ChangingSensorPropertiesEventArgs? e)
        {
            if (e == null) return;
            var evnt = e.Evnt;
            var deviceOnMap = MapDevices.FirstOrDefault(dev => dev.Ip == evnt.Ip);

            if (deviceOnMap == null)
                return;

            string sensorName = evnt.SensorName;

            string eventStatus = evnt.Status;

           deviceOnMap.ChangeSensorState(sensorName, eventStatus);

        }

        /// <summary>
        /// Вызывается после соранения настроек мониторинга устройства
        /// </summary>
        private void ChangeSensorsSettingsOnMap(object? sender, ChangingDeviceStateEventArgs e)
        {
            var deviceForMap = MapDevices.FirstOrDefault(x => x.Ip == e.Ip);

            if (deviceForMap != null)
            {
                var config = PGDataAccess.GetOidsForMonitoring(e.Ip);
                SetSensorState(deviceForMap, config);
            }
        }

        private static void SetDeviceState(MonitoringDevice selectedDevice, DeviceOnMapModel deviceForMap)
        {
            string state = PGDataAccess.GetLastDeviceState(selectedDevice.IpAddress);
            deviceForMap.IsAvailable = (state == HostStates.Enabled) || (state != HostStates.Disabled);
        }

        /// <summary>
        /// Вызывается, когда устройство меняет статус online/offline
        /// </summary>        
        public void ChangingDeviceStateOnMap(object? sender, ChangingDeviceStateEventArgs? e)
        {
            if (e == null) return;
            string ip = e.Ip;
            string state = e.State;

            var deviceOnMap = MapDevices.FirstOrDefault(dev => dev.Ip == ip);

            if (deviceOnMap == null) return;
            
            if (state == HostStates.Enabled)
            {
                deviceOnMap.IsAvailable = true;
            }
            else if (state == HostStates.Disabled)
            {
                deviceOnMap.IsAvailable = false;

                var config = PGDataAccess.GetOidsForMonitoring(ip);
                SetSensorState(deviceOnMap, config);
            }
        }

        /// <summary>
        /// Устанавливает состояние сенсоров
        /// </summary>        
        private static void SetSensorState(DeviceOnMapModel deviceForMap, List<OidsForDevice> config)
        {
            foreach (var row in config)
            {
                string sensorName = row.Name;

                if (sensorName.Contains("portPoeStatusState"))
                {
                    PoeStatusHandler(sensorName, deviceForMap, row.Enable);
                }
                else if (sensorName.Contains("linkState"))
                {
                    LinkStateHandler(sensorName, deviceForMap, row.Enable);
                }
                else
                {
                    switch(sensorName)
                    {
                        case "inputStateSensor#1":
                            {
                                if (row.Enable == 1)
                                    deviceForMap.StateInputSensor1 = SensorState.Bad;
                                else
                                    deviceForMap.StateInputSensor1 = SensorState.NotUse;
                                break;
                            }
                        case "inputStateSensor#2":
                            {
                                if (row.Enable == 1)
                                    deviceForMap.StateInputSensor2 = SensorState.Bad;
                                else
                                    deviceForMap.StateInputSensor2 = SensorState.NotUse;
                                break;
                            }
                        case "inputStateTamper":
                            {
                                if (row.Enable == 1)
                                    deviceForMap.StateInputTamper = SensorState.Bad;
                                else
                                    deviceForMap.StateInputTamper = SensorState.NotUse;
                                break;
                            }
                        case "upsPwrSource":
                            {
                                if (row.Enable == 1)
                                    deviceForMap.UpsState = SensorState.Bad;
                                else
                                    deviceForMap.UpsState = SensorState.NotUse;
                                break;
                            }
                    }
                }
              
            }
        }

        private static void LinkStateHandler(string sensorName, DeviceOnMapModel deviceForMap, int enable)
        {
            int position = sensorName.IndexOf("#");
            if (position < 0)
                return;

            string portId = sensorName[(position + 1)..];
            var port = deviceForMap.Ports.FirstOrDefault(p => p.Id == portId);

            if (port != null)
            {
                if (enable == 1)
                    port.Link = Models.LinkState.Down;
                else
                    port.Link = Models.LinkState.NotUse;
            }
            if (deviceForMap.Uplink1.Id == portId)
            {
                if (enable == 1)
                    deviceForMap.Uplink1.Link = Models.LinkState.Down;
                else
                    deviceForMap.Uplink1.Link = Models.LinkState.NotUse;
            }
            else if (deviceForMap.Uplink2.Id == portId)
            {
                if (enable == 1)
                    deviceForMap.Uplink2.Link = Models.LinkState.Down;
                else
                    deviceForMap.Uplink2.Link = Models.LinkState.NotUse;
            }
        }

        private static void PoeStatusHandler(string sensorName, DeviceOnMapModel deviceForMap, int enable)
        {
            int position = sensorName.IndexOf("#");
            if (position < 0)
                return;

            string portId = sensorName[(position + 1)..];
            var port = deviceForMap.Ports.FirstOrDefault(p => p.Id == portId);

            if (port != null)
            {
                if (enable == 1)
                    port.Poe = PoeState.Down;
                else
                    port.Poe = PoeState.NotUse;
            }
        }

        private static void AddLinesToMap(ConnectionOnMapModel? uplink1, ConnectionOnMapModel? uplink2, ObservableCollection<PortOnMapModel> ports)
        {
            if (uplink1 != null)
            {
                bool lineExists = Connections.Any(line => line.OriginPoint == uplink1.OriginPoint &&
                                                    line.DestinationPoint == uplink1.DestinationPoint);
                if (!lineExists)
                {
                    Connections.Add(uplink1);
                    AddHoldersToMap(uplink1);
                }
            }

            if (uplink2 != null)
            {
                bool lineExists = Connections.Any(line => line.OriginPoint == uplink2.OriginPoint &&
                                                    line.DestinationPoint == uplink2.DestinationPoint);
                if (!lineExists)
                {
                    Connections.Add(uplink2);
                    AddHoldersToMap(uplink2);
                }
            }

            foreach (var port in ports)
            {
                var lineOnMap = port.Line;
                if (lineOnMap == null) continue;

                bool lineExists = Connections.Any(line => line.OriginPoint == lineOnMap.OriginPoint &&
                                                    line.DestinationPoint == lineOnMap.DestinationPoint);

                if (!lineExists)
                {
                    Connections.Add(lineOnMap);
                    AddHoldersToMap(lineOnMap);
                }
            }
        }    

        private static void AddHoldersToMap(ConnectionOnMapModel uplink1)
        {
            foreach (var holder in uplink1.Holders)
            {
                Holders.Add(holder);
            }
        }

        /// <summary>
        /// Срабатывает когда коллекция соединений была изменена
        /// </summary>        
        private void ConnectionsChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            if (e == null) return;
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems[0] is ConnectionOnMapModel connection)
                {
                    connection.DelConnectionFromPorts(); 
                    RemoveHolders(connection); 
                }
            }
        }

        private void DevicesForMapChanged(object? sender, NotifyCollectionChangedEventArgs? e)
        {
            if (e == null) return;
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (e.OldItems[0] is DeviceOnMapModel device)
                {
                    RemoveLine(device.Uplink1.Line);
                    RemoveLine(device.Uplink2.Line);
                    foreach (PortOnMapModel port in device.Ports)
                    {
                        RemoveLine(port.Line);
                    }
                }
            }
        }

        private static void RemoveLine(ConnectionOnMapModel line)
        {
            if (line == null)
                return;
            Connections.Remove(Connections.SingleOrDefault(x =>
                                    x.OriginPoint == line.OriginPoint &&
                                    x.DestinationPoint == line.DestinationPoint));
        }

        private static void RemoveHolders(ConnectionOnMapModel connection)
        {
            foreach (var holder in connection.Holders)
            {
                Holders.Remove(holder);
            }
        }

        private static void СlearMap()
        {
            MapDevices.Clear();
            Connections.Clear();
            Holders.Clear();
        }

        private static void SaveDevicesForMapToFile()
        {
            var dcss = new DataContractSerializerSettings { PreserveObjectReferences = true };
            var dcs = new DataContractSerializer(typeof(ObservableCollection<DeviceOnMapModel>), dcss);

            using Stream fStream = new FileStream("devicesformap.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            dcs.WriteObject(fStream, MapDevices);
        }

        /// <summary>
        /// Восстановление устройств на карту из файла
        /// </summary>
        private static void LoadDevicesForMapFromFile()
        {
            ObservableCollection<DeviceOnMapModel>? data = GetMapDataFromFile();

            if (data.IsNullOrEmpty()) return;

            СlearMap();

            foreach (var device in data)
            {
                ConnectionOnMapModel? uplink1 = device.Uplink1?.Line ?? null;
                ConnectionOnMapModel? uplink2 = device.Uplink2?.Line ?? null;
                ObservableCollection<PortOnMapModel> ports = device.Ports;

                MapDevices.Add(device);

                AddLinesToMap(uplink1, uplink2, ports);
            }
        }

        private static ObservableCollection<DeviceOnMapModel>? GetMapDataFromFile()
        {
            ObservableCollection<DeviceOnMapModel>? data = new();

            var settingsSerializer = new DataContractSerializerSettings { PreserveObjectReferences = true };
            var serializer = new DataContractSerializer(typeof(ObservableCollection<DeviceOnMapModel>), settingsSerializer);

            using (Stream fStream = new FileStream("devicesformap.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    data = serializer.ReadObject(fStream) as ObservableCollection<DeviceOnMapModel>;
                }
                catch (System.Xml.XmlException)
                {
                    //
                }
            }

            return data;
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the view model has changed.
        /// </summary>
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the view model has changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion

        public void Dispose()
        {
            SaveDevicesForMapToFile();
            MonitoringEventService.ChangingDeviceState -= ChangingDeviceStateOnMap;
            MonitoringEventService.ChangingSensorProperties -= ChangingSensorStateOnMap;
            _userSettings.MapSettings.Scale = Scale;
        }
    }
}