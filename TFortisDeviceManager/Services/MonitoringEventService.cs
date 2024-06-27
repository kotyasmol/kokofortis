using Lextm.SharpSnmpLib;
using Lextm.SharpSnmpLib.Messaging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Properties;
using Variable = Lextm.SharpSnmpLib.Variable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Serilog;
using TFortisDeviceManager.Models.Devices;

namespace TFortisDeviceManager.Services
{
    public  interface IMonitoringEventService
    {
        Task Run();
        public event NotifyCollectionChangedEventHandler? SignalIfDeviceAdded;
        public event NotifyCollectionChangedEventHandler? SignalIfDeviceDeleted;
        public static event EventHandler<DeviceAddedEventArgs>? DeviceAddedForDashboard; // СНОВА ПОТРОГАЛ


        public void StartMonitoring();
        public void StartMonitoringRefreshMap(string deviceIp);
        public void ClearEvents();
        public void AddToDevicesTable(MonitoringDevice device);
        public void StopMonitoring();

    }

    public class MonitoringEventService : IMonitoringEventService, IDisposable
    {
        private readonly IUserSettings _userSettings;
        private readonly IMailSender _mailSender;
        private readonly INotificationService _notificationService;


        readonly string statusOk = Resources.StatusOk;
        readonly string statusProblem = Resources.StatusProblem;
        readonly string statusError = Resources.StatusError;
        readonly string statusInfo = Resources.StatusInfo;
        readonly string sensorErrorExceptionValue = Resources.SensorErrorExceptionValue;

        readonly string sensorNameHostStatus = Resources.SensorNameHostStatus;
        readonly string hostStatusEnabled = Resources.HostStatusEnabled;
        readonly string hostStatusDisabled = Resources.HostStatusDisabled;
        readonly string hostStatusReloaded = Resources.HostStatusReloaded;
       
        BlockingCollection<EventModel>? queueEvents;

        static CancellationTokenSource? ctsForProducer;
        static CancellationTokenSource? ctsForConsumer;

        ConcurrentBag<Task>? tasks;

        SnmpTrapDaemon? snmpTrapD;

        public static event EventHandler<ChangingDeviceStateEventArgs>? ChangingDeviceState; 
        public static event EventHandler<ChangingSensorPropertiesEventArgs>? ChangingSensorProperties;
        public static event EventHandler<ChangingDeviceStateEventArgs>? UpdateMapSettings;

        public event NotifyCollectionChangedEventHandler? SignalIfDeviceAdded;
        public event NotifyCollectionChangedEventHandler? SignalIfDeviceDeleted;
        public static  event EventHandler<DeviceAddedEventArgs>? DeviceAddedForDashboard; // СНОВА ПОТРОГАЛ

        Task? taskConsumer;
        public static ObservableCollection<EventModel> MonitoringEvents { get; } = new ObservableCollection<EventModel>();
        public static ObservableCollection<MonitoringDevice> MonitoringDevices { get; } = new ObservableCollection<MonitoringDevice>();

        public MonitoringEventService(ISettingsProvider settingsProvider, 
            IMailSender mailSender, 
            INotificationService notificationService)
        {
            _userSettings = settingsProvider.UserSettings;
            _mailSender = mailSender ?? throw new ArgumentNullException(nameof(mailSender));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));

