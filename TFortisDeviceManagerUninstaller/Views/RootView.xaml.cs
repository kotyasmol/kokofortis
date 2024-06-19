using System.IO;
using System.Threading;
using System;
using TFortisDeviceManagerUninstaller.ViewModels;
using Microsoft.Win32;
using Microsoft.VisualBasic.ApplicationServices;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Forms;

namespace TFortisDeviceManagerUninstaller.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class RootView : HandyControl.Controls.GlowWindow
    {
        public RootView()
        {
            InitializeComponent();
            
            Closed += OnApplicationExit;
        }

        private void OnApplicationExit(object sender, EventArgs e)
        {          
            if (RootViewModel.IsUninstallFinished)
            {
                ProcessStartInfo startInfo = new()
                {
                    CreateNoWindow = false,
                    FileName = "cmd.exe",
                    Arguments = string.Format("/C rd /s /q \"{0}\"", Application.StartupPath),
                    Verb = "runas"
                };

                Process.Start(startInfo);
            }
        }
    }
}
