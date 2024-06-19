using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace TFortisDeviceManager.Models
{
    public class PortOnMapModel : INotifyPropertyChanged
    {
        public string? Id { get; set; }

        private LinkState link = LinkState.NotUse;
        public LinkState Link
        {
            get
            {
                return link;
            }
            set
            {
                if (link == value)
                {
                    return;
                }

                link = value;

                OnPropertyChanged("Link");
            }
        }

        private bool isAvailable;
        public bool IsAvailable
        {
            get
            {
                return isAvailable;
            }
            set
            {
                if (isAvailable == value)
                {
                    return;
                }

                isAvailable = value;

                OnPropertyChanged("IsAvailable");
            }
        }

        private PoeState poe = PoeState.NotUse;
        public PoeState Poe
        {
            get
            {
                return poe;
            }
            set
            {
                if (poe == value)
                {
                    return;
                }

                poe = value;

                OnPropertyChanged("Poe");
            }
        }

        public DeviceOnMapModel? ParentDevice { get; set; }

        public ConnectionOnMapModel? Line { get; set; }

        public PortOnMapModel(string id, DeviceOnMapModel parentDevice)
        {
            Id = id;
            ParentDevice = parentDevice;
        }

        public PortOnMapModel(DeviceOnMapModel parentDevice)
        {
            ParentDevice = parentDevice;
        }

        public PortOnMapModel()
        {
        }

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Raises the 'PropertyChanged' event when the value of a property of the view model has changed.
        /// </summary>
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// 'PropertyChanged' event that is raised when the value of a property of the view model has changed.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        #endregion
    }

    public enum PoeState
    {
        Down,
        Up,
        NotUse
    }
    public enum LinkState
    {
        Down,
        Up,
        NotUse
    }
}

