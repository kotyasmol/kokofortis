using HandyControl.Themes;
using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public interface ICommonSettings : INotifyPropertyChanged
    {
        [DefaultValue(90)]
        int KeepEventInDatabase { get; set; }

        [DefaultValue("Google")]
        string SelectedProvider { get; set; }

        [DefaultValue("en")]
        string AppLanguage { get; set; }

        [DefaultValue(ApplicationTheme.Dark)]
        ApplicationTheme AppTheme { get; set; }

    }
}