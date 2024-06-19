using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.ViewModels
{
    public sealed class AboutViewModel : Screen, IDisposable
    {
        public string Version { get; set; } = $"{Properties.Resources.Version} 3.0.1";

        public AboutViewModel()
        {
            DisplayName = Properties.Resources.About;
        }
        public void Dispose()
        {
            //
        }
    }
}
