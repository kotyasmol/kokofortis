using System;
using System.IO;
using System.Text;
using System.Windows;
using HandyControl.Tools;
using Microsoft.Win32;
using NodeNetwork;
using Serilog;

namespace TFortisDeviceManager
{
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            NNViewRegistrar.RegisterSplat();

            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File("Logs//log.txt",// File save path
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] {Message:lj}{NewLine}{Exception}",// Output date format
            rollingInterval: RollingInterval.Day,// Log Save on day
            rollOnFileSizeLimit: true,          // Limit the maximum length of a single file
            encoding: Encoding.UTF8,            // File character encoding     
            retainedFileCountLimit: 31,         // Maximum Save File     
            fileSizeLimitBytes: 1024 * 1024)      // Maximum single file length
          .CreateLogger();

            Log.Information("Приложение запущено");

            if (!ApplicationHelper.IsSingleInstance("TFortisDeviceManager"))
            {
                MessageBox.Show(TFortisDeviceManager.Properties.Resources.AlreadyRunningError,
                    "TFortisDeviceManager",
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error, 
                    MessageBoxResult.OK,
                    MessageBoxOptions.ServiceNotification);

                Current.Shutdown();
            }
            else
                base.OnStartup(e);
        }

    }
}
