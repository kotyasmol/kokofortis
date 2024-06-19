using HandyControl.Themes;
using Stylet;
using StyletIoC;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TFortisDeviceManagerUninstaller.ViewModels;

namespace TFortisDeviceManagerUninstaller
{
    public class Bootstrapper : Bootstrapper<RootViewModel>
    {
        private readonly string _programName = "TFortisDeviceManagerUninstaller";
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appDir = Path.Combine(appDataPath, _programName);
            Directory.CreateDirectory(appDir);       
        }

        protected override void Configure()
        {            
            base.Configure();
        }
    }
}
