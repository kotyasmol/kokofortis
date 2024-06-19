using System.Windows;
using System.ComponentModel;
using System;
using System.Windows.Controls;
using System.Windows.Forms;
using DocumentFormat.OpenXml.Drawing.Charts;
using Serilog;

namespace TFortisDeviceManager.Views
{

    public partial class RootView : HandyControl.Controls.Window
    {
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
           
                Hide();

            base.OnClosing(e);
        }

        void NotifyIcon_Click(object sender, EventArgs e)
        {
            Show();
            if (WindowState == WindowState.Minimized)
            {
                WindowState = WindowState.Normal;
            }

            Log.Information("Приложение развернуто из трея");

            Activate();
            Topmost = true;  
            Topmost = false; 
            Focus();         

        }

        public RootView()
        {
            InitializeComponent();        
        }

        private void MenuItemExpand_Click(object sender, RoutedEventArgs e)
        {
            Show();
            if (WindowState == WindowState.Minimized)
                WindowState = WindowState.Normal;


            Activate();
            Topmost = true;
            Topmost = false;
            Focus();
        }

        private void MenuItemClose_Click(object sender, RoutedEventArgs e)
        {
            Log.Information("Приложение закрыто из трея");

            System.Windows.Application.Current.Shutdown();
        }
    }
}
