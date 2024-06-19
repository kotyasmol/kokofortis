using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace TFortisDeviceManager.Models
{
    public class ConnectionOnMapModel : INotifyPropertyChanged
    {
        public ObservableCollection<Point> Points { get; } = new();

        public ObservableCollection<HolderOnMapModel> Holders { get; } = new();

        public PortOnMapModel OriginPort { get; set; }
        public PortOnMapModel DestinationPort { get; set; }

        public void DelConnectionFromPorts()
        {
            DestinationPort.Line = null;
            OriginPort.Line = null;
        }

        public int NumberOfSegments { get; } = 3;


        private Point originPoint;
        public Point OriginPoint
        {
            get
            {
                return originPoint;
            }
            set
            {
                if (originPoint == value)
                {
                    return;
                }

                originPoint = value;
                ChangeEndsConnectionPoint();

                OnPropertyChanged("OriginPoint");
            }
        }


        private Point destinationPoint;
        public Point DestinationPoint
        {
            get
            {
                return destinationPoint;
            }
            set
            {
                if (destinationPoint == value)
                {
                    return;
                }

                destinationPoint = value;
                ChangeEndsConnectionPoint();
                OnPropertyChanged("DestinationPoint");
            }
        }


        private void ChangeEndsConnectionPoint()
        {
            if (Points.Any())
            {
                Points[^1] = new Point(DestinationPoint.X, DestinationPoint.Y);
                Points[0] = new Point(OriginPoint.X, OriginPoint.Y);
                OnPropertyChanged("Points");
            }
        }

        public void SayPointsWasUpdated()
        {
            OnPropertyChanged("Points");
        }



        private void SetPoints()
        {
            var deltaX = (DestinationPoint.X - OriginPoint.X) / NumberOfSegments;
            var deltaY = (DestinationPoint.Y - OriginPoint.Y) / NumberOfSegments;

            double startX = OriginPoint.X;
            double startY = OriginPoint.Y;

            for (int i = 0; i <= NumberOfSegments; i++)
            {
                Points.Add(new Point(Math.Round(startX + deltaX * i), Math.Round(startY + deltaY * i)));
            }

        }
        public ConnectionOnMapModel(PortOnMapModel originPort, Point originPoint, PortOnMapModel destinationPort, Point destinationPoint)
        {
            OriginPort = originPort;
            OriginPoint = originPoint;

            DestinationPort = destinationPort;
            DestinationPoint = destinationPoint;
            SetPoints();
        }

        public ConnectionOnMapModel()
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
}
