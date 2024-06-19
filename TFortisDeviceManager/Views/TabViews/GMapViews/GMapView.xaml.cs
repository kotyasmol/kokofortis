using DynamicData;
using GMap.NET.WindowsPresentation;
using HandyControl.Tools.Extension;
using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.ViewModels;
using Image = System.Windows.Controls.Image;

namespace TFortisDeviceManager.Views
{
    /// <summary>
    /// Логика взаимодействия для GmapView.xaml
    /// </summary>
    public partial class GMapView : UserControl, IDisposable
    {
        private readonly IUserSettings _userSettings;

        private bool markersLoaded;
        public GMapView(IUserSettings userSettings)
        {
            InitializeComponent();

            _userSettings = userSettings ?? throw new ArgumentNullException(nameof(userSettings));

            GMapViewModel.Markers.CollectionChanged += Marker_CollectionChanged;                        

            markersLoaded = false;
        }

        void Marker_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        if (e.NewItems?[0] is MarkerModel newMarker)
                            AddMarker(newMarker);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        if (e.OldItems?[0] is MarkerModel oldMarker)
                            DeleteMarkerFromControl(oldMarker);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        if ((e.NewItems?[0] is MarkerModel replacingMarker) &&
                            (e.OldItems?[0] is MarkerModel replacedMarker))
                        {
                            DeleteMarkerFromControl(replacedMarker);
                            AddMarker(replacingMarker);
                        }
                        break;
                default:
                    break;
                }
        }

        private void ReplaceMarkers(MarkerModel replacedMarker, MarkerModel replacingMarker)
        {
            // Находим маркер, который нужно заменить
            GMapMarker oldMarker = gmapControl.Markers.FirstOrDefault(marker =>
                marker.Position.Lat == replacedMarker.Lat && marker.Position.Lng == replacedMarker.Lng);

            // Если маркер не найден, выходим из метода
            if (oldMarker == null)
                return;

            // Создаем новый маркер для замены
            GMapMarker newMarker = new GMapMarker(new GMap.NET.PointLatLng(replacingMarker.Lat, replacingMarker.Lng));

            // Настройка изображения для нового маркера в зависимости от состояния
            newMarker.Shape = replacingMarker.IsAvilable ?
                new Image
                {
                    Source = Properties.Resources.psw_enabled.ToImageSource(),
                    Width = 50,
                    Height = 50,
                    ToolTip = $"{replacingMarker.DeviceName}\n{replacingMarker.Ip}\n{replacingMarker.Description}\n{Properties.Resources.Online}"
                } :
                new Image
                {
                    Source = Properties.Resources.psw_disabled.ToImageSource(),
                    Width = 50,
                    Height = 50,
                    ToolTip = $"{replacingMarker.DeviceName}\n{replacingMarker.Ip}\n{replacingMarker.Description}\n{Properties.Resources.Offline}"
                };

            // Заменяем маркер в коллекции
            gmapControl.Markers.Replace(oldMarker, newMarker);
        }

        public static ImageSource ToImageSource(Icon icon)
        {
            ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());

            return imageSource;
        }

        private void AddMarker(MarkerModel markerModel)
        {

            Dispatcher.Invoke(new Action(() =>
            {
                try
                {
                    GMapMarker marker = new(new GMap.NET.PointLatLng(markerModel.Lat, markerModel.Lng));

                    if (markerModel.IsAvilable)
                    {
                        marker.Shape = new Image
                        {
                            Source = Properties.Resources.psw_enabled.ToImageSource(),
                            Width = 50,
                            Height = 50,
                            ToolTip = $"{markerModel.DeviceName}\n{markerModel.Ip}\n{markerModel.Description}\n{Properties.Resources.Online}"
                        };
                    }
                    else
                    {
                        marker.Shape = new Image
                        {
                            Source = Properties.Resources.psw_disabled.ToImageSource(),
                            Width = 50,
                            Height = 50,
                            ToolTip = $"{markerModel.DeviceName}\n{markerModel.Ip}\n{markerModel.Description}\n{Properties.Resources.Offline}"
                        };
                    }

                    gmapControl.Markers.Add(marker);
                }
                catch(Exception ex)
                {
                    //
                }
            }));            
        }

        private void DeleteMarkerFromControl(MarkerModel markerModel)
        {
            Dispatcher.Invoke(new Action(() =>
            {
            try
            {
                var markerToDelete = gmapControl.Markers.FirstOrDefault(x => x.Position.Lat == markerModel.Lat && x.Position.Lng == markerModel.Lng);

                if (markerToDelete != null)
                {
                    gmapControl.Markers.DeleteIfExists(markerToDelete);
                }
                }
                catch (Exception ex)
                {
                    //
                }
            }));
        }

        private void gmapControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;

            switch (_userSettings.CommonSettings.SelectedProvider)
            {
                case "Google":
                    {
                        gmapControl.MapProvider = GMap.NET.MapProviders.GoogleMapProvider.Instance;
                        gmapControl.ReloadMap();

                        break;
                    }
                case "Yandex":
                    {
                        gmapControl.MapProvider = GMap.NET.MapProviders.YandexMapProvider.Instance;
                        gmapControl.ReloadMap();

                        break;
                    }
                case "OSM":
                    {
                        gmapControl.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
                        gmapControl.ReloadMap();

                        break;
                    }
                case "Bing":
                    {
                        gmapControl.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
                        gmapControl.ReloadMap();

                        break;
                    }
            }

            gmapControl.MinZoom = 3;
            gmapControl.MaxZoom = 18;
            gmapControl.Zoom = _userSettings.GMapSettings.Zoom;
            gmapControl.Position = new GMap.NET.PointLatLng(_userSettings.GMapSettings.CenterX, _userSettings.GMapSettings.CenterY);
            gmapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            gmapControl.CanDragMap = true;
            gmapControl.DragButton = System.Windows.Input.MouseButton.Left;
            gmapControl.ShowCenter = false;
            gmapControl.ShowTileGridLines = false;

            if (!markersLoaded)
            {
                GMapViewModel.LoadMarkersForMapFromFile();
                markersLoaded = true;
            }
        }

        private void gmapControl_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            double lat = gmapControl.FromLocalToLatLng((int)e.GetPosition(gmapControl).X, (int)e.GetPosition(gmapControl).Y).Lat;
            double lng = gmapControl.FromLocalToLatLng((int)e.GetPosition(gmapControl).X, (int)e.GetPosition(gmapControl).Y).Lng;

            GMapViewModel.AddMarker(lat, lng);
        }

        private void gmapControl_OnMapZoomChanged()
        {
            _userSettings.GMapSettings.Zoom = gmapControl.Zoom;
        }

        private void gmapControl_OnPositionChanged(GMap.NET.PointLatLng point)
        {
            _userSettings.GMapSettings.CenterX = point.Lat; 
            _userSettings.GMapSettings.CenterY = point.Lng;
        }

        public void Dispose()
        {

        }
    }
}
