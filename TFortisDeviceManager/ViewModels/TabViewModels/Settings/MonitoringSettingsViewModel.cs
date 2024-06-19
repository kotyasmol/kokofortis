using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using HandyControl.Controls;
using HandyControl.Tools.Extension;
using MimeKit;
using Serilog;
using Stylet;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;

namespace TFortisDeviceManager.ViewModels;

public sealed class MonitoringSettingsViewModel : Screen, IDisposable
{
    private readonly IUserSettings _userSettings;
    private readonly IWindowManager _windowManager;
    private readonly Func<EventsExportViewModel> _eventsExportViewModelFactory;

    public int SNMPTimeout
    {
        get => _userSettings.MonitoringSettings.TimeoutWaitingForResponseSnmp;
        set
        {
            _userSettings.MonitoringSettings.TimeoutWaitingForResponseSnmp = value;
            NotifyOfPropertyChange(nameof(SNMPTimeout));
        }
    }
    public int UptimeCheckPeriod
    {
        get => _userSettings.MonitoringSettings.TimeoutCheckUptime;
        set
        {
            _userSettings.MonitoringSettings.TimeoutCheckUptime = value;
            NotifyOfPropertyChange(nameof(UptimeCheckPeriod));
        }
    }
    public int SensorReadPeriod
    {
        get => _userSettings.MonitoringSettings.DelayBetweenTaskReadSensorValueLoop;
        set
        {
            _userSettings.MonitoringSettings.DelayBetweenTaskReadSensorValueLoop = value;
            NotifyOfPropertyChange(nameof(SensorReadPeriod));
        }
    }

    public int HideNotificationsAfter
    {
        get => _userSettings.MonitoringSettings.HideNotificationsAfter;
        set
        {
            _userSettings.MonitoringSettings.HideNotificationsAfter = value;
            NotifyOfPropertyChange(nameof(HideNotificationsAfter));
        }
    }

    public bool IsAutoHideNotifications
    {
        get => _userSettings.MonitoringSettings.IsAutoHideNotifications;
        set
        {
            _userSettings.MonitoringSettings.IsAutoHideNotifications = value;
            NotifyOfPropertyChange(nameof(IsAutoHideNotifications));
        }
    }

    public ObservableCollection<string> DeviceColumnsVisibility { get; set; } = new();
    public ObservableCollection<string> EventColumnsVisibility { get; set; } = new();

    public List<string> SelectedDeviceColumns { get; } = new();
    public List<string> SelectedEventColumns { get; } = new();



    public MonitoringSettingsViewModel(IUserSettings userSettings,
                                       IWindowManager windowManager,
                                       Func<EventsExportViewModel> eventsExportViewModelFactory)
    {
        this.DisplayName = Properties.Resources.MonitoringSettings;

        _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
        _eventsExportViewModelFactory = eventsExportViewModelFactory;
        _windowManager = windowManager ?? throw new ArgumentNullException(nameof(windowManager));

        DeviceColumnsVisibility = new() 
        {
            Properties.Resources.Mac,
            Properties.Resources.Location,
            Properties.Resources.Description
        };

        EventColumnsVisibility = new()
        {
            Properties.Resources.Location,
            Properties.Resources.DeviceDescription,
            Properties.Resources.Sensor,
            Properties.Resources.Age,
        };

        if (_userSettings.MonitoringSettings.ShowMacAddressColumn)
            SelectedDeviceColumns.Add(Properties.Resources.Mac);
        if (_userSettings.MonitoringSettings.ShowLocationColumn)
            SelectedDeviceColumns.Add(Properties.Resources.Location);
        if (_userSettings.MonitoringSettings.ShowDescriptionColumn)
            SelectedDeviceColumns.Add(Properties.Resources.Description);

        if (_userSettings.MonitoringSettings.ShowEventLocationColumn)
            SelectedEventColumns.Add(Properties.Resources.Location);
        if (_userSettings.MonitoringSettings.ShowDeviceDescriptionColumn)
            SelectedEventColumns.Add(Properties.Resources.DeviceDescription);
        if (_userSettings.MonitoringSettings.ShowSensorColumn)
            SelectedEventColumns.Add(Properties.Resources.Sensor);
        if (_userSettings.MonitoringSettings.ShowAgeColumn)
            SelectedEventColumns.Add(Properties.Resources.Age);
    }

    public void SetDeviceTableSettings(object parameters)
    {
        var settings = (object[])parameters;
        var deviceTableSettings = (IList)settings[0];
        var eventTableSettings = (IList)settings[1];

        if(deviceTableSettings != null)    
        {
            _userSettings.MonitoringSettings.ShowMacAddressColumn = deviceTableSettings.Contains(Properties.Resources.Mac);
            _userSettings.MonitoringSettings.ShowLocationColumn = deviceTableSettings.Contains(Properties.Resources.Location);
            _userSettings.MonitoringSettings.ShowDescriptionColumn = deviceTableSettings.Contains(Properties.Resources.Description);
        }

        if (deviceTableSettings != null)
        {
            _userSettings.MonitoringSettings.ShowEventLocationColumn = eventTableSettings.Contains(Properties.Resources.Location);
            _userSettings.MonitoringSettings.ShowDeviceDescriptionColumn = eventTableSettings.Contains(Properties.Resources.DeviceDescription);
            _userSettings.MonitoringSettings.ShowSensorColumn = eventTableSettings.Contains(Properties.Resources.Sensor);
            _userSettings.MonitoringSettings.ShowAgeColumn = eventTableSettings.Contains(Properties.Resources.Age);
        }
    }

    public void ExportEventsCommand()
    {
        var exportEventsViewModel = _eventsExportViewModelFactory();

        Log.Information("Открыто окно экспорта событий");
        _windowManager.ShowDialog(exportEventsViewModel);
    }

    public void Dispose()
    {
        //
    }
}