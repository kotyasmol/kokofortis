using System;
using Stylet;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using TFortisDeviceManager.Database;
using System.Linq;
using HandyControl.Tools.Extension;
using System.Collections.Specialized;
using System.Windows;
using Microsoft.IdentityModel.Tokens;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using Serilog;
using Telegram.Bot.Types;
using System.Collections.ObjectModel;
using System.Threading;
using TFortisDeviceManager.Telegram;
using DynamicData;

namespace TFortisDeviceManager.ViewModels
{
    /// <summary>
    /// Логика взаимодействия для MonitoringView.xaml
    /// </summary>
    public sealed class MonitoringViewModel : Screen, IDisposable
    {
        private readonly IUserSettings _userSettings;
        private readonly IMonitoringEventService _monitoringEventService;
        private readonly MonitoringFiltersViewModel _monitoringFiltersViewModel;
        private readonly Func<DeviceTableSettingsViewModel> _deviceTableSettingsViewModelFactory;
        private readonly Func<EventTableSettingsViewModel> _eventTableSettingsViewModelFactory;
        private readonly Func<ManualAddingViewModel> _manualAddingViewModelFactory;
        private readonly IWindowManager _windowManager;
        private readonly Func<AddingToMonitoringViewModel> _addingToMonitoringViewModelFactory;

        bool filtersVisible;

        private int _filtersWidth;
        public int FiltersWidth
        {
            get => _filtersWidth;
            set => SetAndNotify(ref _filtersWidth, value);
        }
        
        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
        }

        private int? _selectedIndex = -1;
        public int? SelectedIndex
        {
            get => _selectedIndex;
            set => SetAndNotify(ref _selectedIndex, value);
        }

        private EventModel? _selectedEvent;
        public EventModel? SelectedEvent
        {
            get => _selectedEvent;
            set => SetAndNotify(ref _selectedEvent, value);
        }


        #region DevicesTableSetting
        private Visibility _showModelColumn;
        public Visibility ShowModelColumn
        {
            get => _showModelColumn;
            set => SetAndNotify(ref _showModelColumn, value);
        }

        private Visibility _showIpAddressColumn;
        public Visibility ShowIpAddressColumn
        {
            get => _showIpAddressColumn;
            set => SetAndNotify(ref _showIpAddressColumn, value);
        }

        private Visibility _showMacAddressColumn;
        public Visibility ShowMacAddressColumn
        {
            get => _showMacAddressColumn;
            set => SetAndNotify(ref _showMacAddressColumn, value);
        }

        private Visibility _showSerialNumberColumn;
        public Visibility ShowSerialNumberColumn
        {
            get => _showSerialNumberColumn;
            set => SetAndNotify(ref _showSerialNumberColumn, value);
        }

        private Visibility _showLocationColumn;
        public Visibility ShowLocationColumn
        {
            get => _showLocationColumn;
            set => SetAndNotify(ref _showLocationColumn, value);
        }

        private Visibility _showDescriptionColumn;
        public Visibility ShowDescriptionColumn
        {
            get => _showDescriptionColumn;
            set => SetAndNotify(ref _showDescriptionColumn, value);
        }
        #endregion

        #region EventsTableSetting
        private Visibility _showTimeColumn;
        public Visibility ShowTimeColumn
        {
            get => _showTimeColumn;
            set => SetAndNotify(ref _showTimeColumn, value);
        }

        private Visibility _showNameColumn;
        public Visibility ShowNameColumn
        {
            get => _showNameColumn;
            set => SetAndNotify(ref _showNameColumn, value);
        }

        private Visibility _showEventIpAddressColumn;
        public Visibility ShowEventIpAddressColumn
        {
            get => _showEventIpAddressColumn;
            set => SetAndNotify(ref _showEventIpAddressColumn, value);
        }

        private Visibility _showSensorColumn;
        public Visibility ShowSensorColumn
        {
            get => _showSensorColumn;
            set => SetAndNotify(ref _showSensorColumn, value);
        }

        private Visibility _showValueColumn;
        public Visibility ShowValueColumn
        {
            get => _showValueColumn;
            set => SetAndNotify(ref _showValueColumn, value);
        }

        private Visibility _showAgeColumn;
        public Visibility ShowAgeColumn
        {
            get => _showAgeColumn;
            set => SetAndNotify(ref _showAgeColumn, value);
        }

