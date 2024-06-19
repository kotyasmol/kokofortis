using DynamicData;
using GMap.NET.WindowsPresentation;
using HandyControl.Tools.Extension;
using Microsoft.IdentityModel.Tokens;
using Splat;
using Stylet;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using TFortisDeviceManager.Database;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.Services;
using static System.Windows.Forms.AxHost;

namespace TFortisDeviceManager.ViewModels
{
    public class GMapViewModel : Screen, IDisposable
    {
        public static ObservableCollection<MonitoringDevice> MonitoringDevices { get; set; } = new();

        public static ObservableCollection<MarkerModel> Markers { get; set; } = new();

        public static ObservableCollection<GMapMarker> MarkersViewSource { get; set; } = new();

        private static readonly BitmapImage disabledBitmap = new();
        private static readonly BitmapImage enabledBitmap = new();

        private object _selectedViewModel;
        public object SelectedViewModel
        {
            get { return _selectedViewModel; }
            set { _selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
        }

        private static MonitoringDevice? _selectedDevice;
        public static MonitoringDevice? SelectedDevice
        {
            get => _selectedDevice;
            set => _selectedDevice = value;
        }

        public GMapViewModel()
        {
            DisplayName = Properties.Resources.MainMenuGMap;

            MonitoringEventService.ChangingDeviceState += ChangeDeviceState;
            MonitoringViewModel.DevicesForMonitoring.CollectionChanged += DeviceListChanged;

            MonitoringDevices.AddRange(PGDataAccess.GetDevicesForMonitoring());

            enabledBitmap.BeginInit();
            enabledBitmap.UriSource = new Uri(@"\TFortisDeviceManager;component\Views\Images\psw_enabled.ico", UriKind.Relative);
            enabledBitmap.EndInit();

            disabledBitmap.BeginInit();
            disabledBitmap.UriSource = new Uri(@"\TFortisDeviceManager;component\Views\Images\psw_disabled.ico", UriKind.Relative);
            disabledBitmap.EndInit();
        }

        void DeviceListChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems?[0] is MonitoringDevice newDevice)
                    {
                        var device = MonitoringDevices.FirstOrDefault(x => x.IpAddress == newDevice.IpAddress);

                        if(device == null)
                        {
                            MonitoringDevices.Add(newDevice);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems?[0] is MonitoringDevice oldDevice)
                    {
                        var device = MonitoringDevices.FirstOrDefault(x => x.IpAddress == oldDevice.IpAddress);

                        if(device != null)
                        MonitoringDevices.Remove(device);

                        var marker = Markers.FirstOrDefault(x => x.Ip == oldDevice.IpAddress);

                        if(marker != null)
                        Markers.Remove(marker);
                    }
                    break;
            }
        }

        private void ChangeDeviceState(object? sender, ChangingDeviceStateEventArgs? e)
        {
            try
            {
                string state = e.State;

                if (state == HostStates.Enabled)
                {
                    MonitoringDevice device = MonitoringDevices.FirstOrDefault(x => x.IpAddress == e.Ip);
                    MonitoringDevices.FirstOrDefault(x => x.IpAddress == e.Ip).Available = true;

                    if (device != null)
                        SetMarkerEnabled(device);

                    return;
                }

                if (state == HostStates.Disabled)
                {
                    MonitoringDevice device = MonitoringDevices.FirstOrDefault(x => x.IpAddress == e.Ip);
                    MonitoringDevices.FirstOrDefault(x => x.IpAddress == e.Ip).Available = false;

                    if (device != null)
                        SetMarkerDisabled(device);

                    return;
                }
            }
            catch
            {
                //
            }
        }

        private static void SetMarkerEnabled(MonitoringDevice device)
        {
            var oldMarker = Markers.FirstOrDefault(x => x.Ip == device.IpAddress);

            if (oldMarker == null || oldMarker.IsAvilable == true) return;

            var newMarker = oldMarker;

            newMarker.IsAvilable = true;

            Markers.Replace(oldMarker, newMarker);
        }

