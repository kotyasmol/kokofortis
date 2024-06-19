using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
  

    public sealed class DeviceTableSettingsViewModel : ValidatingModelBase
    {
        private readonly IUserSettings _userSettings;

        private bool _showModelColumn;
        public bool ShowModelColumn
        {
            get => _showModelColumn;
            set => SetAndNotify(ref _showModelColumn, value);
        }

        private bool _showIpAddressColumn;
        public bool ShowIpAddressColumn
        {
            get => _showIpAddressColumn;
            set => SetAndNotify(ref _showIpAddressColumn, value);
        }

        private bool _showMacAddressColumn;
        public bool ShowMacAddressColumn
        {
            get => _showMacAddressColumn;
            set => SetAndNotify(ref _showMacAddressColumn, value);
        }

        private bool _showSerialNumberColumn;
        public bool ShowSerialNumberColumn
        {
            get => _showSerialNumberColumn;
            set => SetAndNotify(ref _showSerialNumberColumn, value);
        }

        private bool _showLocationColumn;
        public bool ShowLocationColumn
        {
            get => _showLocationColumn;
            set => SetAndNotify(ref _showLocationColumn, value);
        }

        private bool _showDescriptionColumn;
        public bool ShowDescriptionColumn
        {
            get => _showDescriptionColumn;
            set => SetAndNotify(ref _showDescriptionColumn, value);
        }


        public DeviceTableSettingsViewModel(ISettingsProvider settingsProvider)
        {
            _userSettings = settingsProvider.UserSettings;

            ShowModelColumn = _userSettings.MonitoringSettings.ShowModelColumn;
            ShowIpAddressColumn = _userSettings.MonitoringSettings.ShowIpAddressColumn;
            ShowMacAddressColumn = _userSettings.MonitoringSettings.ShowMacAddressColumn;
            ShowSerialNumberColumn = _userSettings.MonitoringSettings.ShowSerialNumberColumn;
            ShowLocationColumn = _userSettings.MonitoringSettings.ShowLocationColumn;
            ShowDescriptionColumn = _userSettings.MonitoringSettings.ShowDescriptionColumn;
        }

        public void ConfirmSettingsCommand()
        {
            _userSettings.MonitoringSettings.ShowModelColumn = ShowModelColumn;
            _userSettings.MonitoringSettings.ShowIpAddressColumn = ShowIpAddressColumn;
            _userSettings.MonitoringSettings.ShowMacAddressColumn = ShowMacAddressColumn;
            _userSettings.MonitoringSettings.ShowSerialNumberColumn = ShowSerialNumberColumn;
            _userSettings.MonitoringSettings.ShowLocationColumn = ShowLocationColumn;
            _userSettings.MonitoringSettings.ShowDescriptionColumn = ShowDescriptionColumn;

        }
    }
}
