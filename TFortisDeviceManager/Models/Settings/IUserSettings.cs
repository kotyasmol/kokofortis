using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public interface IUserSettings : INotifyPropertyChanged
    {
        ISearchSettings SearchSettings { get; }
        IMonitoringSettings MonitoringSettings { get; }
        IMapSettings MapSettings { get; }
        IAlertSettings AlertSettings { get; }
        ICommonSettings CommonSettings { get; }
        IGMapSettings GMapSettings { get; }

    }
}
