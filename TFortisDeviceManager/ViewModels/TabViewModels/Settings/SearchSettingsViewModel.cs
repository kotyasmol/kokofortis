using System;
using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels;

public sealed class SearchSettingsViewModel : Screen, IDisposable
{
    private readonly IUserSettings _userSettings;

    public int SearchTimeout
    {
        get => _userSettings.SearchSettings.SearchTimeout;
        set
        {
            _userSettings.SearchSettings.SearchTimeout = value;
            NotifyOfPropertyChange(nameof(SearchTimeout));
        }
    }

    public SearchSettingsViewModel(IUserSettings userSettings)
    {
        this.DisplayName = Properties.Resources.SearchSettings;

        _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));
    }
    public void Dispose()
    {
        //
    }
}