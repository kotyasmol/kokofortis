using Serilog;
using Stylet;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using TFortisDeviceManager.Telegram;

namespace TFortisDeviceManager.ViewModels
{
    public class RootViewModel : Conductor<IScreen>.Collection.OneActive, IDisposable
    {
        private readonly SearchViewModel _searchScreen;
        private readonly MonitoringViewModel _monitoringScreen;
        private readonly MapViewModel _mapScreen;
        private readonly GMapViewModel _gmapScreen;
        private readonly CommonSettingsViewModel _commonSettingsScreen;
        private readonly AlertSettingsViewModel _alertSettingsScreen;
        private readonly SearchSettingsViewModel _searchSettingsScreen;
        private readonly MonitoringSettingsViewModel _monitoringSettingsScreen;
        private readonly MapSettingsViewModel _mapSettingsScreen;
        private readonly AboutViewModel _aboutScreen;
        private readonly NotificationBotService _notificationBotService;
        private readonly GraphicsViewModel _graphicsScreen;         // и вот тут добавить вьюшку (+)

        public string Title
        {         
            get => string.Format(CultureInfo.InvariantCulture, Properties.Resources.WindowTitle, ActiveItem?.DisplayName ?? string.Empty);
        }

        private string? _info;
        public string? Info
        {
            get => _info;
            set => SetAndNotify(ref _info, value);
        }

        private bool? _isFullScreen = false;
        public bool? IsFullScreen
        {
            get => _isFullScreen;
            set => SetAndNotify(ref _isFullScreen, value);
        }

        private readonly CancellationTokenSource cancellationTokenSource = new();

        public RootViewModel(SearchViewModel searchScreen, 
            MonitoringViewModel monitoringScreen, 
            MapViewModel mapScreen, 
            GMapViewModel gmapScreen,
            CommonSettingsViewModel commonSettingsScreen, 
            AlertSettingsViewModel alertSettingsScreen, 
            SearchSettingsViewModel searchSettingsScreen, 
            MonitoringSettingsViewModel monitoringSettingsScreen, 
            MapSettingsViewModel mapSettingsScreen,
            AboutViewModel aboutScreen,
            NotificationBotService notificationBotService, GraphicsViewModel graphicsScreen)
    
            
        {            
            _searchScreen = searchScreen ?? throw new System.ArgumentNullException(nameof(searchScreen));
            _monitoringScreen = monitoringScreen ?? throw new System.ArgumentNullException(nameof(monitoringScreen));
            _mapScreen = mapScreen ?? throw new System.ArgumentNullException(nameof(mapScreen));
            _gmapScreen = gmapScreen ?? throw new System.ArgumentNullException(nameof(gmapScreen));
            _commonSettingsScreen = commonSettingsScreen ?? throw new System.ArgumentNullException(nameof(commonSettingsScreen));
            _alertSettingsScreen = alertSettingsScreen ?? throw new System.ArgumentNullException(nameof(alertSettingsScreen));
            _searchSettingsScreen = searchSettingsScreen ?? throw new System.ArgumentNullException(nameof(searchSettingsScreen));
            _monitoringSettingsScreen = monitoringSettingsScreen ?? throw new System.ArgumentNullException(nameof(monitoringSettingsScreen));
            _mapSettingsScreen = mapSettingsScreen ?? throw new System.ArgumentNullException(nameof(mapSettingsScreen));
            _aboutScreen = aboutScreen ?? throw new System.ArgumentNullException(nameof(aboutScreen));
            _notificationBotService = notificationBotService ?? throw new System.ArgumentNullException(nameof(notificationBotService));
            _graphicsScreen = graphicsScreen ?? throw new System.ArgumentNullException(nameof(graphicsScreen));
            // и сюдf
            ActiveItem = searchScreen;

            _commonSettingsScreen.SettingsChanged += OnSettingsChanged;
            _alertSettingsScreen.SettingsChanged += OnSettingsChanged;

            Task.Run(() => _notificationBotService.StartAsync(cancellationTokenSource.Token));
        }

        private void OnSettingsChanged(object? sender, SettingsStateEventArgs e)
        {
            if (e.NeedRestartApp)
                Info = Properties.Resources.ShouldRestartAppMessage;
            else
                Info = string.Empty;
        }

        public void SwitchScreenCommand(string tag)
        {
            ActiveItem = TagToScreen(tag);
        }

        private IScreen TagToScreen(string tag) =>
            tag switch // сюда хасунуть
            {
                "MainMenuSearchTag" => _searchScreen,
                "MainMenuMonitoringTag" => _monitoringScreen,
                "MainMenuMapTag" => _mapScreen,
                "GMapTag" => _gmapScreen,
                "SettingsMenuCommonTag" => _commonSettingsScreen,
                "SettingsMenuAlertTag" => _alertSettingsScreen,
                "SettingsMenuSearchTag" => _searchSettingsScreen,
                "SettingsMenuMonitoringTag" => _monitoringSettingsScreen,
                "SettingsMenuMapTag" => _mapSettingsScreen,
                "SettingsMenuAboutTag" => _aboutScreen,
                "Graphics" => _graphicsScreen,
                _ => throw new ArgumentOutOfRangeException(nameof(tag), $"Not expected tag value: {tag}"),
            };        
        

        private IScreen? _lastMainScreen;
        private IScreen? _lastSettingsScreen;
        private bool disposedValue;

        public void OpenSettingsScreenCommand()
        {
            _lastMainScreen = ActiveItem;

            ActiveItem = _lastSettingsScreen ?? _commonSettingsScreen;                     
        }

        public void CloseSettingsScreenCommand()
        {
            _lastSettingsScreen = ActiveItem;

            ActiveItem = _lastMainScreen ?? _searchScreen;
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(ActiveItem))
            {
                NotifyOfPropertyChange(nameof(Title));
            }

            base.OnPropertyChanged(propertyName);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (_commonSettingsScreen != null)
                        _commonSettingsScreen.SettingsChanged -= OnSettingsChanged;

                    if (_alertSettingsScreen != null)
                        _alertSettingsScreen.SettingsChanged -= OnSettingsChanged;
                }

                cancellationTokenSource.Cancel();

                Log.Information("Приложение закрыто");

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
