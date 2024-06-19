using Stylet;
using System;
using System.Collections.Generic;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using System.Diagnostics.CodeAnalysis;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class ManualAddingViewModel : Screen, IDisposable
    {
        private readonly IWindowManager _windowManager;
        private readonly IMessageBoxService _messageBoxService;
        private readonly Func<AddingToMonitoringViewModel> _addingToMonitoringViewModelFactory;

        public List<string?> ModelsFromDb { get; set; } = new();

        private bool _canAddDeviceCommand;
        public bool CanAddDeviceCommand
        {
            get => _canAddDeviceCommand;
            set => SetAndNotify(ref _canAddDeviceCommand, value);
        }

        private string? _selectedModel;
        public string? SelectedModel
        {
            get => _selectedModel;
            set => SetAndNotify(ref _selectedModel, value);
        }

        private string? _modelErrorColor;
        public string? ModelErrorColor
        {
            get => _modelErrorColor;
            set => SetAndNotify(ref _modelErrorColor, value);
        }

        private string _ipAddress;
        public string? IpAddress
        {
            get => _ipAddress;
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    NotifyOfPropertyChange(nameof(IpAddress));
                }
            }
        }

        private NetworkDevice? _device;
        public NetworkDevice? Device
        {
            get => _device;
            set => SetAndNotify(ref _device, value);
        }



        public ManualAddingViewModel(IModelValidator<ManualAddingViewModel> validator,
                                     IWindowManager windowManager,
                                     IMessageBoxService messageBoxService,
                                     Func<AddingToMonitoringViewModel> addingToMonitoringViewModelFactory) : base(validator)
        {
            _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));
            _messageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            _addingToMonitoringViewModelFactory = addingToMonitoringViewModelFactory;

            ModelsFromDb = PGDataAccess.GetAllDeviceModels();

            CanAddDeviceCommand = false;

            DisplayName = "ManualAdding";

        }

        public void SelectionChanged()
        {
            ModelErrorColor= string.Empty;
        }

        public void AddToMonitoringCommand()
        {
            if(SelectedModel == null)
            {
                ModelErrorColor = "red";
                return;
            }

            if (SelectedModel != null && IpAddress != null)
            {
                Device = new NetworkDevice
                {
                    IpAddress = IpAddress,
                    Name = SelectedModel
                };
                Device.Id = (uint)PGDataAccess.GetDeviceTypeId(Device.Name);
                Device.SerialNumber = "unknown";
                Device.Mac = "unknown";
                Device.Description = "unknown";
                Device.Location = "unknown";

                using TfortisdbContext database = new();

                if (!database.Database.CanConnect())
                {
                    _messageBoxService.ShowError(Properties.Resources.DatabaseConnectError, Properties.Resources.StatusError);
                    return;
                }

                var deviceExists = PGDataAccess.CheckIfDeviceExists(Device.IpAddress);

                // Если такого устройства нет, то открываем окно с настройками
                if (!deviceExists)
                {
                    var addingToMonitoringViewModel = _addingToMonitoringViewModelFactory();
                    addingToMonitoringViewModel.InitConfigurator(Device);
                    _windowManager.ShowDialog(addingToMonitoringViewModel);
                }
                else
                {
                    _messageBoxService.ShowNotification(Properties.Resources.DeviceIsAlreadyAdded, Properties.Resources.StatusWarning);
                }
            }
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(HasErrors))
            {
                CanAddDeviceCommand = !HasErrors;
            }
            base.OnPropertyChanged(propertyName);
        }

        public void Dispose()
        {
            //
        }
    }
}