        private static void SetMarkerDisabled(MonitoringDevice device)
        {
            var oldMarker = Markers.FirstOrDefault(x => x.Ip == device.IpAddress);

            if (oldMarker == null || oldMarker.IsAvilable == false) return;

            var newMarker = oldMarker;

            newMarker.IsAvilable = false;

            Markers.Replace(oldMarker, newMarker);
        }

        public static void AddMarker(double lat, double lng)
        {
            if(SelectedDevice == null) return;

            if (Markers.Where(x => x.Ip == SelectedDevice.IpAddress).FirstOrDefault() != null)
            {
                string message = Properties.Resources.DeviceIsAlreadyExists;
                HandyControl.Controls.MessageBox.Show(message, $"Ip: {SelectedDevice.IpAddress}", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }     

             MarkerModel markerModel = new()
             {
                 DeviceName = SelectedDevice.Name,
                 Ip = SelectedDevice.IpAddress,
                 IsAvilable = SelectedDevice.Available,
                 Description = SelectedDevice.Description ?? "",
                 Lat = lat,
                 Lng = lng,
                 X = lat.ToString("0.00000", CultureInfo.InvariantCulture),
                 Y = lng.ToString("0.00000", CultureInfo.InvariantCulture),
             };

            MonitoringDevices.FirstOrDefault(x => x.IpAddress == SelectedDevice.IpAddress).OnMap = true;

            Markers.Add(markerModel);
        }

        public void DeleteMarkerCommand(string ip)
        {
            var marker = Markers.FirstOrDefault(x => x.Ip == ip);

            if (marker == null) return;

            Markers.DeleteIfExists(marker);

            MonitoringDevices.FirstOrDefault(x => x.IpAddress == ip).OnMap = false;
        }

        private static void SaveMarkersForMapToFile()
        {
            var dcss = new DataContractSerializerSettings { PreserveObjectReferences = true };
            var dcs = new DataContractSerializer(typeof(ObservableCollection<MarkerModel>), dcss);

            using Stream fStreamMarkerModel = new FileStream("markers.xml", FileMode.Create, FileAccess.Write, FileShare.None);
            dcs.WriteObject(fStreamMarkerModel, Markers);

            fStreamMarkerModel.Close();
        }

        public static void LoadMarkersForMapFromFile()
        {
            ObservableCollection<MarkerModel> data = GetMarkersFromFile();

            Markers.Clear();

            foreach (var marker in data)
            {
                Markers.Add(marker);
            }
        }

        private static ObservableCollection<MarkerModel> GetMarkersFromFile()
        {
            ObservableCollection<MarkerModel> data = new();

            var settingsSerializer = new DataContractSerializerSettings { PreserveObjectReferences = true };
            var serializer = new DataContractSerializer(typeof(ObservableCollection<MarkerModel>), settingsSerializer);

            using (Stream fStream = new FileStream("markers.xml", FileMode.OpenOrCreate))
            {
                try
                {
                    data = (ObservableCollection<MarkerModel>)serializer.ReadObject(fStream);

                    fStream.Close();
                }
                catch (System.Xml.XmlException)
                {
                    //
                }
            }

            return data;
        }

        public void TableSortCommand(object sender, DataGridSortingEventArgs e)
        {
            if (sender is not DataGrid dataGrid) return;

            string sortPropertyName = e.Column.SortMemberPath;

            if (!string.IsNullOrEmpty(sortPropertyName))
            {
                if (sortPropertyName == "IpAddress")
                {
                    e.Handled = true;
                    ListSortDirection direction = (e.Column.SortDirection != ListSortDirection.Ascending) ? ListSortDirection.Ascending : ListSortDirection.Descending;
                    e.Column.SortDirection = direction;
                    ListCollectionView lcv = (ListCollectionView)CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
                    IComparer comparer = new SortIPAddress(direction);
                    lcv.CustomSort = comparer;
                }
            }
        }

        public void Dispose()
        {
            SaveMarkersForMapToFile();
        }
    }
}
