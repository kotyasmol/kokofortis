using System;
using System.ComponentModel;
using System.Windows;

namespace TFortisDeviceManager.Models
{
    public class HolderOnMapModel : INotifyPropertyChanged, IEquatable<HolderOnMapModel>
    {
        public double Height { get; } = 6;
        public double Width { get; } = 6;

        private double x;
        public double X
        {
            get
            {
                return x;
            }
            set
            {
                if (x == value)
                {
                    return;
                }

                PointInLine = new Point(value + Math.Round(Width / 2), PointInLine.Y);

                x = value;

                OnPropertyChanged("X");
            }
        }


        private double y;
        public double Y
        {
            get
            {
                return y;
            }
            set
            {
                if (y == value)
                {
                    return;
                }

                ////var deltaY = value - y;

                PointInLine = new Point(PointInLine.X, value + Math.Round(Height / 2));
                //Console.WriteLine(PointInLine.Y);

                y = value;

                OnPropertyChanged("Y");
            }
        }

        private Point pointInLine;
        public Point PointInLine
        {
            get
            {
                return pointInLine;
            }
            set
            {
                if (pointInLine == value)
                {
                    return;
                }


                if (Connection != null)
                {
                    int i = 0;

                    foreach (var point in Connection.Points)
                    {
                        //Console.WriteLine(point);
                        //Console.WriteLine(pointInLine);
                        //Console.WriteLine();
                        if (point == pointInLine)
                        {
                            //Console.WriteLine("!!!");
                            break;
                        }
                        i++;
                    }

                    if (i > 0 && i < Connection.Points.Count)
                    {
                        Connection.Points[i] = value;
                    }

                    Connection.SayPointsWasUpdated();
                }

                pointInLine = value;
                OnPropertyChanged("PointInLine");
            }
        }

        private ConnectionOnMapModel _сonnection;
        public ConnectionOnMapModel Connection
        {
            get
            {
                return _сonnection;
            }
            set
            {
                if (_сonnection == value)
                {
                    return;
                }

                _сonnection = value;
                OnPropertyChanged("Connection");
            }
        }

        public HolderOnMapModel()
        {
        }

        public bool Equals(HolderOnMapModel? other)
        {
            if (other == null)
                return false;

            return (X == other.X) && (Y == other.Y);
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

        public override bool Equals(object? obj)
        {
            return Equals(obj as HolderOnMapModel);
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
