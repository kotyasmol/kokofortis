using HandyControl.Controls;
using HandyControl.Themes;
using Stylet;
using StyletIoC;
using System;
using System.Globalization;
using System.IO;
using System.Threading;
using TFortisDeviceManagerSetup.ViewModels;

namespace TFortisDeviceManagerSetup
{
    public class Bootstrapper : Bootstrapper<LanguageSelectorViewModel>
    {
        private readonly string _programName = "TFortisDeviceManagerSetup";
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
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
