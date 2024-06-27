using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Lextm.SharpSnmpLib.Messaging;
using Lextm.SharpSnmpLib;
using LiveCharts;
using LiveCharts.Definitions.Series;
using LiveCharts.Wpf;
using Stylet;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using System.Net;
using TFortisDeviceManager.Properties;
using Variable = Lextm.SharpSnmpLib.Variable;
using Serilog;
using Task = System.Threading.Tasks.Task;
using TFortisDeviceManager.Models.Devices;
using System.Runtime.CompilerServices;

namespace TFortisDeviceManager.ViewModels
{
    public class DeviceEventArgs : EventArgs
    {
        public MonitoringDevice Device { get; set; }
    }
    public sealed class GraphicsViewModel : Screen, INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Random _random = new Random();
        private readonly Queue<QGraphics> _dataQueue = new Queue<QGraphics>();
        private readonly Dictionary<string, Queue<double>> _lineDataDict;
        private readonly int _maxQueueSize = 10;
        private readonly int _maxPoints = 50;
        private readonly IMonitoringEventService _monitoringEventService;

        public GraphicsViewModel(IMonitoringEventService monitoringEventService, ISettingsProvider settingsProvider)
        {
            _monitoringEventService = monitoringEventService;
            _userSettings = settingsProvider.UserSettings;

            MonitoringEventService.DeviceAddedForDashboard += OnDeviceAddedToDashboard;


            _lineDataDict = new Dictionary<string, Queue<double>>
            {
                { "Series1", new Queue<double>(Enumerable.Repeat(0.0, _maxPoints)) }
            };
            _monitoringDevices = new ObservableCollection<MonitoringDevice>();
            SeriesCollection = new SeriesCollection();
            LineSeriesCollection = new SeriesCollection();
            Labels = Enumerable.Range(0, _maxPoints).Select(i => i.ToString()).ToList();
            InitializeData();
            UpdateDataAsync();
        }

