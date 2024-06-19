using System.ComponentModel;

namespace TFortisDeviceManager.Models;

public interface IApplicationSettings : INotifyPropertyChanged
{
    [DefaultValue(6123)]
    public int Port { get; }

    [DefaultValue((byte)225)]
    public byte SearchResponseCode { get; }

    [DefaultValue((byte)224)]
    public byte SearchRequestCode { get; }

    [DefaultValue((byte)227)]
    public byte SettingsResponseCode { get; }

    [DefaultValue((byte)226)]
    public byte SettingsRequestCode { get; }

    [DefaultValue(444)]
    public int MessageSize { get; }

    [DefaultValue(15)]
    public int SettingsConfirmationTimeout { get; }
}