using System;

namespace TFortisDeviceManager.Models
{
    public interface ISettingsState
    {
        event EventHandler<SettingsStateEventArgs> SettingsChanged;
    }

    public class SettingsStateEventArgs : EventArgs
    {
        public bool NeedRestartApp { get; }

        public SettingsStateEventArgs(bool needRestartApp)
        {
            NeedRestartApp = needRestartApp;
        }
    }
}
