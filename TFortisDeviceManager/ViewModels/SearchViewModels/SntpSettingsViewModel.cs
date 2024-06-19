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
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Linq;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class SntpSettingsViewModel : Screen, IDisposable
    {
        private readonly INetworkDeviceManager _networkDeviceManager;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IMessageBoxService _messageBoxService;
        private readonly IWindowManager _windowManager;

        public Dictionary<string, int> UTCDictionary { get; } = new();
        public List<NetworkDevice> FoundDevices { get; set; } = new List<NetworkDevice>();


        private NetworkDevice? _networkDevice;
        public NetworkDevice? NetworkDevice
        {
            get { return _networkDevice; }
            set { _networkDevice = value; OnPropertyChanged(nameof(NetworkDevice)); }
        }

        private string? _sntpServer;
        public string? SntpServer
        {
            get => _sntpServer;
            set
            {
                SetAndNotify(ref _sntpServer, value);
            }
        }

        private string _timezone = "-10";
        public string Timezone
        {
            get => _timezone;
            set
            {
                SetAndNotify(ref _timezone, value);
            }
        }

        private string? _sntpPeriod = "10";
        public string? SntpPeriod
        {
            get => _sntpPeriod;
            set
            {
                SetAndNotify(ref _sntpPeriod, value);
            }
        }

        private bool _sntpToAll;
        public bool SntpToAll
        {
            get => _sntpToAll;
            set
            {
                SetAndNotify(ref _sntpToAll, value);
            }
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

        private bool _settingApplyingInProgress;
        public bool SettingApplyingInProgress
        {
            get => _settingApplyingInProgress;
            set => SetAndNotify(ref _settingApplyingInProgress, value);
        }

        public SntpSettingsViewModel(ISettingsProvider settingsProvider,
                        IMessageBoxService messageBoxService,
                        IWindowManager windowManager,
                        INetworkDeviceManager networkDeviceManager)
        {
            UTCDictionary = TimezoneDictionary.GetTimezones();
            _networkDeviceManager = networkDeviceManager ?? throw new ArgumentNullException(nameof(networkDeviceManager));
            _applicationSettings = settingsProvider.ApplicationSettings;
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
        }


        private async Task SingleConfirmSntp(NetworkDevice device)
        {
            SettingApplyingInProgress = true;
            ErrorMessage = string.Empty;
            ErrorColor = string.Empty;

            bool dataChanged = false;

            int timeout = _applicationSettings.SettingsConfirmationTimeout;
            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken cancellationToken = cancelTokenSource.Token;

            device.ValidateGateway();

            NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.SetSntpSettingsAsync(device!, Login, Password, SntpServer, Timezone, SntpPeriod, cancellationToken);

            Log.Information($"Применение настроек: ip:{device.IpAddress}, описание:{device.Description}, местоположение:{device.Location}, шлюз:{device.Gateway}, маска:{device.NetworkMask}");

            AuthenticationViewModel authenticationViewModel = new();

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
                        if (dataChanged)
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.AuthenticationError, Properties.Resources.Message);
                            Log.Error($"Ошибка применения настроек: {Properties.Resources.AuthenticationError}");
                        }
                        else
                        {
                            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
                            cancellationToken = cancelTokenSource.Token;

                            _windowManager.ShowDialog(authenticationViewModel);
                            Login = authenticationViewModel.Login;
                            Password = authenticationViewModel.Password;

                            errorCode = await _networkDeviceManager.SetSntpSettingsAsync(device!, Login, Password, SntpServer, Timezone, SntpPeriod, cancellationToken);

                            dataChanged = true;
                        }

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

            SettingApplyingInProgress = false;
        }

        public async Task ConfirmSntpCommand(NetworkDevice device)
        {
            if (SntpToAll)
            {
                var selectedDevices = FoundDevices.Where(x => x.IsSelected == true);

                foreach (var dev in selectedDevices)
                {
                    await SingleConfirmSntp(dev);
                }
            }
            else
            {
                await SingleConfirmSntp(device);
            }
        }

        public void SetDevice(object? sender, ChangingSelectedDeviceEventArgs? e)
        {
            if (!IsNotNull(e) || !IsNotNull(e.Device)) return;

            NetworkDevice device = e.Device;

            NetworkDevice = device;

            FoundDevices = e.FoundDevices;          
        }
        private static bool IsNotNull([NotNullWhen(true)] object? obj) => obj != null;

        public void Dispose()
        {
            //
        }
    }
}
