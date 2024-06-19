using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TFortisDeviceManager.Models
{
    public interface IMapSettings : INotifyPropertyChanged
    {
        [DefaultValue("")]
        string BGImage { get; set; }

        [DefaultValue(1.75)]
        double Scale { get; set; }
    }
}