        private Visibility _showEventDescriptionColumn;
        public Visibility ShowEventDescriptionColumn
        {
            get => _showEventDescriptionColumn;
            set => SetAndNotify(ref _showEventDescriptionColumn, value);
        }

        private Visibility _showStateColumn;
        public Visibility ShowStateColumn
        {
            get => _showStateColumn;
            set => SetAndNotify(ref _showStateColumn, value);
        }

        private Visibility _showEventLocationColumn;
        public Visibility ShowEventLocationColumn
        {
            get => _showEventLocationColumn;
            set => SetAndNotify(ref _showEventLocationColumn, value);
        }

        private Visibility _showDeviceDescriptionColumn;
        public Visibility ShowDeviceDescriptionColumn
        {
            get => _showDeviceDescriptionColumn;
            set => SetAndNotify(ref _showDeviceDescriptionColumn, value);
        }

        private Visibility _showConfirmColumn;
        public Visibility ShowConfirmColumn
        {
            get => _showConfirmColumn;
            set => SetAndNotify(ref _showConfirmColumn, value);
        }
        #endregion

        private MonitoringDevice? _selectedDevice;
        public MonitoringDevice? SelectedDevice
        {
            get => _selectedDevice;
            set => SetAndNotify(ref _selectedDevice, value);
        }

        public static ObservableCollection<MonitoringDevice> DevicesForMonitoring { get; set; } = new();
        public BindableCollection<string> SelectedModels { get; set; } = new();
        public BindableCollection<string> SelectedAdresses { get; set; } = new();
        public BindableCollection<string> SelectedParameters { get; set; } = new();
        public BindableCollection<string> SelectedStates { get; set; } = new();

        public BindableCollection<EventModel> MonitoringEvents { get; set; } = new();

        public List<string> SelectedDevices { get; set; } = new();

        public MonitoringViewModel(MonitoringFiltersViewModel monitoringFiltersViewModel,
                        IMonitoringEventService monitoringEventService,
                        ISettingsProvider settingsProvider,
                        IWindowManager windowManager,
                        Func<DeviceTableSettingsViewModel> deviceTableSettingsViewModelFactory,
                        Func<EventTableSettingsViewModel> eventTableSettingsViewModelFactory,
                        Func<ManualAddingViewModel> manualAddingViewModelFactory,
                        Func<AddingToMonitoringViewModel> addingToMonitoringViewModelFactory)
        {
            _monitoringFiltersViewModel = monitoringFiltersViewModel ?? throw new ArgumentNullException(nameof(monitoringFiltersViewModel));
            _userSettings = settingsProvider.UserSettings;
            _monitoringEventService = monitoringEventService ?? throw new ArgumentNullException(nameof(monitoringEventService));
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _deviceTableSettingsViewModelFactory = deviceTableSettingsViewModelFactory ?? throw new ArgumentNullException(nameof(deviceTableSettingsViewModelFactory));
            _eventTableSettingsViewModelFactory = eventTableSettingsViewModelFactory ?? throw new ArgumentNullException(nameof(eventTableSettingsViewModelFactory));
            _manualAddingViewModelFactory = manualAddingViewModelFactory;

            _selectedViewModel = _monitoringFiltersViewModel ?? throw new ArgumentNullException(nameof(_monitoringFiltersViewModel)); 
            _addingToMonitoringViewModelFactory = addingToMonitoringViewModelFactory ?? throw new ArgumentNullException(nameof(addingToMonitoringViewModelFactory)); 

            MonitoringEventService.MonitoringDevices.CollectionChanged += DeviceListChanged;

            LoadEventListForMonitoring();
            LoadDeviceListForMonitoring();

            UpdateComboBoxes();

            Task.Run(() => RunUpdateScreenAsync(5));

            DisplayName = Properties.Resources.Monitoring;
        }

