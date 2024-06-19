using Config.Net;
using FluentValidation;
using HandyControl.Themes;
using Microsoft.Extensions.Logging;
using Serilog;
using Stylet;
using StyletIoC;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using TFortisDeviceManager.Telegram;
using TFortisDeviceManager.Validators;
using TFortisDeviceManager.ViewModels;

namespace TFortisDeviceManager
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        private readonly string _programName = "TFortis Device Manager";
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            builder.Bind(typeof(IModelValidator<>)).To(typeof(FluentValidationAdapter<>));
            builder.Bind(typeof(IValidator<>)).ToAllImplementations();
            builder.Bind<IDeviceSearcher>().To<DeviceSearcher>();
            builder.Bind<IMonitoringEventService>().To<MonitoringEventService>();
                 
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDir = Path.Combine(appDataPath, _programName);
            Directory.CreateDirectory(appDir);

            var userConfigFileName = Path.Combine(appDir, "UserConfig.json");

            IUserSettings userSettings = new ConfigurationBuilder<IUserSettings>()
                .UseJsonFile(userConfigFileName)
                .Build();
            builder.Bind<IUserSettings>().ToInstance(userSettings);

            var appConfigFileName = Path.Combine(appDir, "AppConfig.json");

            IApplicationSettings applicationSettings = new ConfigurationBuilder<IApplicationSettings>()
                .UseJsonFile(appConfigFileName)
                .Build();

            builder.Bind<IApplicationSettings>().ToInstance(applicationSettings);

            builder.Bind<ISettingsProvider>().To<SettingsProvider>();
            builder.Bind<INotificationBot>().To<NotificationBotService>();

            builder.Bind<INetworkDeviceManager>().To<NetworkDeviceManager>();
            builder.Bind<IMailSender>().To<MailSender>();

            builder.Bind<IMessageBoxService>().To<MessageBoxService>();
            builder.Bind<INotificationService>().To<NotificationService>();
        }

        protected override void Configure()
        {            
            var userSettings = Container.Get<IUserSettings>();
            if (userSettings != null)
            {
                var culture = userSettings.CommonSettings.AppLanguage == "en"
                    ? CultureInfo.CreateSpecificCulture("en")
                    : CultureInfo.CreateSpecificCulture("ru-RU");
                _ = userSettings.CommonSettings.AppTheme == ApplicationTheme.Dark
                    ? ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark
                    : ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;

                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            base.Configure();
        }
    }
}
