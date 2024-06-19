using Microsoft.IdentityModel.Tokens;
using Serilog;
using Stylet;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class AddingToMonitoringViewModel : ValidatingModelBase
    {
        private readonly IMonitoringEventService _monitoringEventService;
        private readonly IMessageBoxService _messageBoxService;

        private NetworkDevice _networkDevice;

        public BindableCollection<OidModel> OidsViewSource { get; set; } = new BindableCollection<OidModel>();

        private readonly List<OidsForDevice> oidsForDevice = new(); 
        public bool IsNewEvents { get; set; } = true;
        public string Community { get; set; } = "public";
        public int SendEmail { get; set; }

        public string Name
        {
            get => _networkDevice.Name;
            set
            {
                if (IsNotNull(_networkDevice) && _networkDevice.Name != value)
                {
                    _networkDevice.Name = value;
                    NotifyOfPropertyChange(nameof(Name));
                }
            }
        }

        public string IpAddress
        {
            get => _networkDevice.IpAddress;
            set
            {
                if (IsNotNull(_networkDevice) && _networkDevice.IpAddress != value)
                {
                    _networkDevice.IpAddress = value;
                    NotifyOfPropertyChange(nameof(IpAddress));
                }
            }
        }
   
        public string Mac
        {
            get => _networkDevice.Mac;
            set
            {
                if (IsNotNull(_networkDevice) && _networkDevice.Mac != value)
                {
                    _networkDevice.Mac = value;
                    NotifyOfPropertyChange(nameof(Mac));
                }
            }
        }

        public string? Location
        {
            get => _networkDevice.Location;
            set
            {
                if (IsNotNull(_networkDevice) && _networkDevice.Location != value)
                {
                    _networkDevice.Location = value;
                    NotifyOfPropertyChange(nameof(Location));
                }
            }
        }

        public string? Description
        {
            get => _networkDevice.Description;
            set
            {
                if (IsNotNull(_networkDevice) && _networkDevice.Description != value)
                {
                    _networkDevice.Description = value;
                    NotifyOfPropertyChange(nameof(Description));
                }
            }
        }

        private bool _isOnOffChecked;
        public bool IsOnOffChecked
        {
            get => _isOnOffChecked;
            set => SetAndNotify(ref _isOnOffChecked, value);
        }

        private bool _checkSendEmail;
        public bool CheckSendEmail
        {
            get => _checkSendEmail;
            set => SetAndNotify(ref _checkSendEmail, value);
        }

        public AddingToMonitoringViewModel(IMonitoringEventService monitoringEventService, IMessageBoxService messageBoxService)
        {
            _monitoringEventService = monitoringEventService ?? throw new ArgumentNullException(nameof(monitoringEventService));

            CheckSendEmail = true;
            _messageBoxService = messageBoxService;
        }

        public void ReconfigureDevice(NetworkDevice device)
        {
            Log.Information("Нажата кнопка Настройки мониторинга");
          
            Log.Information($"Будет изменены сенсоры на [{device}]");

            _networkDevice = device.TryCreateDeepCopyAsync().Result;

            if (_networkDevice == null)
                return;
                
            SendEmail = Convert.ToInt32(PGDataAccess.GetSendEmail(_networkDevice.IpAddress));

            oidsForDevice.Clear();
            oidsForDevice.AddRange(PGDataAccess.GetOidsForMonitoring(_networkDevice.IpAddress).OrderBy(x => x.Description));

            foreach (var oid in oidsForDevice)
            {
                bool invert = false;

                if(oid.Invertible == 1)
                {
                    invert = PGDataAccess.GetSensorInvert(_networkDevice.IpAddress, _networkDevice.Name, oid.Name);
                }

                OidsViewSource.Add(oid.ToOidModel(invert));               
            }

    
            Community = PGDataAccess.GetCommunity(_networkDevice.IpAddress);

            IsNewEvents = false;
        }

        public void InitConfigurator(NetworkDevice device)
        {
            _networkDevice = device.TryCreateDeepCopyAsync().Result;

            SendEmail = Convert.ToInt32(PGDataAccess.GetSendEmail(_networkDevice.IpAddress));

            _networkDevice.Id = (uint)PGDataAccess.GetDeviceTypeId(device.Name);

     
                oidsForDevice.Clear();
                oidsForDevice.AddRange(PGDataAccess.GetOids(_networkDevice.Id).OrderBy(x => x.Description));
                foreach(var oid in oidsForDevice)
                {
                    if (Convert.ToBoolean(oid.Enable))
                    {
                        OidsViewSource.Add(oid.ToOidModel());
                    }
                }
            IsNewEvents = true;
        }
        
        public void CheckOnOffCommand()
        {
            if (IsOnOffChecked)
            {
                foreach (var oid in OidsViewSource)
                {
                    oid.Enable = true;
                }
                IsOnOffChecked = false;
            }
            else
            {
                foreach (var oid in OidsViewSource)
                {
                    oid.Enable = false;
                }
                IsOnOffChecked = true;
            }
        }

        public void CheckSendEmailCommand()
        {
            if (CheckSendEmail)
            {
                foreach (var oid in OidsViewSource)
                {
                    oid.SendEmail = true;
                }
                CheckSendEmail = false;
            }
            else
            {
                foreach (var oid in OidsViewSource)
                {
                    oid.SendEmail = false;
                }
                CheckSendEmail = true;
            }
        }

        public void ConfirmConfigurationCommand()
        {
            MonitoringDevice deviceToMonitoring = _networkDevice.ToMonitoringDevice();

            List<DeviceAndOid> deviceAndOids = new();
            if (IsNewEvents)
            {
                try
                {


                    _monitoringEventService.StopMonitoring();
                    PGDataAccess.AddDeviceForMonitoring(deviceToMonitoring, Community, SendEmail);

                    _monitoringEventService.AddToDevicesTable(deviceToMonitoring);

                    if (oidsForDevice.Count > 0)
                    {
                        foreach (var oid in OidsViewSource)
                        {
                            deviceAndOids.Add(
                                new DeviceAndOid
                                {
                                    DeviceForMonitoringKey = PGDataAccess.GetDeviceTypeId(deviceToMonitoring.Name),
                                    OidForDeviceKey = oid.Key,
                                    Timeout = oid.Timeout,
                                    Invert = oid.Invert,
                                    Invertible = oid.Invertible,
                                    SendEmail = oid.SendEmail,
                                    Enable = oid.Enable,
                                    DeviceIp = _networkDevice.IpAddress
                                });

                        }

                        PGDataAccess.SaveDeviceAndOids(deviceAndOids);
                    }
                    _monitoringEventService.StartMonitoring();
                }
                catch(Exception ex)
                {
                    Log.Error($"Ошибка добавления устройства в мониторинг. {ex.Message}");
                    _messageBoxService.ShowNotification("Ошибка добавления устройства в мониторинг.", ex.Message);
                }

            }
            else
            {
                try
                {


                    _monitoringEventService.StopMonitoring();

                    if (oidsForDevice.Count > 0)
                    {
                        foreach (var oid in OidsViewSource)
                        {

                            deviceAndOids.Add(
                                    new DeviceAndOid
                                    {
                                        DeviceForMonitoringKey = PGDataAccess.GetDeviceTypeId(deviceToMonitoring.Name),
                                        OidForDeviceKey = oid.Key,
                                        Timeout = oid.Timeout,
                                        Invert = oid.Invert,
                                        Invertible = oid.Invertible,
                                        SendEmail = oid.SendEmail,
                                        Enable = oid.Enable,
                                        DeviceIp= _networkDevice.IpAddress
                                    });

                        }

                        PGDataAccess.SaveDeviceAndOids(deviceAndOids);
                        PGDataAccess.SetCommunity(deviceToMonitoring.IpAddress, Community);
                        PGDataAccess.SetSendEmail(deviceToMonitoring.IpAddress, SendEmail);
                    }

                    _monitoringEventService.ClearEvents();
                    _monitoringEventService.StartMonitoringRefreshMap(deviceToMonitoring.IpAddress);
                }
                catch (Exception ex)
                {
                    Log.Error($"Ошибка добавления устройства в мониторинг. {ex.Message}");
                    _messageBoxService.ShowNotification("Ошибка добавления устройства в мониторинг.", ex.Message);
                }
            }
        }

        private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;
    }
}
