using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public class MarkerModel : INotifyPropertyChanged
    {
        public string DeviceName { get; set; }

        private string _ip = "";
        public string Ip
        {
            get { return _ip; }
            set
            {
                if (value != _ip)
                {
                    _ip = value;
                    NotifyPropertyChanged(nameof(Ip));
                }
            }
        }

        public bool IsAvilable { get; set; }
        public string X { get; set; } = "";
        public string Description { get; set; } = "";
        public string Y { get; set; } = "";
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Tag { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }       
    }
}