        #region всё для графиков
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _seriesCollection; }
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(nameof(SeriesCollection));
            }
        }
        private SeriesCollection _lineSeriesCollection;
        public SeriesCollection LineSeriesCollection
        {
            get { return _lineSeriesCollection; }
            set
            {
                _lineSeriesCollection = value;
                OnPropertyChanged(nameof(LineSeriesCollection));
            }
        }
        private double _temperature;
        public double Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged(nameof(Temperature));
            }
        }


        private List<string> _labels;
        public List<string> Labels
        {
            get { return _labels; }
            set
            {
                _labels = value;
                OnPropertyChanged(nameof(Labels));
            }
        }

        private void InitializeData()
        {
            FillQueueWithZeros();
            InitializePieChart();
            InitializeLineChart();
        }

        private void FillQueueWithZeros()
        {
            for (int i = 0; i < _maxQueueSize; i++)
            {
                _dataQueue.Enqueue(new QGraphics
                {
                    Value = 0,
                    Title = $"Параметр {i + 1}"
                });
            }
        }

        private void AddRandomData()
        {
            var data = new QGraphics
            {
                Value = _random.Next(1, 10),
                Title = $"Параметр {_dataQueue.Count + 1}"
            };
            _dataQueue.Enqueue(data);
        }

        private async void UpdateDataAsync()
        {
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                if (_dataQueue.Count >= _maxQueueSize)
                {
                    _dataQueue.Dequeue();
                }

                AddRandomData();

                foreach (var key in _lineDataDict.Keys.ToList())
                {
                    if (_lineDataDict[key].Count >= _maxPoints)
                    {
                        _lineDataDict[key].Dequeue();
                    }
                    _lineDataDict[key].Enqueue(_random.Next(1, 100));
                }

                UpdatePieChart();
                UpdateLineChart();
                //UpdateTemperature();
            }
        }

        private void InitializePieChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(0, 32, 46),
                Color.FromRgb(44, 72, 117),
                Color.FromRgb(138, 80, 143),
                Color.FromRgb(188, 80, 144),
                Color.FromRgb(255, 99, 97),
                Color.FromRgb(255, 133, 49),
                Color.FromRgb(255, 166, 0),
                Color.FromRgb(128, 211, 83),
                Color.FromRgb(96, 159, 63),
                Color.FromRgb(64, 106, 42)
            };

            int index = 0;
            foreach (var data in _dataQueue)
            {
                SeriesCollection.Add(new PieSeries
                {
                    Title = data.Title,
                    Values = new ChartValues<double> { data.Value },
                    DataLabels = true,
                    Fill = new SolidColorBrush(colors[index % colors.Count])
                });
                index++;
            }
        }

        private void UpdatePieChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(0, 32, 46),
                Color.FromRgb(44, 72, 117),
                Color.FromRgb(138, 80, 143),
                Color.FromRgb(188, 80, 144),
                Color.FromRgb(255, 99, 97),
                Color.FromRgb(255, 133, 49),
                Color.FromRgb(255, 166, 0),
                Color.FromRgb(128, 211, 83),
                Color.FromRgb(96, 159, 63),
                Color.FromRgb(64, 106, 42)
            };

            int index = 0;

            foreach (var data in _dataQueue)
            {
                if (index >= SeriesCollection.Count)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = data.Title,
                        Values = new ChartValues<double> { data.Value },
                        DataLabels = true,
                        Fill = new SolidColorBrush(colors[index % colors.Count])
                    });
                }
                else
                {
                    var series = SeriesCollection[index] as PieSeries;
                    if (series != null)
                    {
                        series.Values[0] = data.Value;
                        series.Title = data.Title;
                        series.Fill = new SolidColorBrush(colors[index % colors.Count]);
                    }
                }
                index++;
            }

            while (SeriesCollection.Count > _dataQueue.Count)
            {
                SeriesCollection.RemoveAt(SeriesCollection.Count - 1);
            }
        }

        private void InitializeLineChart()
        {
            var colors = new List<Color>
            {
                Color.FromRgb(255, 0, 0), // Red
                Color.FromRgb(0, 255, 0), // Green
                Color.FromRgb(0, 0, 255), // Blue
                Color.FromRgb(255, 255, 0) // Yellow
            };

            int index = 0;
            foreach (var key in _lineDataDict.Keys)
            {
                LineSeriesCollection.Add(new LineSeries
                {
                    Title = key,
                    Values = new ChartValues<double>(_lineDataDict[key]),
                    PointGeometry = null,
                    Stroke = new SolidColorBrush(colors[index % colors.Count]),
                    Fill = Brushes.Transparent,
                });
                index++;
            }
        }
        private void UpdateLineChart()
        {
            int index = 0;
            foreach (var key in _lineDataDict.Keys)
            {
                var series = LineSeriesCollection[index] as LineSeries;
                if (series != null)
                {
                    var values = series.Values as ChartValues<double>;
                    if (values != null)
                    {
                        values.Clear();
                        values.AddRange(_lineDataDict[key]);
                    }
                }
                index++;
            }
            Labels = Enumerable.Range(0, _maxPoints).Select(i => (i + 1).ToString()).ToList();
        }
        private void UpdateTemperature()
        {
            Temperature = Temperature;
        }

        #endregion всё для графиков

        public event EventHandler<DeviceEventArgs> DeviceAdded;
        public void AddDevice(MonitoringDevice device)
        {
            DeviceAdded?.Invoke(this, new DeviceEventArgs { Device = device });
        }

        private readonly IUserSettings _userSettings;
        private readonly INotificationService _notificationService;

        private readonly string statusOk = Resources.StatusOk;
        private readonly string statusProblem = Resources.StatusProblem;
        private readonly string statusError = Resources.StatusError;
        private readonly string statusInfo = Resources.StatusInfo;
        private readonly string sensorErrorExceptionValue = Resources.SensorErrorExceptionValue;

        private readonly string sensorNameHostStatus = Resources.SensorNameHostStatus;
        private readonly string hostStatusEnabled = Resources.HostStatusEnabled;
        private readonly string hostStatusDisabled = Resources.HostStatusDisabled;
        private readonly string hostStatusReloaded = Resources.HostStatusReloaded;

        private BlockingCollection<EventModel>? queueEvents;
        private CancellationTokenSource? ctsForProducer;
        private CancellationTokenSource? ctsForConsumer;
        private ConcurrentBag<Task>? tasks;
        private SnmpTrapDaemon? snmpTrapD;
        private Task? taskConsumer;


        private ObservableCollection<MonitoringDevice> _monitoringDevices;
        public ObservableCollection<MonitoringDevice> MonitoringDevices
        {
            get { return _monitoringDevices; }
            set
            {
                _monitoringDevices = value;
                OnPropertyChanged(nameof(MonitoringDevices));
            }
        }



        private double GetUptime(string ip, string community) 
        {
            int timeoutWaitingForResponseSnmp = _userSettings.MonitoringSettings.TimeoutWaitingForResponseSnmp;
            var results = Messenger.Get(VersionCode.V1,
                                        new IPEndPoint(IPAddress.Parse(ip), 161),
                                        new OctetString(community),
                                        new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.2.1.1.3.0")) },
                                        timeoutWaitingForResponseSnmp);

            var variable = results.First();
            double seconds = TimeSpan.Parse(variable.Data.ToString(), CultureInfo.InvariantCulture).TotalSeconds;
            return seconds;
        }
        private double GetBatteryTime(string ip, string community)
        {
            int timeoutWaitingForResponseSnmp = _userSettings.MonitoringSettings.TimeoutWaitingForResponseSnmp;
            var results = Messenger.Get(VersionCode.V1,
                                        new IPEndPoint(IPAddress.Parse(ip), 161),
                                        new OctetString(community),
                                        new List<Variable> { new Variable(new ObjectIdentifier("1.3.6.1.4.1.42019.3.2.2.1.4.0")) },
                                        timeoutWaitingForResponseSnmp);

            var variable = results.First();
            double seconds = Convert.ToDouble(variable.Data.ToString(), CultureInfo.InvariantCulture);
            return seconds;
        }

        public void StartMonitoring()
        {
            MonitoringEvents.Clear();
            Task.Run(() => Run());
            Log.Information("Мониторинг запущен");
        }
        public async Task Run()
        {
            //bool trapEnable = _userSettings.MonitoringSettings.EnableRecieveTrap;
            queueEvents = new BlockingCollection<EventModel>(10000);
            ctsForProducer = new CancellationTokenSource();
            ctsForConsumer = new CancellationTokenSource();

            ctsForProducer.Token.Register(() => Console.WriteLine("ctsForProducer cancelled"));
            ctsForConsumer.Token.Register(() => Console.WriteLine("ctsForConsumer cancelled"));

            tasks = new ConcurrentBag<Task>();

            // Здесь только одно устройство для мониторинга
            var selectedDevice = MonitoringDevices.FirstOrDefault(); 

            if (selectedDevice != null)
            {
                tasks.Add(CheckUptimeLoopAsync(selectedDevice, ctsForProducer.Token));
            }

           /* if (trapEnable)
            {*/
                snmpTrapD = new SnmpTrapDaemon(queueEvents, ctsForConsumer.Token);
                snmpTrapD.Start();
           //}

            await Task.WhenAll(tasks);
            ctsForProducer.Dispose();
            ctsForConsumer.Dispose();
        }

        private async Task CheckUptimeLoopAsync(MonitoringDevice device, CancellationToken token)
        {

            int uptimeValue;
            int uptimePrevious;
            int timeoutCount = 0;

            string descriptionDeviceNotAvilable = Resources.DescriptionDeviceNotAvilable;
            string descriptionHostReloaded = Resources.DescriptionHostReloaded;

            bool sensorsGetRun = false;

            var sensors = PGDataAccess.LoadOidsForDashboard(device.IpAddress); 

            var community = PGDataAccess.GetCommunity(device.IpAddress);


            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("CheckUptimeLoopAsync canceled.");
                    queueEvents.CompleteAdding();
                    queueEvents.Dispose();
                    break;
                }

                int timeout = _userSettings.MonitoringSettings.TimeoutCheckUptime;
                EventModel? evnt = null;

                uptimePrevious = PGDataAccess.GetUptime(device);

                try
                {
                    uptimeValue = (int)GetUptime(device.IpAddress, community);
                    device.Available = true;

                    if (uptimeValue < uptimePrevious)
                    {
                        evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                                            hostStatusReloaded, descriptionHostReloaded, statusInfo);
                    }
                    else
                    {
                        TimeSpan span = TimeSpan.FromSeconds(uptimeValue);
                        string uptime = FormatUptime(span);

                        string description = $"{Properties.Resources.ContinuousOperationTime} {uptime}";

                        evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                            hostStatusEnabled, description, statusOk);
                    }

                    device.Uptime = uptimeValue;

                    if (!sensorsGetRun)
                    {
                        await RunReadSensorsAsync(sensors, device, token);
                        Log.Information($"Опрос сенсоров для {device.IpAddress} был запущен");

                        sensorsGetRun = true;
                    }
                    timeoutCount = 0;
                }
                catch (Lextm.SharpSnmpLib.Messaging.TimeoutException)
                {
                    HandleTimeoutException(ref timeoutCount, device, ref evnt, sensorNameHostStatus, descriptionDeviceNotAvilable);
                }
                catch (ErrorException ex)
                {
                    HandleErrorException(ex, device, ref evnt, sensorNameHostStatus);
                }
                catch (SnmpException)
                {
                    HandleSnmpException(ref timeoutCount, device, ref evnt, sensorNameHostStatus);
                }

                if (evnt != null)
                    queueEvents.TryAdd(evnt, 2, token);

                await Task.Delay(TimeSpan.FromSeconds(timeout), token).ConfigureAwait(false);
            }
        }

        private async Task RunReadSensorsAsync(List<Sensor> sensors, MonitoringDevice device, CancellationToken token)
        {
            int delayBetweenTaskReadSensorValueLoop = _userSettings.MonitoringSettings.DelayBetweenTaskReadSensorValueLoop;

            foreach (var sensor in sensors)
            {
                if (sensor.Enable)
                {
                    tasks.Add(ReadSensorValueLoop(sensor, device, token));
                }

                await Task.Delay(delayBetweenTaskReadSensorValueLoop, token).ConfigureAwait(false);
            }
        }

        private int GetSensorValue(Sensor sensor, string community)
        {
            int timeoutWaitingForResponseSnmp = _userSettings.MonitoringSettings.TimeoutWaitingForResponseSnmp;
            var results = Messenger.Get(VersionCode.V1,
                                        new IPEndPoint(IPAddress.Parse(sensor.Ip!), 161),
                                        new OctetString(community),
                                        new List<Variable> { new Variable(new ObjectIdentifier(sensor.Address!)) },
                                        timeoutWaitingForResponseSnmp);

            var variable = results.First();
            int sensorValueResult = int.Parse(variable.Data.ToString(), CultureInfo.InvariantCulture);
            return sensorValueResult;
        }

        private async Task ReadSensorValueLoop(Sensor sensor, MonitoringDevice device, CancellationToken token)
        {
            int timeout = sensor.Timeout;
            string sensorValueText = "";

            var community = PGDataAccess.GetCommunity(device.IpAddress);

            while (sensor.Enable)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("ReadSensorValueLoop canceled.");
                    break;
                }

                if (!device.Available)
                {
                    await Task.Delay(TimeSpan.FromSeconds(timeout), token).ConfigureAwait(false);
                    continue;
                }

                EventModel? evnt = null;

                try
                {
                    string? description;
                    int sensorValue = GetSensorValue(sensor, community);
                    string status = DetermineSensorStatus(sensor, sensorValue, ref sensorValueText);

                    switch (sensor.Name)
                    {
                        case "upsPwrSource": // тип питания
                            if (sensorValue == sensor.BadValue)
                            {
                                description = GetUpsPowerSourceDescription(sensor, community);
                            }
                            else
                            {
                                description = sensor.Description;
                            }
                            break;

                        case "PortPoeStatusPower#1": // добавила
                            double volt = GetSensorValue(sensor, community);
                            description = sensor.Description;
                            Temperature = volt;
                            break;

                        default:
                            description = sensor.Description;
                            break;
                    }

                    evnt = CreateEvent(sensor.DeviceName, sensor.Ip, device.Description, device.Location, sensor.Name, sensorValueText, description, status);
                }
                catch (Lextm.SharpSnmpLib.Messaging.TimeoutException) { }
                catch (ErrorException ex)
                {
                    evnt = CreateEvent(sensor.DeviceName, sensor.Ip, device.Description, device.Location, sensor.Name, sensorErrorExceptionValue, ex.Message, statusError);
                }
                catch (SnmpException) { }

                if (evnt != null)
                    queueEvents.TryAdd(evnt, 2, token);

                await Task.Delay(timeout * 1000, token).ConfigureAwait(false);
            }
        }
        private string GetUpsPowerSourceDescription(Sensor sensor, string community)
        {
            double seconds = GetBatteryTime(sensor.Ip!, community);
            TimeSpan timeLeft = TimeSpan.FromSeconds(seconds);
            return $"{Resources.TimeLeft} {timeLeft.Hours}h.{timeLeft.Minutes}m";
        }
        private string DetermineSensorStatus(Sensor sensor, int sensorValue, ref string sensorValueText)
        {
            string status = statusOk;

            if (sensor.Name == "temperature" || sensor.Name == "humidity")
            {
                sensorValueText = sensorValue.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                if (sensorValue == sensor.OkValue)
                {
                    if (sensor.Invert)
                    {
                        status = statusProblem;
                    }
                    sensorValueText = sensor.OkValueText!;
                }
                else if (sensorValue == sensor.BadValue)
                {
                    if (sensor.Invert)
                    {
                        status = statusOk;
                    }
                    else
                    {
                        status = statusProblem;
                    }
                    sensorValueText = sensor.BadValueText!;
                }
            }

            return status;
        }
        private string FormatUptime(TimeSpan span)
        {
            string uptime = $"{span.Days}{Resources.Day} {span.Hours}{Resources.Minute} {span.Minutes}{Resources.Minute}";

            if (span.Days == 0)
                uptime = $"{span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";

            if (span.Days == 0 && span.Hours == 0)
                uptime = $"{span.Minutes}{Resources.Minute}";

            return uptime;
        }

        #region обработка ошибок

        private void HandleTimeoutException(ref int timeoutCount, MonitoringDevice device, ref EventModel? evnt, string sensorNameHostStatus, string descriptionDeviceNotAvilable)
        {
            timeoutCount++;
            if (timeoutCount == 5)
            {
                device.Available = false;
                evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus, hostStatusDisabled, descriptionDeviceNotAvilable, statusProblem);
                timeoutCount = 0;
            }
        }

        private void HandleErrorException(ErrorException ex, MonitoringDevice device, ref EventModel? evnt, string sensorNameHostStatus)
        {
            evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus, sensorErrorExceptionValue, ex.Message, statusError);
        }

        private void HandleSnmpException(ref int timeoutCount, MonitoringDevice device, ref EventModel? evnt, string sensorNameHostStatus)
        {
            timeoutCount++;
            if (timeoutCount == 5)
            {
                device.Available = false;
                evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus, hostStatusDisabled, "SNMP Error", statusProblem);
                timeoutCount = 0;
            }
        }

        public void Dispose()
        {
            ctsForProducer?.Cancel();
            ctsForConsumer?.Cancel();
            ctsForProducer?.Dispose();
            ctsForConsumer?.Dispose();
        }

        #endregion бработка ошибок

        #region ивенты 
        public static EventModel CreateEvent(string deviceName, string ip, string deviceDescription, string deviceLocation, string sensorName, string sensorValueText, string description, string status)
        {
            return new EventModel()
            {
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                DeviceName = deviceName,
                Ip = ip,
                SensorName = sensorName,
                Description = description,
                SensorValueText = sensorValueText,
                Status = status,
                DeviceDescription = deviceDescription,
                DeviceLocation = deviceLocation
            };
        }
        public ObservableCollection<DashboardDevice> MonitoringEvents { get; } = new ObservableCollection<DashboardDevice>();
        private void OnDeviceAddedToDashboard(object sender, DeviceAddedEventArgs e)
        {
            if (e.Device is MonitoringDevice dashboardDevice)
            {
                MonitoringDevice monitoringDevice = e.Device;
                DashboardDevice device = new(monitoringDevice.Id, 
                    monitoringDevice.Name, 
                    monitoringDevice.IpAddress, 
                    monitoringDevice.Location, 
                    monitoringDevice.Description, 
                    monitoringDevice.Firmware, 
                    monitoringDevice.Uptime);

                MonitoringDevices.Add(device);

                StartMonitoring();
            }
        }
        #endregion ивенты

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}



