namespace TFortisDeviceManager.Models
{
    public interface ISettingsProvider
    {
        IApplicationSettings ApplicationSettings { get; }
        IUserSettings UserSettings { get; }
    }
    public class SettingsProvider : ISettingsProvider
    {
        public IApplicationSettings ApplicationSettings { get; }
        public IUserSettings UserSettings { get; }

        public SettingsProvider(IApplicationSettings applicationSettings, IUserSettings userSettings)
        {
            ApplicationSettings = applicationSettings;
            UserSettings = userSettings;
        }        
    }
}
