    using HandyControl.Themes;
using Microsoft.Win32;
using Serilog;
using Stylet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;

namespace TFortisDeviceManager.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для SearchView.xaml
    /// </summary>
    public sealed class SearchViewModel : Screen, IDisposable
    {
        private readonly IDeviceSearcher _deviceSearcher;
        private readonly IUserSettings _userSettings;
        private readonly IApplicationSettings _applicationSettings;
        private readonly IWindowManager _windowManager;
        private readonly IMessageBoxService _messageBoxService;
        private readonly INetworkDeviceManager _networkDeviceManager;
        private readonly Func<AddingToMonitoringViewModel> _addingToMonitoringViewModelFactory;
        private readonly DeviceSettingsViewModel _deviceSettingsViewModel;
        private readonly SntpSettingsViewModel _sntpSettingsViewModel;
        private readonly DfuUpdateViewModel _dfuUpdateViewModel;

        public BindableCollection<NetworkDevice> FoundDevices { get; } = new BindableCollection<NetworkDevice>();

        public static event EventHandler<ChangingSelectedDeviceEventArgs>? ChangingSelectedDevice;
        public static event EventHandler<EventArgs>? SettingsConfirmed;
        public static event EventHandler<ChangingSelectedDeviceEventArgs>? SntpSettingsConfirmed;
        public static event EventHandler<EventArgs>? DfuUploadStarted;
        public static event EventHandler<EventArgs>? DfuUpdateStarted;
        public static event EventHandler<EventArgs>? SelectUpdateFile;

        private bool _groupSettingApplyingInProgress;
        public bool GroupSettingApplyingInProgress
        {
            get => _groupSettingApplyingInProgress;
            set => SetAndNotify(ref _groupSettingApplyingInProgress, value);
        }      

        private bool _canStartStubSearchCommand = true;
        public bool CanStartSearchCommand
        {
            get => _canStartStubSearchCommand;
            set => SetAndNotify(ref _canStartStubSearchCommand, value);
        }

        private int _camerasListWidth = 500;
        public int CamerasListWidth
        {
            get => _camerasListWidth;
            set => SetAndNotify(ref _camerasListWidth, value);
        }

        private bool _isCamerasListVisible = true;
        public bool IsCamerasListVisible
        {
            get => _isCamerasListVisible;
            set => SetAndNotify(ref _isCamerasListVisible, value);
        }

        private string? _fromIpAddress;
        public string? FromIpAddress
        {
            get => _fromIpAddress;
            set
            {
                SetAndNotify(ref _fromIpAddress, value);
                ValidateProperty();
            }
        }

        private string? _toIpAddress;
        public string? ToIpAddress
        {
            get => _toIpAddress;
            set
            {
                SetAndNotify(ref _toIpAddress, value);
                ValidateProperty();
            }
        }

        private bool _isAutoSearch;
        public bool IsAutoSearch
        {
            get => _isAutoSearch;
            set => SetAndNotify(ref _isAutoSearch, value);
        }

        private bool _isProgress;
        public bool IsProgress
        {
            get => _isProgress;
            set => SetAndNotify(ref _isProgress, value);
        }      

        private bool _groupSnmpEnabled;
        public bool GroupSnmpEnabled
        {
            get => _groupSnmpEnabled;
            set => SetAndNotify(ref _groupSnmpEnabled, value);
        }

        private bool _groupLldpEnabled;
        public bool GroupLldpEnabled
        {
            get => _groupLldpEnabled;
            set => SetAndNotify(ref _groupLldpEnabled, value);
        }

        private string? _groupSettingsApplyingProgress;
        public string? GroupSettingsApplyingProgress
        {
            get => _groupSettingsApplyingProgress;
            set
            {
                SetAndNotify(ref _groupSettingsApplyingProgress, value);
            }
        }

        private object _deviceSettingsTabViewModel;
        public object DeviceSettingsTabViewModel
        {
            get { return _deviceSettingsTabViewModel; }
            set { _deviceSettingsTabViewModel = value; OnPropertyChanged(nameof(DeviceSettingsTabViewModel)); }
        }

        private object _dfuUpdateTabViewModel;
        public object DfuUpdateTabViewModel
        {
            get { return _dfuUpdateTabViewModel; }
            set { _dfuUpdateTabViewModel = value; OnPropertyChanged(nameof(DfuUpdateTabViewModel)); }
        }

        private object _sntpSettingsTabViewModel;
        public object SntpSettingsTabViewModel
        {
            get { return _sntpSettingsTabViewModel; }
            set { _sntpSettingsTabViewModel = value; OnPropertyChanged(nameof(SntpSettingsTabViewModel)); }
        }

        private Visibility _showLoading;
        public Visibility ShowLoading
        {
            get => _showLoading;
            set => SetAndNotify(ref _showLoading, value);
        }

        private NetworkDevice? _selectedDevice;
        public NetworkDevice? SelectedDevice
        {
            get => _selectedDevice;
            set => SetAndNotify(ref _selectedDevice, value);
        }

        public SearchViewModel(IDeviceSearcher deviceSearcher,
            IModelValidator<SearchViewModel> validator,
            ISettingsProvider settingsProvider,
            IWindowManager windowManager,
            IMessageBoxService messageBoxService,
            INetworkDeviceManager networkDeviceManager,
            DeviceSettingsViewModel deviceSettingsViewModel,
            SntpSettingsViewModel sntpSettingsViewModel,
            DfuUpdateViewModel dfuUpdateViewModel,
            Func<AddingToMonitoringViewModel> addingToMonitoringViewModelFactory) : base(validator)
        {
            DisplayName = Properties.Resources.Search;

            _deviceSearcher = deviceSearcher ?? throw new ArgumentNullException(nameof(deviceSearcher));
            if (settingsProvider is null) throw new ArgumentNullException(nameof(settingsProvider));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _networkDeviceManager = networkDeviceManager ?? throw new ArgumentNullException(nameof(networkDeviceManager));
            _userSettings = settingsProvider.UserSettings;
            _applicationSettings = settingsProvider.ApplicationSettings;
            _addingToMonitoringViewModelFactory = addingToMonitoringViewModelFactory;
            _deviceSettingsViewModel = deviceSettingsViewModel ?? throw new ArgumentNullException(nameof(deviceSettingsViewModel));
            _sntpSettingsViewModel = sntpSettingsViewModel ?? throw new ArgumentNullException(nameof(sntpSettingsViewModel));
            _dfuUpdateViewModel = dfuUpdateViewModel ?? throw new ArgumentNullException(nameof(dfuUpdateViewModel));
            _deviceSettingsTabViewModel = _deviceSettingsViewModel ?? throw new ArgumentNullException(nameof(_deviceSettingsViewModel));
            _sntpSettingsTabViewModel = _sntpSettingsViewModel ?? throw new ArgumentNullException(nameof(_sntpSettingsViewModel));
            _dfuUpdateTabViewModel = _dfuUpdateViewModel ?? throw new ArgumentNullException(nameof(_dfuUpdateViewModel));

            IsAutoSearch = true;
            AutoValidate = false;

            ChangingSelectedDevice += _deviceSettingsViewModel.SetDevice;
            ChangingSelectedDevice += _sntpSettingsViewModel.SetDevice;
            ChangingSelectedDevice += _dfuUpdateViewModel.SetDevice;


            SettingsConfirmed += async (s, e) =>
            {
                await _deviceSettingsViewModel.ConfirmSettingsCommandAsync();
            };

            SntpSettingsConfirmed += async (s, e) =>
            {
                await _sntpSettingsViewModel.ConfirmSntpCommand(e.Device);
            };

            DfuUploadStarted += async (s, e) =>
            {
                await _dfuUpdateViewModel.UploadDfuCommand();
            };

            DfuUpdateStarted += async (s, e) =>
            {
                await _dfuUpdateViewModel.UpdateDfuCommand();
            };

            SelectUpdateFile +=  (s, e) =>
            {
                _dfuUpdateViewModel.SelectFileCommand();
            };

            ThemeManager.Current.UsingSystemTheme = true;
        }

        public async Task StartSearchCommand()
        {
            if (GroupSettingApplyingInProgress == true)
            {
                return;
            }
            IsProgress = true;

            int timeout = _userSettings.SearchSettings.SearchTimeout;
            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken token = cancelTokenSource.Token;


            IEnumerable<NetworkDevice> devices;
          
            FoundDevices.Clear();

            if (string.IsNullOrEmpty(FromIpAddress) && string.IsNullOrEmpty(ToIpAddress))
            {
                devices = await _deviceSearcher.SearchBroadcastAsync(token); //Поиск
            }
            else
            {
                if (string.IsNullOrEmpty(FromIpAddress))
                {
                    FromIpAddress = ToIpAddress;
                }

                if (string.IsNullOrEmpty(ToIpAddress))
                {
                    ToIpAddress = FromIpAddress;
                }

                devices = await _deviceSearcher.SearchUnicastAsync(FromIpAddress!, ToIpAddress!, token); //Поиск
            }

            using TfortisdbContext database = new();

            if (database.Database.CanConnect())
            {
                foreach (var device in devices)
                {
                    device.InMonitoring = PGDataAccess.CheckIfDeviceExists(device.IpAddress);
                }
            }
         
            FoundDevices.AddRange(devices);

            IsProgress = false;
        }

        public void SelectAllCommand(bool isChecked)
        {
            if (isChecked)
            {
                foreach (var device in FoundDevices)
                {
                    device.IsSelected = true;
                }
            }
            else
            {
                foreach (var device in FoundDevices)
                {
                    device.IsSelected = false;
                }
            }
        }

        #region TabBarCommands

        public void ConfirmSettingsCommand()
        {
            SettingsConfirmed?.Invoke(this, EventArgs.Empty);
        }
        public void ConfirmSntpCommand()
        {
            SntpSettingsConfirmed?.Invoke(this, new ChangingSelectedDeviceEventArgs(SelectedDevice, FoundDevices.ToList()));
        }

        public void UploadDfuCommand()
        {
            DfuUploadStarted?.Invoke(this, EventArgs.Empty);
        }
        public void UpdateDfuCommand()
        {
            DfuUpdateStarted?.Invoke(this, EventArgs.Empty);
        }
        public void SelectFileCommand()
        {
            SelectUpdateFile?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public void OpenInBrowserCommand()
        {
            if (SelectedDevice == null || SelectedDevice.IpAddress == null)
                return;

            try
            {
                var sInfo = new ProcessStartInfo($"http://{SelectedDevice.IpAddress}")
                {
                    UseShellExecute = true,
                };

                Log.Information($"Открыт веб интерфейс {SelectedDevice.IpAddress}");

                Process.Start(sInfo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task RebootDeviceCommand()
        {
            if (SelectedDevice is not NetworkDevice selectedDevice)
                return;

            int timeout = _applicationSettings.SettingsConfirmationTimeout;

            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken cancellationToken = cancelTokenSource.Token;

            var result = _messageBoxService.ShowQuestion(Properties.Resources.RebootDeviceConfirmation, Properties.Resources.Confirmation);
            if (result == MessageBoxResult.No)
                return;

            string? login = null;
            string? password = null;

            bool done = false;
            bool dataChanged = false;

            AuthenticationViewModel authenticationViewModel = new();

            while (!done)
            {
                NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.RebootDeviceAsync(PGDataAccess.Parse(selectedDevice.Mac), login, password, cancellationToken);
                switch (errorCode)
                {
                    case NetworkDeviceManagerErrorCode.Accepted:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.RebootSuccessful, Properties.Resources.Message);

                            Log.Information($"Устройство перезапущено {selectedDevice.Mac}");

                            done = true;
                            break;
                        }
                    case NetworkDeviceManagerErrorCode.ConfigError:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.ConfigError, Properties.Resources.Message);
                            done = true;

                            Log.Error($"Ошибка перезапуска {selectedDevice.Mac}. {Properties.Resources.ConfigError}");

                            break;
                        }
                    case NetworkDeviceManagerErrorCode.AuthenticationError:
                        {
                            if (dataChanged)
                            {
                                _messageBoxService.ShowNotification(Properties.Resources.AuthenticationError, Properties.Resources.Message);

                                Log.Error($"Ошибка перезапуска {selectedDevice.Mac}. {Properties.Resources.AuthenticationError}");

                                done = true;
                            }
                            else
                            {
                                _windowManager.ShowDialog(authenticationViewModel);
                                login = authenticationViewModel.Login;
                                password = authenticationViewModel.Password;
                                dataChanged = true;
                            }
                            break;
                        }
                    case NetworkDeviceManagerErrorCode.Timeout:
                        {
                            Log.Error($"Ошибка перезапуска {selectedDevice.Mac}. Таймаут");

                            done = true;
                            break;
                        }
                    default:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.DeviceNotRespond, Properties.Resources.Message);

                            Log.Error($"Ошибка перезапуска {selectedDevice.Mac}. Устройство не отвечает");

                            done = true;
                            break;
                        }
                }
            }
        }

        public async Task ResetSettingsCommand()
        {
            if (SelectedDevice is not NetworkDevice selectedDevice)
                return;

            int timeout = _applicationSettings.SettingsConfirmationTimeout;

            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken cancellationToken = cancelTokenSource.Token;

            var result = _messageBoxService.ShowQuestion(Properties.Resources.ResetSettings, Properties.Resources.Confirmation);
            if (result == MessageBoxResult.No)
                return;

            string? login = null;
            string? password = null;

            bool done = false;
            bool dataChanged = false;

            AuthenticationViewModel authenticationViewModel = new();
            while (!done)
            {
                NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.SetDefaultSettingsAsync(PGDataAccess.Parse(selectedDevice.Mac), login, password, cancellationToken);

                switch (errorCode)
                {
                    case NetworkDeviceManagerErrorCode.Accepted:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.ResetSuccessful, Properties.Resources.Message);

                            Log.Information($"Настройки сброшены {selectedDevice.Mac}");

                            done = true;
                            break;
                        }
                    case NetworkDeviceManagerErrorCode.ConfigError:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.ConfigError, Properties.Resources.Message);

                            Log.Error($"Ошибка сброса настроек {selectedDevice.Mac}. {Properties.Resources.ConfigError}");

                            done = true;
                            break;
                        }
                    case NetworkDeviceManagerErrorCode.AuthenticationError:
                        {
                            if (dataChanged)
                            {
                                _messageBoxService.ShowNotification(Properties.Resources.AuthenticationError, Properties.Resources.Message);

                                Log.Error($"Ошибка сброса настроек {selectedDevice.Mac}. {Properties.Resources.AuthenticationError}");

                                done = true;
                            }
                            else
                            {
                                _windowManager.ShowDialog(authenticationViewModel);
                                login = authenticationViewModel.Login;
                                password = authenticationViewModel.Password;
                                dataChanged = true;
                            }
                            break;
                        }
                    case NetworkDeviceManagerErrorCode.Timeout:
                        {
                            Log.Error($"Ошибка сброса настроек {selectedDevice.Mac}. Таймаут");

                            done = true;
                            break;
                        }
                    default:
                        {
                            _messageBoxService.ShowNotification(Properties.Resources.DeviceNotRespond, Properties.Resources.Message);

                            Log.Error($"Ошибка сброса настроек {selectedDevice.Mac}. {Properties.Resources.DeviceNotRespond}");

                            done = true;
                            break;
                        }
                }
            }
        }

        public async Task ImportSettingsCommand()
        {
            if (SelectedDevice is not NetworkDevice selectedDevice)
                return;

            int timeout = _applicationSettings.SettingsConfirmationTimeout;

            using CancellationTokenSource cancelTokenSource = new();
            cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(timeout));
            CancellationToken cancellationToken = cancelTokenSource.Token;

            OpenFileDialog openFileDialog = new()
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 0
            };

            if (openFileDialog.ShowDialog() != true)
                return;

            string filePath = openFileDialog.FileName;

            string? login = null;
            string? password = null;

            bool done = false;
            bool dataChanged = false;

            AuthenticationViewModel authenticationViewModel = new();

            while (!done)
            {
                try
                {
                    NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.SetSettingsFromFileAsync(PGDataAccess.Parse(selectedDevice.Mac), login, password, filePath, cancellationToken); // Отправляем строку настроек
                    
                    Log.Information($"Импорт настроек {selectedDevice.Mac}");

                    switch (errorCode)
                    {
                        case NetworkDeviceManagerErrorCode.FileIsBig:
                            {
                                _messageBoxService.ShowError(Properties.Resources.FileTooBig, Properties.Resources.Message);

                                Log.Error($"Ошибка импорта настроек {selectedDevice.Mac}. {Properties.Resources.FileTooBig}");

                                done = true;
                                break;
                            }
                        case NetworkDeviceManagerErrorCode.Accepted:
                            {
                                _messageBoxService.ShowNotification(Properties.Resources.ApplySettingsSuccessful, Properties.Resources.Message);

                                Log.Information($"Импорт настроек {selectedDevice.Mac}. Успешно");

                                done = true;
                                break;
                            }
                        case NetworkDeviceManagerErrorCode.AuthenticationError:
                            {
                                if (dataChanged)
                                {
                                    _messageBoxService.ShowNotification(Properties.Resources.AuthenticationError, Properties.Resources.Message);

                                    Log.Error($"Ошибка импорта настроек {selectedDevice.Mac}. {Properties.Resources.AuthenticationError}");

                                    done = true;
                                }
                                else
                                {
                                    _windowManager.ShowDialog(authenticationViewModel);
                                    login = authenticationViewModel.Login;
                                    password = authenticationViewModel.Password;
                                    dataChanged = true;
                                }
                                break;
                            }
                        case NetworkDeviceManagerErrorCode.ConfigError:
                            {
                                _messageBoxService.ShowNotification(Properties.Resources.ConfigError, Properties.Resources.Message);

                                Log.Error($"Ошибка импорта настроек {selectedDevice.Mac}. {Properties.Resources.ConfigError}");

                                done = true;
                                break;
                            }
                        default:
                            {
                                _messageBoxService.ShowNotification(Properties.Resources.DeviceNotRespond, Properties.Resources.Message);

                                Log.Error($"Ошибка импорта настроек {selectedDevice.Mac}. {Properties.Resources.DeviceNotRespond}");

                                done = true;
                                break;
                            }
                    }
                }
                catch (IOException ex)
                {
                    _messageBoxService.ShowError($"Failed to open file {filePath}. {ex.Message}", Properties.Resources.Message);

                    Log.Error($"Failed to open file {filePath}. {ex.Message}");

                    done = true;
                }
                catch (Exception ex)
                {
                    _messageBoxService.ShowError($"Failed to open file {filePath}. {ex.Message}", Properties.Resources.Message);

                    Log.Error($"Failed to open file {filePath}. {ex.Message}");

                    done = true;
                }
            }
        }

        public void AddToMonitoringCommand()
        {
            if (SelectedDevice == null)
                return;
            using TfortisdbContext database = new();

            if (!database.Database.CanConnect())
            {
                _messageBoxService.ShowError(Properties.Resources.DatabaseConnectError, Properties.Resources.StatusError);

                Log.Error($"Ошибка подключения к базе данных");

                return;
            }

            var deviceExists = PGDataAccess.CheckIfDeviceExists(SelectedDevice.IpAddress);

            if (!deviceExists)
            {
                var addingToMonitoringViewModel = _addingToMonitoringViewModelFactory();
                addingToMonitoringViewModel.InitConfigurator(SelectedDevice);

                Log.Information($"Открыто окно конфигурации и добавления в мониторинг");

                _windowManager.ShowDialog(addingToMonitoringViewModel);
            }
            else
            {
                _messageBoxService.ShowNotification(Properties.Resources.DeviceIsAlreadyAdded, Properties.Resources.StatusWarning);

                Log.Information($"Попытка добавить устройство {SelectedDevice.IpAddress} в мониторинг повторно.");

            }
        }

        public void TableSortCommand(object sender, DataGridSortingEventArgs e)
        {
            DataGrid? dataGrid = sender as DataGrid;
            string sortPropertyName = e.Column.SortMemberPath;

            if (!string.IsNullOrEmpty(sortPropertyName))
            {
                if (sortPropertyName == "IpAddress")
                {
                    e.Handled = true;
                    ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
                    e.Column.SortDirection = direction;
                    ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                    IComparer comparer = new SortIPAddress(direction);
                    lcv.CustomSort = comparer;
                }

                /*if (sortPropertyName == "Status")
                {
                    IComparer comparer = null;
                    e.Handled = true;
                    ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
                    e.Column.SortDirection = direction;
                    ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                    comparer = new SortState(direction);
                    lcv.CustomSort = comparer;
                }*/
            }
        }

        public async Task ConfirmGroupSettingsCommand()
        {
            GroupSettingApplyingInProgress = true;
            int count = 0;

            foreach(var device in FoundDevices)
            {
                if (device.IsSelected)
                { 
                using CancellationTokenSource cancelTokenSource = new();
                cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
                CancellationToken cancellationToken = cancelTokenSource.Token;

                string login = "";
                string password = "";
                bool dataChanged = false;

                AuthenticationViewModel authenticationViewModel = new();

                GroupSettingsApplyingProgress = $"{Properties.Resources.SettingsConfirmed} : {count}/{FoundDevices.Count}";

                NetworkDeviceManagerErrorCode errorCode = await _networkDeviceManager.SetGroupSettingsAsync(device, login, password, GroupSnmpEnabled, GroupLldpEnabled, cancellationToken);

                    switch (errorCode)
                    {
                        case NetworkDeviceManagerErrorCode.Accepted:
                            {
                                count++;

                                GroupSettingsApplyingProgress = $"{Properties.Resources.SettingsConfirmed}: {count}/{FoundDevices.Count}";

                                Log.Information(Properties.Resources.SettingsSetSuccessful);

                                break;
                            }
                        case NetworkDeviceManagerErrorCode.AuthenticationError:
                            {

                                if (dataChanged)
                                {
                                    _messageBoxService.ShowNotification(Properties.Resources.AuthenticationError, Properties.Resources.Message);

                                    Log.Error($"Ошибка импорта настроек {device.Mac}. {Properties.Resources.AuthenticationError}");

                                }
                                else
                                {
                                    cancelTokenSource.CancelAfter(TimeSpan.FromSeconds(10));
                                    cancellationToken = cancelTokenSource.Token;

                                    _windowManager.ShowDialog(authenticationViewModel);
                                    login = authenticationViewModel.Login;
                                    password = authenticationViewModel.Password;



                                    errorCode = await _networkDeviceManager.SetGroupSettingsAsync(device, login, password, GroupSnmpEnabled, GroupLldpEnabled, cancellationToken);

                                    if (errorCode == NetworkDeviceManagerErrorCode.Accepted)
                                    {
                                        count++;
                                        GroupSettingsApplyingProgress = $"{Properties.Resources.SettingsConfirmed}: {count}/{FoundDevices.Count}";
                                    }

                                    dataChanged = true;

                                }
                                Log.Error($"Ошибка применения настроек: {Properties.Resources.AuthenticationError}");

                                break;
                            }
                        case NetworkDeviceManagerErrorCode.Timeout:
                            {

                                Log.Error($"Ошибка применения настроек: {Properties.Resources.DeviceNotRespond}");

                                break;
                            }
                        default:
                            {

                                Log.Error($"Ошибка применения настроек: {Properties.Resources.DeviceNotRespond}");

                                break;
                            }
                    }
                }            
            }
            GroupSettingApplyingInProgress = false;

        }

        public void ChangeSelectionCommand()
        {
            ChangingSelectedDevice?.Invoke(this, new ChangingSelectedDeviceEventArgs(SelectedDevice, FoundDevices.ToList()));
        }

        public void ShowCamerasList()
        {
            if (!IsCamerasListVisible)
                {
                    CamerasListWidth = 500;
                    IsCamerasListVisible = true;
                }
                else
                {
                    CamerasListWidth = 0;
                    IsCamerasListVisible = false;
                }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(HasErrors))
            {
                CanStartSearchCommand = !HasErrors;
            }
            else if (propertyName == nameof(IsAutoSearch) && IsAutoSearch)
            {
                FromIpAddress = null;
                ToIpAddress = null;
            }
            else if (propertyName == nameof(IsProgress))
            {
                CanStartSearchCommand = !IsProgress;

                if (IsProgress)
                {
                    ShowLoading = Visibility.Visible;
                }
                else
                {
                    ShowLoading = Visibility.Hidden;
                }
            }
            base.OnPropertyChanged(propertyName);
        }
        // ТРОГАЛ
        /*private readonly MonitoringEventService _eventService;

        public ObservableCollection<MonitoringDevice> Devices { get; set; }

        public SearchViewModel(MonitoringEventService eventService)
        {
            _eventService = eventService;
            Devices = new ObservableCollection<MonitoringDevice>();
        }

        private MonitoringDevice _selectedDeviceDash;
        public MonitoringDevice SelectedDeviceDash
        {
            get { return _selectedDeviceDash; }
            set
            {
                _selectedDeviceDash = value;
                OnPropertyChangedDash();
                _eventService.AddDevice(_selectedDeviceDash);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChangedDash([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }*/
        // НЕ ТРОГАЛ
        public void Dispose()
        {
            //
        }
    }
}