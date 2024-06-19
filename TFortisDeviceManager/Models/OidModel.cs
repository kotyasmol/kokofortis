using System.ComponentModel;

namespace TFortisDeviceManager.Models
{
    public class OidModel : INotifyPropertyChanged
    {
        private bool _enable = false;
        private bool _sendEmail = false;

        public int Key { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Timeout { get; set; }
        public bool Invertible { get; set; }
        public bool Invert { get; set; }

        public bool Enable
        {
            get { return _enable; }
            set
            {
                if (value != _enable)
                {
                    _enable = value;
                    NotifyPropertyChanged("Enable");
                }
            }
        }
        public bool SendEmail
        {
            get { return _sendEmail; }
            set
            {
                if (value != _sendEmail)
                {
                    _sendEmail = value;
                    NotifyPropertyChanged("SendEmail");
                }
            }
        }
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
