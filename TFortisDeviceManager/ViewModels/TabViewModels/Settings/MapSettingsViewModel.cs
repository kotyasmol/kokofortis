using System;
using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels;

public sealed class MapSettingsViewModel : Screen, IDisposable
{

    public MapSettingsViewModel()
    {
        DisplayName = Properties.Resources.MapSettings;
    }

    public void Dispose()
    {
        //
    }
}