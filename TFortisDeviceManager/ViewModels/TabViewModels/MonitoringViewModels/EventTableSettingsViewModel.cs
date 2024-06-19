using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class EventTableSettingsViewModel : ValidatingModelBase
    {
        private readonly IUserSettings _userSettings;

        private bool _showTimeColumn;
        public bool ShowTimeColumn
        {
            get => _showTimeColumn;
            set => SetAndNotify(ref _showTimeColumn, value);
        }

        private bool _showNameColumn;
        public bool ShowNameColumn
        {
            get => _showNameColumn;
            set => SetAndNotify(ref _showNameColumn, value);
        }

        private bool _showEventIpAddressColumn;
        public bool ShowEventIpAddressColumn
        {
            get => _showEventIpAddressColumn;
            set => SetAndNotify(ref _showEventIpAddressColumn, value);
        }

        private bool _showSensorColumn;
        public bool ShowSensorColumn
        {
            get => _showSensorColumn;
            set => SetAndNotify(ref _showSensorColumn, value);
        }

        private bool _showValueColumn;
        public bool ShowValueColumn
        {
            get => _showValueColumn;
            set => SetAndNotify(ref _showValueColumn, value);
        }

        private bool _showAgeColumn;
        public bool ShowAgeColumn
        {
            get => _showAgeColumn;
            set => SetAndNotify(ref _showAgeColumn, value);
        }

        private bool _showEventDescriptionColumn;
        public bool ShowEventDescriptionColumn
        {
            get => _showEventDescriptionColumn;
            set => SetAndNotify(ref _showEventDescriptionColumn, value);
        }

        private bool _showStateColumn;
        public bool ShowStateColumn
        {
            get => _showStateColumn;
            set => SetAndNotify(ref _showStateColumn, value);
        }

        private bool _showEventLocationColumn;
        public bool ShowEventLocationColumn
        {
            get => _showEventLocationColumn;
            set => SetAndNotify(ref _showEventLocationColumn, value);
        }

        private bool _showDeviceDescriptionColumn;
        public bool ShowDeviceDescriptionColumn
        {
            get => _showDeviceDescriptionColumn;
            set => SetAndNotify(ref _showDeviceDescriptionColumn, value);
        }

        public EventTableSettingsViewModel(ISettingsProvider settingsProvider)
        {
            _userSettings = settingsProvider.UserSettings;

            ShowTimeColumn = _userSettings.MonitoringSettings.ShowTimeColumn;
            ShowNameColumn = _userSettings.MonitoringSettings.ShowNameColumn;
            ShowEventIpAddressColumn = _userSettings.MonitoringSettings.ShowEventIpAddressColumn;
            ShowSensorColumn = _userSettings.MonitoringSettings.ShowSensorColumn;
            ShowValueColumn = _userSettings.MonitoringSettings.ShowValueColumn;
            ShowAgeColumn = _userSettings.MonitoringSettings.ShowAgeColumn;
            ShowEventDescriptionColumn = _userSettings.MonitoringSettings.ShowEventDescriptionColumn;
            ShowStateColumn = _userSettings.MonitoringSettings.ShowStateColumn;
            ShowEventLocationColumn = _userSettings.MonitoringSettings.ShowEventLocationColumn;
            ShowDeviceDescriptionColumn = _userSettings.MonitoringSettings.ShowDeviceDescriptionColumn;
        }

        public void ConfirmSettingsCommand()
        {
            _userSettings.MonitoringSettings.ShowTimeColumn = ShowTimeColumn;
            _userSettings.MonitoringSettings.ShowNameColumn = ShowNameColumn;
            _userSettings.MonitoringSettings.ShowEventIpAddressColumn = ShowEventIpAddressColumn;
            _userSettings.MonitoringSettings.ShowSensorColumn = ShowSensorColumn;
            _userSettings.MonitoringSettings.ShowValueColumn = ShowValueColumn;
            _userSettings.MonitoringSettings.ShowAgeColumn = ShowAgeColumn;
            _userSettings.MonitoringSettings.ShowEventDescriptionColumn = ShowEventDescriptionColumn;
            _userSettings.MonitoringSettings.ShowStateColumn = ShowStateColumn;
            _userSettings.MonitoringSettings.ShowEventLocationColumn = ShowEventLocationColumn;
            _userSettings.MonitoringSettings.ShowDeviceDescriptionColumn = ShowDeviceDescriptionColumn;

        }
    }
}
