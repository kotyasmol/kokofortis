using GMap.NET.WindowsPresentation;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Converters
{
    class MarkerToMarkerViewConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ObservableCollection<GMapMarker> MarkerViewSource = new(); 
            ObservableCollection<MarkerModel> markerCollection = (ObservableCollection<MarkerModel>)value;

            foreach (var marker in markerCollection)
            {
                if (marker.IsAvilable)
                {
                    BitmapImage bitmapImage = new();

                    bitmapImage.BeginInit();
                    var uri = new Uri(@"\TFortisDeviceManager;component\Views\Images\psw_enabled.ico", UriKind.Relative);
                    bitmapImage.UriSource = uri;

                    bitmapImage.EndInit();

                    System.Windows.Controls.Image image = new()
                    {
                        Source = bitmapImage,
                        Width = 50,
                        Height = 50,
                        ToolTip = $"{marker.Ip}\n{Properties.Resources.Online}"
                    };

                    GMapMarker markerView = new(new GMap.NET.PointLatLng(marker.Lat, marker.Lng))
                    {
                        Shape = image
                    };

                    MarkerViewSource.Add(markerView);
                }
                else
                {
                    BitmapImage bitmapImage = new();

                    bitmapImage.BeginInit();
                    var uri = new Uri(@"\TFortisDeviceManager;component\Views\Images\psw_disabled.ico", UriKind.Relative);
                    bitmapImage.UriSource = uri;

                    bitmapImage.EndInit();

                    System.Windows.Controls.Image image = new()
                    {
                        Source = bitmapImage,
                        Width = 50,
                        Height = 50,
                        ToolTip = $"{marker.Ip}\n{Properties.Resources.Offline}"
                    };

                    GMapMarker markerView = new(new GMap.NET.PointLatLng(marker.Lat, marker.Lng))
                    {
                        Shape = image
                    };

                    MarkerViewSource.Add(markerView);
                }
            }
            return MarkerViewSource;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}