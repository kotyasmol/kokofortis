using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFortisDeviceManager.Models
{
    public interface IGMapSettings : INotifyPropertyChanged
    {
        [DefaultValue(66.4169575018027)]
        double CenterX { get; set; }

        [DefaultValue(94.25025752215694)]
        double CenterY { get; set; }

        [DefaultValue(5.0)]
        double Zoom { get; set; }
    }
}
