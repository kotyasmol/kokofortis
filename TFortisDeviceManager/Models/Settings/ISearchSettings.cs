using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public interface ISearchSettings : INotifyPropertyChanged
    {
        [DefaultValue(10)]
        int SearchTimeout { get; set; }

    }
}