        void DeviceListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is MonitoringDevice newDevice)
                        DevicesForMonitoring.Add(newDevice);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is MonitoringDevice oldDevice)
                        DevicesForMonitoring.Remove(oldDevice);
                    break;
            }

            UpdateComboBoxes();
        }

        /// <summary>
        /// Загрузка списка устройств, добавленных в мониторинг.
        /// </summary>
        
        private void LoadDeviceListForMonitoring()
        {
            try
            {
                var devices = PGDataAccess.GetDevicesForMonitoring();

                foreach (var device in devices)
                {
                    var eventsForDevice = PGDataAccess.LoadEventsWithoutOldWithDeviceAsync(device);
                    eventsForDevice = AgregateEvents(eventsForDevice);
                }

                MonitoringEventService.MonitoringDevices.Clear();
                MonitoringEventService.MonitoringDevices.AddRange(devices);

                _monitoringEventService.StartMonitoring();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task LoadEventListForMonitoring()
        {
            try
            {
                List<EventModel> eventsFromDb = new();

                foreach(var device in MonitoringEventService.MonitoringDevices)
                {

                    var eventsForDevice = PGDataAccess.LoadEventsWithoutOldWithDeviceAsync(device);

                    eventsFromDb.AddRange(eventsForDevice.OrderBy(x => x.Ip).ThenBy(x => x.SensorName));

                    eventsFromDb = AgregateEvents(eventsFromDb);

                    bool state = false;

                    foreach(var evnt in eventsForDevice)
                    {
                        bool selected = SelectedAdresses.IsNullOrEmpty() || SelectedAdresses.Contains(device.IpAddress);

                        if (!selected) break;

                        if(evnt.Status == "Problem")
                        {
                            state = true;
                            break;
                        }
                    }

                    device.State = state;                 
                }

                var newEvents = eventsFromDb.Except(MonitoringEvents).ToList();

                var oldEvents = MonitoringEvents.Except(eventsFromDb).ToList();


                if (!newEvents.IsNullOrEmpty() )
                {
                    AddNewEvents(newEvents);
                }

                if (!oldEvents.IsNullOrEmpty())
                {
                    DeleteOldEvents(oldEvents);
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void DeleteOldEvents(List<EventModel> oldEvents)
        {
            int? selectedEvent = SelectedIndex;

            foreach (var evnt in oldEvents)
            {
                MonitoringEvents.Remove(evnt);
            }

            SelectedIndex = selectedEvent;
        }

        private void AddNewEvents(List<EventModel> newEvents)
        {
            int? selectedEvent = SelectedIndex;
        
            foreach (var evnt in newEvents)
            {
                var oldEvent = MonitoringEvents.Where(x => x.Ip == evnt.Ip && x.SensorName == evnt.SensorName).FirstOrDefault();

                if (oldEvent != null)
                    MonitoringEvents.Replace(oldEvent, evnt);
                else
                    MonitoringEvents.Add(evnt);
            }

            SelectedIndex = selectedEvent;         
        }

        public void AddSelectedDeviceCommand()
        {
            if (SelectedDevice == null) return;
            if(!SelectedAdresses.Contains(SelectedDevice.IpAddress))
                SelectedAdresses.Add(SelectedDevice.IpAddress);
        }

        public void RemoveSelectedDeviceCommand()
        {
            if (SelectedDevice == null) return;

            if (SelectedAdresses.Contains(SelectedDevice.IpAddress))
                SelectedAdresses.Remove(SelectedDevice.IpAddress);
        }

        public void SelectDeviceCommand()
        {
            if (SelectedDevice == null) return;

            if(SelectedDevice.IsSelected)
            {
                SelectedAdresses.Add(SelectedDevice.IpAddress);
            }
            else
            {
                SelectedAdresses.Remove(SelectedDevice.IpAddress);
            }
        }

        private List<EventModel> AgregateEvents(List<EventModel> events)
        {
            List<EventModel> agregatedEvents = new();

            List<EventModel> agregatedModels = new();
            List<EventModel> agregatedAddresses = new();
            List<EventModel> agregatedParameters = new();
            List<EventModel> agregatedStates = new();

            agregatedEvents.AddRange(events);

            if (SelectedModels.Count > 0)
            {
                foreach (var model in SelectedModels)
                {
                    agregatedModels.AddRange(agregatedEvents.Where(t => t.DeviceName == model).ToList());
                    agregatedEvents.AddRange(agregatedModels);
                }

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedModels);
            }

            if (SelectedAdresses.Count > 0)
            {
                foreach (var address in SelectedAdresses)
                {
                    agregatedAddresses.AddRange(agregatedEvents.Where(t => t.Ip == address).ToList());
                    agregatedEvents.AddRange(agregatedAddresses);
                }

               

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedAddresses);
            }

            if (SelectedParameters.Count > 0)
            {
                foreach (var parameter in SelectedParameters)
                {
                    agregatedParameters.AddRange(agregatedEvents.Where(t => t.SensorName == parameter).ToList());
                    agregatedEvents.AddRange(agregatedParameters);
                }

                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedParameters);
            }

            if (SelectedStates.Count > 0)
            {
                foreach (var state in SelectedStates)
                {
                    agregatedStates.AddRange(agregatedEvents.Where(t => t.Status == state).ToList());
                    agregatedEvents.AddRange(agregatedStates);
                }
                agregatedEvents.Clear();
                agregatedEvents.AddRange(agregatedStates);
            }

            //agregatedEvents = agregatedEvents.Distinct().ToList();

            return agregatedEvents;
           
        }      

        /// <summary>
        /// Обновляем список событий на экране
        /// </summary>        
        private async Task RunUpdateScreenAsync(int timeoutUpdate)
        {
            while(true)
            {
                await Task.Run(() => LoadEventListForMonitoring());

                await Task.Run(() => UpdateDeviceTableSetting());
                await Task.Run(() => UpdateEventsTableSetting());

                await Task.Delay(timeoutUpdate * 1000);
            }
        }

        private void UpdateDeviceTableSetting()
        {
            if (_userSettings.MonitoringSettings.ShowModelColumn)
                ShowModelColumn = Visibility.Visible;
            else
                ShowModelColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowIpAddressColumn)
                ShowIpAddressColumn = Visibility.Visible;
            else
                ShowIpAddressColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowMacAddressColumn)
                ShowMacAddressColumn = Visibility.Visible;
            else
                ShowMacAddressColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowSerialNumberColumn)
                ShowSerialNumberColumn = Visibility.Visible;
            else
                ShowSerialNumberColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowLocationColumn)
                ShowLocationColumn = Visibility.Visible;
            else
                ShowLocationColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowDescriptionColumn)
                ShowDescriptionColumn = Visibility.Visible;
            else
                ShowDescriptionColumn = Visibility.Hidden;
        }

        private void UpdateEventsTableSetting()
        {
            if (_userSettings.MonitoringSettings.ShowTimeColumn)
                ShowTimeColumn = Visibility.Visible;
            else
                ShowTimeColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowNameColumn)
                ShowNameColumn = Visibility.Visible;
            else
                ShowNameColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowEventIpAddressColumn)
                ShowEventIpAddressColumn = Visibility.Visible;
            else
                ShowEventIpAddressColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowSensorColumn)
                ShowSensorColumn = Visibility.Visible;
            else
                ShowSensorColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowValueColumn)
                ShowValueColumn = Visibility.Visible;
            else
                ShowValueColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowAgeColumn)
                ShowAgeColumn = Visibility.Visible;
            else
                ShowAgeColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowEventDescriptionColumn)
                ShowEventDescriptionColumn = Visibility.Visible;
            else
                ShowEventDescriptionColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowStateColumn)
                ShowStateColumn = Visibility.Visible;
            else
                ShowStateColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowEventLocationColumn)
                ShowEventLocationColumn = Visibility.Visible;
            else
                ShowEventLocationColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowDeviceDescriptionColumn)
                ShowDeviceDescriptionColumn = Visibility.Visible;
            else
                ShowDeviceDescriptionColumn = Visibility.Hidden;
            if (_userSettings.MonitoringSettings.ShowConfirmColumn)
                ShowConfirmColumn = Visibility.Visible;
            else
                ShowConfirmColumn = Visibility.Hidden;
        }

        private void UpdateComboBoxes()
        {
            try
            {
                var eventsFromDb = PGDataAccess.LoadEventsWithoutOldAsync();
                if (!eventsFromDb.IsNullOrEmpty())
                    _monitoringFiltersViewModel.InitComboBoxes(eventsFromDb);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void SetFiltersCommand(object parameters)
        {
            var filters = (object[])parameters;
            var selectedModels = (IList)filters[0];
            var selectedAddresses = (IList)filters[1];
            var selectedParameters = (IList)filters[2];
            var selectedStates = (IList)filters[3];

            SelectedModels.Clear();
            SelectedAdresses.Clear();
            SelectedParameters.Clear();
            SelectedStates.Clear();

            if (selectedModels.Count > 0)
            {
                foreach (var model in selectedModels)
                {
                    SelectedModels.Add(model.ToString());
                }
            }

            if (selectedAddresses.Count > 0)
            {
                foreach (var address in selectedAddresses)
                {
                    SelectedAdresses.Add(address.ToString());
                }

                foreach(var device in DevicesForMonitoring)
                {
                    device.IsSelected = SelectedAdresses.Contains(device.IpAddress);
                }
            }
            else
            {
                foreach(var device in DevicesForMonitoring)
                {
                    if(device.IsSelected)
                        SelectedAdresses.Add(device.IpAddress);
                }
            }

            if (selectedParameters.Count > 0)
            {
                foreach (var parameter in selectedParameters)
                {               
                    if(parameter == "Host status")
                    {
                        SelectedParameters.Add(Properties.Resources.SensorNameHostStatus);
                    }
                    SelectedParameters.Add(parameter.ToString());
                }
            }

            if (selectedStates.Count > 0)
            {
                foreach (var state in selectedStates)
                {
                    SelectedStates.Add(state.ToString());
                }
            }

            foreach(var device in DevicesForMonitoring)
            {
                device.IsSelected = false;
            }

            Log.Information("Фильтры применены");

            LoadEventListForMonitoring();
        }

        public void SetDefaultFiltersCommand()
        {
            _monitoringFiltersViewModel.ModelsFromDataFromDb.Clear();
            _monitoringFiltersViewModel.IpsFromDataFromDb.Clear();
            _monitoringFiltersViewModel.ParametersFromDb.Clear();
            _monitoringFiltersViewModel.EventStatusFromDb.Clear();

            SelectedModels.Clear();
            SelectedAdresses.Clear();
            SelectedParameters.Clear();
            SelectedStates.Clear();

            foreach(var device in DevicesForMonitoring)
            {
                device.IsSelected = false;
            }

            UpdateComboBoxes();

            LoadEventListForMonitoring();
        }

        public void DeleteFromMonitoringCommand()
        {
            _monitoringEventService.StopMonitoring();

            if (SelectedDevice != null)
            {
                PGDataAccess.DelDevice(SelectedDevice);

                PGDataAccess.SetOldAllEvent(SelectedDevice);
                var newEventList = MonitoringEventService.MonitoringEvents.Where(e => e.DeviceName != SelectedDevice.Name && e.Ip != SelectedDevice.IpAddress).ToList();
                MonitoringEventService.MonitoringEvents.Clear();
                MonitoringEventService.MonitoringEvents.AddRange(newEventList);
                MonitoringEventService.MonitoringDevices.DeleteIfExists(SelectedDevice);
                LoadEventListForMonitoring();
            }

            _monitoringEventService.StartMonitoring();
        }

        public void ConfigureDevice()
        {

            NetworkDevice device = SelectedDevice.ToNetworkDevice();

            if (device == null) return;
            var addingToMonitoringViewModel = _addingToMonitoringViewModelFactory();
            addingToMonitoringViewModel.ReconfigureDevice(device);

            Log.Information($"Открыто окно повторной конфигурации для {device.IpAddress}");
            _windowManager.ShowDialog(addingToMonitoringViewModel);

        }

        public void AddDeviceToMap()
        {
            if (SelectedDevice == null) return;
            MapViewModel.AddToMap(SelectedDevice);
        }

        public void ConfirmEventReloaded()
        {
            Log.Information("Нажата кнопка [Подтвердить] - событие reloaded Error");
            if (SelectedEvent != null)
            {
                PGDataAccess.UpdateEventStatusToOldWithTime(SelectedEvent, SelectedEvent.SensorValueText);

                LoadEventListForMonitoring();
            }
        }

        public void AddDeviceManually()
        {
            var manualAddingViewModel = _manualAddingViewModelFactory();
            Log.Information($"Открыто окно для добавления устройства в мониторинг вручную");

            _windowManager.ShowDialog(manualAddingViewModel);
        }

        public void ShowFiltersCommand()
        {
            if (!filtersVisible)
            {
                FiltersWidth = 300;
                filtersVisible = true;
            }
            else
            {
                FiltersWidth = 0;
                filtersVisible = false; 
            }
        }

        public void TableSortCommand(object sender, DataGridSortingEventArgs e)
        {
            if (sender is not DataGrid dataGrid) return;

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
            }
        }

        public void Dispose()
        {

        }
    }
}