            MonitoringDevices.CollectionChanged += (sender, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    SignalIfDeviceAdded?.Invoke(sender, e);
                }
                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    SignalIfDeviceDeleted?.Invoke(sender, e);
                }
            };
        }

        public void StartMonitoringRefreshMap(string deviceIp)
        {
            MonitoringEvents.Clear();

            UpdateMapSettings.Invoke(this, new ChangingDeviceStateEventArgs(deviceIp));

            Task.Run(() => Run());
            Log.Information("Мониторинг запущен");
        }

        public void StartMonitoring()
        {
            MonitoringEvents.Clear();

            Task.Run(() => Run());
            Log.Information("Мониторинг запущен");
        }

        public async Task Run()
        {
            bool trapEnable = _userSettings.MonitoringSettings.EnableRecieveTrap;
            queueEvents = new BlockingCollection<EventModel>(10000);
            ctsForProducer = new CancellationTokenSource();
            ctsForConsumer = new CancellationTokenSource();

            ctsForProducer.Token.Register(() => Console.WriteLine("ctsForProducer cancelled"));
            ctsForConsumer.Token.Register(() => Console.WriteLine("ctsForConsumer cancelled"));

            tasks = new ConcurrentBag<Task>();

            List<MonitoringDevice> devicesForMonitoring = PGDataAccess.GetDevicesForMonitoring(); // мониторинг нужен
         
            taskConsumer = AddInDBFromQueueEventsAsync(ctsForConsumer.Token);

            for (int n = 0; n < devicesForMonitoring.Count; n++)
            {
                tasks.Add(CheckUptimeLoopAsync(devicesForMonitoring[n], ctsForProducer.Token)); // цикл убрать, надо чтобы конкретное только одно
            }

            if (trapEnable)
            {
                snmpTrapD = new SnmpTrapDaemon(queueEvents, ctsForConsumer.Token);
                snmpTrapD.Start();
            }

            await Task.WhenAll(tasks);
            await taskConsumer;
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

            var sensors = PGDataAccess.LoadOidsForMonitroing(device.IpAddress); // потом надо будет потрогать 
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
                        string uptime;

                        uptime = $"{span.Days}{Resources.Day} {span.Hours}{Resources.Minute} {span.Minutes}{Resources.Minute}";

                        if (span.Days == 0)
                        uptime = $"{span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";

                        if(span.Days == 0 && span.Hours == 0)
                        uptime = uptime = $"{span.Minutes}{Resources.Minute}";

                        string description = $"{Properties.Resources.ContinuousOperationTime} {uptime}";

                        evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                            hostStatusEnabled, description, statusOk);
                    }

                    device.Uptime = uptimeValue;
                    PGDataAccess.SetUptime(device);

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
                    timeoutCount++;
                    if (timeoutCount == 5)
                    {
                        device.Available = false;

                        evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                                                 hostStatusDisabled, descriptionDeviceNotAvilable, statusProblem);

                        timeoutCount = 0;
                    }
                }
                catch (ErrorException ex)
                {
                    evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                                        sensorErrorExceptionValue, ex.Message, statusError);
                }
                catch (SnmpException)
                {
                    timeoutCount++;
                    if (timeoutCount == 5)
                    {
                        device.Available = false;

                        evnt = CreateEvent(device.Name, device.IpAddress, device.Description, device.Location, sensorNameHostStatus,
                                                 hostStatusDisabled, "SNMP Error", statusProblem);
                        timeoutCount = 0;
                    }
                }

                if (evnt != null)
                    queueEvents.TryAdd(evnt, 2, token);

                await Task.Delay(TimeSpan.FromSeconds(timeout), token).ConfigureAwait(false);
            }
        }

        private async Task RunReadSensorsAsync(List<Sensor> sensors, MonitoringDevice device, CancellationToken token)
        {
            int delayBetweenTaskReadSensorValueLoop = _userSettings.MonitoringSettings.DelayBetweenTaskReadSensorValueLoop;

            for (int n = 0; n < sensors.Count; n++)
            {
                if (sensors[n].Enable)
                    tasks.Add(ReadSensorValueLoop(sensors[n], device, token));
                else
                    SetEventStatusOld(sensors[n]); 

                await Task.Delay(delayBetweenTaskReadSensorValueLoop, token).ConfigureAwait(false);
            }
        }

        private async Task ReadSensorValueLoop(Sensor sensor, MonitoringDevice device, CancellationToken token) // вся суета тут кончается
        {
            int timeout = sensor.Timeout;
            string sensorValueText = "";

            var community = PGDataAccess.GetCommunity(device.IpAddress);

            while (true)
            {
                if (sensor.Enable)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Add ReadSensorValueLoop canceled.");
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

                        int sensorValue = -1;
                        sensorValue = GetSensorValue(sensor, community);
                        string status = statusOk;

                        if (sensor.Name == "temperature" || sensor.Name == "humidity")
                        {
                            sensorValueText = sensorValue.ToString(CultureInfo.InvariantCulture);
                            status = statusOk;
                        }
                        else
                        {
                            if (sensorValue == sensor.OkValue)
                            {
                                if (sensor.Invert) /// ДАТЧИК ПЕРЕВОРОТА МОЖНО ПОИСКАТЬ ДЛЯ ВСКРЫТИЯ И ТУДА ИНВЕРТ :3
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

                        if (sensor.Name == "upsPwrSource" && sensorValue == sensor.BadValue) 
                        {
                            double seconds = 0.0;
                            seconds = GetBatteryTime(sensor.Ip!, community);
                            TimeSpan timeLeft = TimeSpan.FromSeconds(seconds);
                            description = $"{Resources.TimeLeft} {timeLeft.Hours}h.{timeLeft.Minutes}m";
                        }
                        else
                        {
                            description = sensor.Description;
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
                else
                {
                    SetEventStatusOld(sensor); 
                }
            }
        }

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

        private async Task AddInDBFromQueueEventsAsync(CancellationToken token)
        {
            int timeout = _userSettings.MonitoringSettings.TimeoutReadQueueEvents;

            if (queueEvents != null)
            {
                while (!queueEvents.IsCompleted)
                {
                    try
                    {
                        if (!queueEvents.TryTake(out EventModel? evnt, 0, token))
                        {
                            await Task.Delay(TimeSpan.FromSeconds(timeout), token).ConfigureAwait(false);
                        }
                        else
                        {
                            if (evnt != null)
                                await ProcessEvent(evnt);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        Console.WriteLine("Taking canceled.");
                        break;
                    }
                }
            }
        }

        public void AddToDevicesTable(MonitoringDevice device) 
        {
            MonitoringDevices.Add(device);
            DeviceAddedForDashboard?.Invoke(this, new DeviceAddedEventArgs(device));

        }


        private async Task ProcessEvent(EventModel evnt) 
        {
            if (evnt.SensorName == sensorNameHostStatus)                                                
            {
                string sensorValue = evnt.SensorValueText ?? "";

                switch(sensorValue)
                {
                    case var value when value == hostStatusEnabled: 
                        HostStatusEnabledProcessing(evnt, sensorValue);
                        break;

                    case var value when value == hostStatusDisabled:
                        HostStatusDisabledProcessing(evnt, sensorValue);
                        break;

                    case var value when value == hostStatusReloaded:
                        HostStatusReloadedProcessing(evnt);
                        break;

                    case var value when value == sensorErrorExceptionValue:
                        SensorErrorExceptionProcessing(evnt, sensorValue);
                        break;

                    default: return;
                }               
            }
            else                                                                              
            {
                await ProcessOtherEvent(evnt);               
            }
        }

        private async Task ProcessOtherEvent(EventModel evnt)
        {
            EventModel? eventFromDb = PGDataAccess.SelectOneFromEvent(evnt);                    
            if (eventFromDb != null)                                                                              
            {
                bool equalSensorValue = eventFromDb.SensorValueText == evnt.SensorValueText;  
                bool equalStatus = eventFromDb.Status == evnt.Status;                         

                if (equalSensorValue && equalStatus)                                         
                {
                    UpdateAgeEvent(eventFromDb, evnt.Description);
                }
                else                                                                        
                {
                    PGDataAccess.UpdateEventStatusToOldWithTime(eventFromDb);           

                    PGDataAccess.AddEvent(evnt);

                    bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);
                
                    await _notificationService.SendNotificationAsync(evnt, shouldSendEmailForDeviceStatus);

                    PGDataAccess.UpdateEventStatusToOld(evnt, hostStatusDisabled);                 
                    PGDataAccess.UpdateEventStatusToOld(evnt, hostStatusEnabled);                   
                                                                                                             
                    DeleteOutdatedEvents(evnt);
                }

                ChangingSensorProperties?.Invoke(null, new ChangingSensorPropertiesEventArgs(evnt));
            }
            else                                                                            
            {
                PGDataAccess.AddEvent(evnt);                                            

                 bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);

                await _mailSender.SendEmailIfNeeded(evnt, shouldSendEmailForDeviceStatus);

                DeleteOutdatedEvents(evnt);
            }
        }

        private void HostStatusEnabledProcessing(EventModel evnt, string sensorValue)
        {
            EventModel? EvntFromDb = PGDataAccess.SelectOneEventHostStatus(evnt, sensorValue);

            if (EvntFromDb != null)
            {
                UpdateAgeEvent(EvntFromDb, evnt.Description);
            }
            else
            {
                PGDataAccess.AddEvent(evnt);
                PGDataAccess.UpdateEventStatusToOld(evnt, hostStatusDisabled);
                PGDataAccess.UpdateEventStatusToOld(evnt, sensorErrorExceptionValue);

                bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);
                _mailSender.SendEmailIfNeeded(evnt, shouldSendEmailForDeviceStatus);
            }

            ChangingDeviceState?.Invoke(null, new ChangingDeviceStateEventArgs(evnt.Ip, hostStatusEnabled));
        }

        private void HostStatusDisabledProcessing(EventModel evnt, string sensorValue)
        {
            EventModel? EvntFromDb = PGDataAccess.SelectOneEventHostStatus(evnt, sensorValue);
            if (EvntFromDb != null)
            {
                UpdateAgeEvent(EvntFromDb, evnt.Description);
            }
            else
            {
                PGDataAccess.SetOldAllEvent(evnt);
                PGDataAccess.AddEvent(evnt);

                bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);
                _mailSender.SendEmailIfNeeded(evnt, shouldSendEmailForDeviceStatus);
            }

            ChangingDeviceState?.Invoke(null, new ChangingDeviceStateEventArgs(evnt.Ip, hostStatusDisabled));
        }

        private void HostStatusReloadedProcessing(EventModel evnt)
        {
            PGDataAccess.AddEvent(evnt);

            bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);
            _mailSender.SendEmailIfNeeded(evnt, shouldSendEmailForDeviceStatus);
        }

        private void SensorErrorExceptionProcessing(EventModel evnt, string sensorValue)
        {
            EventModel? EvntFromDb = PGDataAccess.SelectOneEventHostStatus(evnt, sensorValue);

            if (EvntFromDb != null)
            {
                UpdateAgeEvent(EvntFromDb);
            }
            else
            {
                PGDataAccess.AddEvent(evnt);

                bool shouldSendEmailForDeviceStatus = PGDataAccess.GetSendEmail(evnt.Ip);
                _mailSender.SendEmailIfNeeded(evnt, shouldSendEmailForDeviceStatus);

                PGDataAccess.UpdateEventStatusToOld(evnt, hostStatusDisabled);
                PGDataAccess.UpdateEventStatusToOld(evnt, hostStatusEnabled);
            }
        }

        private void DeleteOutdatedEvents(EventModel evnt)
        {
            int eventStorageDays = _userSettings.MonitoringSettings.EventStorageDays;
            DateTime evntDateTime = DateTime.Parse(evnt.Time, CultureInfo.InvariantCulture);
            DateTime oldDateTime = evntDateTime.AddDays(eventStorageDays * (-1));
            PGDataAccess.DelEvent(oldDateTime.ToString(CultureInfo.InvariantCulture));
        }

        private static void SetEventStatusOld(Sensor sensor)
        {
            EventModel evnt = new()
            {
                DeviceName = sensor.DeviceName,
                SensorName = sensor.Name,
                Ip = sensor.Ip,
            };
            PGDataAccess.UpdateEventStatusToOld(evnt);
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

        private static void UpdateAgeEvent(EventModel eventFromDb)
        {
            var startTime = DateTime.ParseExact(eventFromDb.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            TimeSpan span = DateTime.Now.Subtract(startTime);

            string age = $"{span.Days}{Resources.Day} {span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";

            if (span.Days == 0)
                age = $"{span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";
            if (span.Days == 0 && span.Hours == 0)
                age = $"{span.Minutes}{Resources.Minute}";
            eventFromDb.Age = age;

            PGDataAccess.UpdateEventAge(eventFromDb);
        }

        private static void UpdateAgeEvent(EventModel eventFromDb, string? description)
        {
            var startTime = DateTime.ParseExact(eventFromDb.Time, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            TimeSpan span = DateTime.Now.Subtract(startTime);

            string age = $"{span.Days}{Resources.Day} {span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";

            if(span.Days == 0)
                age = $"{span.Hours}{Resources.Hour} {span.Minutes}{Resources.Minute}";
            if(span.Days == 0 && span.Hours == 0)
                age = $"{span.Minutes}{Resources.Minute}";
            eventFromDb.Age = age;

            PGDataAccess.UpdateEventAge(eventFromDb, description);
        }
        /// FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF




        // DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDD
        public void ClearEvents()
        {
            MonitoringEvents.Clear();
        }

        public void StopMonitoring()
        {
            try
            {

                snmpTrapD?.Stop();

                ctsForProducer?.Cancel();

                ctsForConsumer?.Cancel();

                Log.Information("Мониторинг остановлен");

            }
            catch (Exception ex)
            {
                Log.Error($"Ошибка при попытке завершения мониторинга. {ex.Message}");
            }

        }
        public void Dispose()
        {
            //
        }

    }
    // снова трогал
}
