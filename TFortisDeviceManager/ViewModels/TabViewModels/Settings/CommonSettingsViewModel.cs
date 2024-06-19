using System;
using System.Collections.Generic;
using HandyControl.Themes;
using Stylet;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class CommonSettingsViewModel : Screen, ISettingsState, IDisposable
    {                     
        private readonly IUserSettings _userSettings;

        private string? _appLanguageInitialValue;
        private string? _selectedProviderInitialValue;


        public event EventHandler<SettingsStateEventArgs>? SettingsChanged;

        public Dictionary<string, string> LanguageDict { get; } = new Dictionary<string, string>
        {
            {"ru-RU", "Русский" },
            {"en", "English" }
        };

        public List<string> ProvidersDict { get; } = new List<string>
        {
            "Google",
            "OSM",
            "Yandex",
            "Bing"
        };

        public Dictionary<ApplicationTheme, string> ThemeDict { get; } = new Dictionary<ApplicationTheme, string>
        {
            {ApplicationTheme.Light, Properties.Resources.Light},
            {ApplicationTheme.Dark, Properties.Resources.Dark}
        };

        public List<string> ThemeList { get; } = new()
        {
            Properties.Resources.SystemTheme,
            Properties.Resources.Light,
            Properties.Resources.Dark
        };
        
        public int KeepEventInDatabase
        {
            get => _userSettings.CommonSettings.KeepEventInDatabase;
            set
            {
                _userSettings.CommonSettings.KeepEventInDatabase = value;
                NotifyOfPropertyChange(nameof(KeepEventInDatabase));
            }
        }

        public string SelectedProvider
        {
            get => _userSettings.CommonSettings.SelectedProvider;
            set
            {
                _userSettings.CommonSettings.SelectedProvider = value;
                NotifyOfPropertyChange(nameof(SelectedProvider));
            }
        }

        public ApplicationTheme AppTheme
        {
            get => _userSettings.CommonSettings.AppTheme;
            set
            {
                _userSettings.CommonSettings.AppTheme = value;
                NotifyOfPropertyChange(nameof(AppTheme));
            }
        }

        public string AppLanguage
        {
            get => _userSettings.CommonSettings.AppLanguage;
            set
            {
                _userSettings.CommonSettings.AppLanguage = value;                
                NotifyOfPropertyChange(nameof(AppLanguage));
            }
        }        

        public CommonSettingsViewModel(IUserSettings userSettings)
        {
            this.DisplayName = Properties.Resources.CommonSettings;

            _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings)); 
        }      
        
        public void ChangeThemeCommand()
        {
            ThemeManager.Current.ApplicationTheme = AppTheme;
        }

        protected override void OnInitialActivate()
        {
            _appLanguageInitialValue = AppLanguage;
            _selectedProviderInitialValue = SelectedProvider;

            base.OnInitialActivate();
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(AppLanguage))
            {
                var needRestartApp = AppLanguage != _appLanguageInitialValue;                
                SettingsChanged?.Invoke(this, new SettingsStateEventArgs(needRestartApp));               
            }
            if (propertyName == nameof(SelectedProvider))
            {
                var needRestartApp = SelectedProvider != _selectedProviderInitialValue;
                SettingsChanged?.Invoke(this, new SettingsStateEventArgs(needRestartApp));
            }

            base.OnPropertyChanged(propertyName);
        }

        public void Dispose()
        {
            //
        }
    }
}
