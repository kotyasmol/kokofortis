using Stylet;
using System.Threading;
using System;
using System.Threading.Tasks;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using System.Diagnostics.CodeAnalysis;
using Serilog;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.IdentityModel.Tokens;
using System.Drawing;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class DeviceSettingsViewModel : Screen, IDisposable
    {
        private readonly INetworkDeviceManager _networkDeviceManager;
        private readonly IApplicationSettings _applicationSettings;

        public ObservableCollection<PortSettings> Ports { get; set; } = new ObservableCollection<PortSettings>();

        private NetworkDevice _networkDevice;
        public NetworkDevice NetworkDevice
        {
            get { return _networkDevice; }
            set { _networkDevice = value; OnPropertyChanged(nameof(NetworkDevice)); }
        }


        private bool _canStartConfirmCommand = true;
        public bool CanStartConfirmCommand
        {
            get => _canStartConfirmCommand;
            set => SetAndNotify(ref _canStartConfirmCommand, value);
        }

        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                _networkDevice.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _ipAddress = "";
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                _ipAddress = value;
                _networkDevice.IpAddress = value;
                OnPropertyChanged(nameof(IpAddress));
                ValidateProperty();
            }
        }

        private string _networkMask = "";
        public string NetworkMask
        {
            get => _networkMask;
            set
            {
                _networkMask = value;
                _networkDevice.NetworkMask = value;
                OnPropertyChanged(nameof(NetworkMask));
                ValidateProperty();
            }
        }

        private string _gateway = "";
        public string Gateway
        {
            get => _gateway;
            set
            {
                _gateway = value;
                _networkDevice.Gateway = value;
                OnPropertyChanged(nameof(Gateway));
                ValidateProperty();
            }
        }

        private string? _location;
        public string? Location
        {
            get => _location;
            set
            {
                _location = value;
                _networkDevice.Location = value;
                OnPropertyChanged(nameof(Location));
            }
        }

        private string? _descripion;
        public string? Description
        {
            get => _descripion;
            set
            {
                _descripion = value;
                _networkDevice.Description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private bool _snmpEnabled;
        public bool SnmpEnabled
        {
            get => _snmpEnabled;
            set => SetAndNotify(ref _snmpEnabled, value);
        }        

        private string? _login;
        public string? Login
        {
            get => _login;
            set => SetAndNotify(ref _login, value);
        }

        private string? _password;
        public string? Password
        {
            get => _password;
            set => SetAndNotify(ref _password, value);
        }

        private bool _isProgress;
        public bool IsProgress 
        {
            get => _isProgress;
            set => SetAndNotify(ref _isProgress, value);
        }

        private bool _isChecked;
        public bool IsChecked 
        {
            get => _isChecked;
            set => SetAndNotify(ref _isChecked, value);
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get => _errorMessage;
            set => SetAndNotify(ref _errorMessage, value);
        }

        private string? _errorColor;
        public string? ErrorColor
        {
            get => _errorColor;
            set => SetAndNotify(ref _errorColor, value);
        }

        public static string Title => Properties.Resources.DeviceSettingsTitle;

        public DeviceSettingsViewModel(IApplicationSettings applicationSettings,
                                       INetworkDeviceManager networkDeviceManager,
                                       IModelValidator<DeviceSettingsViewModel> validator) : base(validator)
        {
            _networkDeviceManager = networkDeviceManager ?? throw new ArgumentNullException(nameof(networkDeviceManager));
            _applicationSettings = applicationSettings ?? throw new ArgumentNullException(nameof(applicationSettings));
        }

        public async Task ConfirmSettingsCommandAsync()
        {
            IsProgress = true;
            ErrorMessage = string.Empty;
            ErrorColor = string.Empty;
            
            int timeout = _applicationSettings.SettingsConfirmationTimeout;
            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken cancellationToken = cancelTokenSource.Token;

            _networkDevice.ValidateGateway();

            NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.SetSettingsAsync(_networkDevice!, Login, Password, SnmpEnabled, Ports, cancellationToken);

            Log.Information($"Применение настроек: ip:{_networkDevice.IpAddress}, описание:{_networkDevice.Description}, местоположение:{_networkDevice.Location}, шлюз:{_networkDevice.Gateway}, маска:{_networkDevice.NetworkMask}");

            switch (errorCode)
            {
                case NetworkDeviceManagerErrorCode.Accepted:
                    {
                        ErrorColor = "Green";
                        ErrorMessage = Properties.Resources.SettingsSetSuccessful;

                        Log.Information(Properties.Resources.SettingsSetSuccessful);

                        break;
                    }
                case NetworkDeviceManagerErrorCode.AuthenticationError:
                    {
                        ErrorColor = "Red";
                        ErrorMessage = Properties.Resources.AuthenticationError;

                        Log.Error($"Ошибка применения настроек: {Properties.Resources.AuthenticationError}");
                        break;
                    }
                case NetworkDeviceManagerErrorCode.Timeout:
                    {
                        ErrorColor = "Red";
                        ErrorMessage = Properties.Resources.DeviceNotRespond;

                        Log.Error($"Ошибка применения настроек: {Properties.Resources.DeviceNotRespond}");

                        break;
                    }
                default:
                    {
                        ErrorColor = "Red";
                        ErrorMessage = Properties.Resources.DeviceNotRespond;

                        Log.Error($"Ошибка применения настроек: {Properties.Resources.DeviceNotRespond}");

                        break;
                    }
            }

            IsProgress = false;
        }

        public void SetDevice(object? sender, ChangingSelectedDeviceEventArgs? e)
        {
            if (!IsNotNull(e) || !IsNotNull(e.Device)) return;

            NetworkDevice device = e.Device;

            NetworkDevice = device;

            Name = device.Name;
            IpAddress = device.IpAddress;
            NetworkMask = device.NetworkMask;
            Gateway = device.Gateway;
            Description = device.Description;
            Location = device.Location;

            if (device.Ports.IsNullOrEmpty()) return;

            for(int i = 0; i < device.Ports.Count; i++)
            {
                if (!device.Ports[i].IsSfp)
                {
                    PortSettings portSettingsRow = new(i + 1, device.Ports[i].Poe, false, false);
                    Ports.Add(portSettingsRow);
                }
            }

            ErrorColor = "";
            ErrorMessage = "";
        }

        protected override void OnPropertyChanged(string propertyName)
        {          
            if (propertyName == nameof(IsProgress))
            {
                CanStartConfirmCommand = !IsProgress;
            }
            base.OnPropertyChanged(propertyName);
        }

        private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;

        public void Dispose()
        {
            //
        }
    }
}