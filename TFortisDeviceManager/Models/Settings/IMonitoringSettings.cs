using Stylet;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public interface IMonitoringSettings : INotifyPropertyChanged
    {
        [DefaultValue(true)]
        bool EnableRecieveTrap { get; set; }

        [DefaultValue(90)]
        int EventStorageDays { get; set; }

        [DefaultValue(5)]
        int TimeoutWaitingForResponseSnmp { get; set; }

        [DefaultValue(10)]
        int TimeoutCheckUptime { get; set; }

        [DefaultValue(10)]
        int DelayBetweenTaskReadSensorValueLoop { get; set; }

        [DefaultValue(1)]
        int TimeoutReadQueueEvents { get; set; }

        [DefaultValue(false)]
        bool IsAutoHideNotifications { get; set; }

        [DefaultValue(10)]
        int HideNotificationsAfter { get; set; }

        [DefaultValue(true)]
        bool ShowModelColumn { get; set; }

        [DefaultValue(true)]
        bool ShowIpAddressColumn { get; set; }

        [DefaultValue(true)]
        bool ShowMacAddressColumn { get; set; }

        [DefaultValue(true)]
        bool ShowSerialNumberColumn { get; set; }

        [DefaultValue(true)]
        bool ShowLocationColumn { get; set; }

        [DefaultValue(true)]
        bool ShowDescriptionColumn { get; set; }


        [DefaultValue(true)]
        bool ShowTimeColumn { get; set; }

        [DefaultValue(true)]
        bool ShowNameColumn { get; set; }

        [DefaultValue(true)]
        bool ShowEventIpAddressColumn { get; set; }

        [DefaultValue(true)]
        bool ShowSensorColumn { get; set; }

        [DefaultValue(true)]
        bool ShowValueColumn { get; set; }

        [DefaultValue(true)]
        bool ShowAgeColumn { get; set; }

        [DefaultValue(true)]
        bool ShowEventDescriptionColumn { get; set; }

        [DefaultValue(true)]
        bool ShowStateColumn { get; set; }

        [DefaultValue(true)]
        bool ShowEventLocationColumn { get; set; }

        [DefaultValue(true)]
        bool ShowDeviceDescriptionColumn { get; set; }

        [DefaultValue(true)]
        bool ShowConfirmColumn { get; set; }
    }
}
