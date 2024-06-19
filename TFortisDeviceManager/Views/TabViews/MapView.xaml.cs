using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using TFortisDeviceManager.Models;
using TFortisDeviceManager.ViewModels;
using TFortisDeviceManager.Properties;
using Stylet;
using TFortisDeviceManager.Database;
using System.Globalization;
using TFortisDeviceManager.Services;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Threading;
using TFortisDeviceManager.Telegram;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Tools;
using MahApps.Metro.Controls;
using System.Diagnostics;

namespace TFortisDeviceManager.Views
{
    /// <summary>
    /// Interaction logic for MapView.xaml
    /// </summary>
    public partial class MapView : UserControl, IDisposable
    {
        private readonly List<DeviceOnMapModel>? selectedDevices = new();
        DeviceOnMapModel? leftDevice, rightDevice, topDevice, bottomDevice;
        private readonly List<HolderOnMapModel>? holdersForMove = new();

        public MapView()
        {
            InitializeComponent();
        }

        Point clickPosition;
        private void RootCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickPosition = e.GetPosition(rootCanvas);
            rootCanvas.CaptureMouse();
        }

        private void RootCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && rootCanvas.IsMouseCaptured)
            {
                dragSelectionCanvas.Visibility = Visibility.Visible;
                Point currentPosition = e.GetPosition(rootCanvas);
                UpdateDragSelectionRect(clickPosition, currentPosition);
            }
        }

        Point lastPoint;

        private void Map_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastPoint = Mouse.GetPosition(rootCanvas);
        }

        private void Map_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            MapViewModel.DelLines.Clear();
            DeselectDevices();
        }

        private void RootLayout_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var element = Mouse.DirectlyOver;
                if (element is Polyline line)
                {
                    RemoveConnection(line);
                }
                DrawDelLine();
            }
        }

        public void DrawDelLine()
        {
            var mousePosition = Mouse.GetPosition(rootCanvas);

            if (mousePosition != lastPoint)
            {
                if (MapViewModel.DelLines.Count < 1000)
                {
                    MapViewModel.DelLines.Add(new DotOnMapModel(mousePosition));
                    lastPoint = mousePosition;
                }
                else
                {
                    MapViewModel.DelLines.Clear();
                }
            }
        }

        private static void RemoveConnection(Polyline line)
        {
            if (line.DataContext is ConnectionOnMapModel connection)
            {
                MapViewModel.Connections.Remove(connection);             
            }
        }

        private void RootLayout_MouseLeave(object sender, MouseEventArgs e)
        {
            MapViewModel.DelLines.Clear();
        }

        private void RootCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DeselectDevices();

            if (rootCanvas.IsMouseCaptured && dragSelectionCanvas.Visibility == Visibility.Visible)
            {
                double leftX = Math.Round(Canvas.GetLeft(dragSelectionBorder) / sliScale.Value);
                double rightX = leftX + Math.Round(dragSelectionBorder.ActualWidth / sliScale.Value);

                double topY = Math.Round(Canvas.GetTop(dragSelectionBorder) / sliScale.Value);
                double bottomY = topY + Math.Round(dragSelectionBorder.ActualHeight / sliScale.Value);

                SelectDevicesOnMap(leftX, rightX, topY, bottomY);
            }

            rootCanvas.ReleaseMouseCapture();
            dragSelectionCanvas.Visibility = Visibility.Collapsed;

            var minX = double.MaxValue;
            var maxX = 0.0;

            var minY = double.MaxValue;
            var maxY = 0.0;

            foreach (var device in selectedDevices)
            {
                if (device.X < minX) minX = device.X;
                if (device.X > maxX) maxX = device.X;

                if (device.Y < minY) minY = device.Y;
                if (device.Y > maxY) maxY = device.Y;

                AddHoldersToList(device);
            }

            leftDevice = selectedDevices.FirstOrDefault(dev => dev.X == minX);
            rightDevice = selectedDevices.FirstOrDefault(dev => dev.X == maxX);
            topDevice = selectedDevices.FirstOrDefault(dev => dev.Y == minY);
            bottomDevice = selectedDevices.FirstOrDefault(dev => dev.Y == maxY);
        }

        private void SelectDevicesOnMap(double leftX, double rightX, double topY, double bottomY)
        {
            foreach (var device in MapViewModel.MapDevices)
            {
                bool xInSelection = device.X >= leftX && device.X + device.Width <= rightX;
                bool yInSelection = device.Y >= topY && device.Y + device.Height <= bottomY;

                if (xInSelection && yInSelection)
                {
                    SelectDevice(device);
                }
            }
        }

        /// <summary>
        /// Выделение устройства на карте
        /// </summary>  
        private void SelectDevice(DeviceOnMapModel device)
        {
            device.Selected = true;
            selectedDevices.Add(device);
        }

        /// <summary>
        ///  Снятие выделения с устройств на карте
        /// </summary>
        private void DeselectDevices()
        {
            foreach (var device in MapViewModel.MapDevices)
            {
                device.Selected = false;
            }
            selectedDevices.Clear();
            holdersForMove.Clear();
        }

        private void AddHoldersToList(DeviceOnMapModel device)
        {
            var uplink1Line = device.Uplink1.Line;
            var uplink2Line = device.Uplink2.Line;

            if (uplink1Line != null)
            {
                foreach (var holder in uplink1Line.Holders)
                {
                    if (!holdersForMove.Contains(holder))
                        holdersForMove.Add(holder);
                }
            }

            if (uplink2Line != null)
            {
                foreach (var holder in uplink2Line.Holders)
                {
                    if (!holdersForMove.Contains(holder))
                        holdersForMove.Add(holder);
                }
            }

            foreach (var port in device.Ports)
            {
                var line = port.Line;
                if (line == null) continue;
                foreach (var holder in line.Holders)
                {
                    if (!holdersForMove.Contains(holder))
                        holdersForMove.Add(holder);
                }
            }
        }

        private void Holder_DragDelta(object sender, DragDeltaEventArgs e)
        {
            Thumb thumb = (Thumb)sender;
            HolderOnMapModel holder = (HolderOnMapModel)thumb.DataContext;

            var maxX = rootLayout.ActualWidth / sliScale.Value - thumb.ActualWidth;
            var maxY = rootLayout.ActualHeight / sliScale.Value - thumb.ActualHeight;

            var newX = holder.X + e.HorizontalChange;
            var newY = holder.Y + e.VerticalChange;

            if (newX >= maxX) newX = maxX;
            if (newX <= 0) newX = 0;

            if (newY >= maxY) newY = maxY;
            if (newY <= 0) newY = 0;

            // Перемещаем холдер
            holder.X = newX;
            holder.Y = newY;
        }

        private void UpdateDragSelectionRect(Point clickPosition, Point currentPosition)
        {
            double width, height;

            var maxX = rootLayout.ActualWidth;
            var maxY = rootLayout.ActualHeight;

            if (currentPosition.X > maxX) currentPosition.X = maxX;
            if (currentPosition.Y > maxY) currentPosition.Y = maxY;

            if (currentPosition.X < 0) currentPosition.X = 0;
            if (currentPosition.Y < 0) currentPosition.Y = 0;

            if (currentPosition.X > clickPosition.X)
            {
                Canvas.SetLeft(dragSelectionBorder, clickPosition.X);
            }
            else
            {
                Canvas.SetLeft(dragSelectionBorder, currentPosition.X);
            }

            if (currentPosition.Y > clickPosition.Y)
            {
                Canvas.SetTop(dragSelectionBorder, clickPosition.Y);
            }
            else
            {
                Canvas.SetTop(dragSelectionBorder, currentPosition.Y);
            }

            width = Math.Abs(currentPosition.X - clickPosition.X);
            height = Math.Abs(currentPosition.Y - clickPosition.Y);

            dragSelectionBorder.Width = width;
            dragSelectionBorder.Height = height;
        }      

        private void DeleteDeviceOnMap_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Item = (MenuItem)sender;
            DeviceOnMapModel selectedDevice = (DeviceOnMapModel)Item.DataContext;
            bool deviceExists = MapViewModel.MapDevices.Any(x => x == selectedDevice);
            if (deviceExists)
            {
                MapViewModel.MapDevices.Remove(MapViewModel.MapDevices.SingleOrDefault(x => x == selectedDevice));
            }
        }

        private void OpenInBrowser_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Item = (MenuItem)sender;
            DeviceOnMapModel selectedDevice = (DeviceOnMapModel)Item.DataContext;
            bool deviceExists = MapViewModel.MapDevices.Any(x => x == selectedDevice);
            if (deviceExists)
            {
                try
                {
                    var sInfo = new ProcessStartInfo($"http://{selectedDevice.Ip}")
                    {
                        UseShellExecute = true,
                    };


                    Process.Start(sInfo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }          
        }

        private void Thumb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (sender is Thumb thumb && thumb.DataContext is DeviceOnMapModel device)
            {
                device.Width = Math.Round(thumb.ActualWidth);
            }
        }

        private void Thumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            try
            {

                Thumb thumb = (Thumb)sender;
                DeviceOnMapModel dragDevice = (DeviceOnMapModel)thumb.DataContext;

                var devInSelectedDevices = selectedDevices.FirstOrDefault(x => x == dragDevice);
                if (devInSelectedDevices == null)
                {
                    DeselectDevices();
                }


                var maxX = rootLayout.ActualWidth / sliScale.Value;
                var maxY = rootLayout.ActualHeight / sliScale.Value;

                if (!selectedDevices.Any())
                {
                    var deltaX = e.HorizontalChange;
                    var deltaY = e.VerticalChange;

                    var newX = dragDevice.X + deltaX;
                    var newY = dragDevice.Y + deltaY;

                    if (newX < 0)
                        newX = 0;
                    if (newX + dragDevice.Width > maxX)
                        newX = maxX - dragDevice.Width;

                    if (newY < 0)
                        newY = 0;
                    if (newY + dragDevice.Height > maxY)
                        newY = maxY - dragDevice.Height;

                    deltaX = newX - dragDevice.X;
                    deltaY = newY - dragDevice.Y;
                    var deltaPoint = new Point(deltaX, deltaY);

                    dragDevice.Move(deltaPoint);

                    MoveEndsLines(dragDevice, deltaPoint);

                    StartLinePoint = new Point();
                    EndLinePoint = new Point();
                }
                else
                {
                    var deltaX = e.HorizontalChange;
                    var deltaY = e.VerticalChange;

                    if (leftDevice != null && leftDevice.X <= 0 && deltaX < 0)
                        deltaX = 0;
                    if (rightDevice != null && rightDevice.X + rightDevice.Width >= maxX && deltaX > 0)
                        deltaX = 0;
                    if (topDevice != null && topDevice.Y <= 0 && deltaY < 0)
                        deltaY = 0;
                    if (bottomDevice != null && bottomDevice.Y + bottomDevice.Height >= maxY && deltaY > 0)
                        deltaY = 0;

                    var deltaPoint = new Point(deltaX, deltaY);

                    foreach (var device in selectedDevices)
                    {
                        device.Move(deltaPoint);
                        MoveEndsLines(device, deltaPoint);

                    }

                    foreach (var holder in holdersForMove)
                    {
                        holder.X += deltaPoint.X;
                        holder.Y += deltaPoint.Y;
                    }
                }
            }
            catch
            {
                //
            }
        }

        private static void MoveEndsLines(DeviceOnMapModel device, Point deltaPoint)
        {
            var uplink1Line = device.Uplink1.Line;
            var uplink2Line = device.Uplink2.Line;

            if (uplink1Line != null && uplink1Line.OriginPort.ParentDevice == device)
                MoveOriginPointLine(uplink1Line, deltaPoint);

            if (uplink1Line != null && uplink1Line.DestinationPort.ParentDevice == device)
                MoveDestinationPointLine(uplink1Line, deltaPoint);

            if (uplink2Line != null && uplink2Line.OriginPort.ParentDevice == device)
                MoveOriginPointLine(uplink2Line, deltaPoint);

            if (uplink2Line != null && uplink2Line.DestinationPort.ParentDevice == device)
                MoveDestinationPointLine(uplink2Line, deltaPoint);

            foreach (var port in device.Ports)
            {
                var line = port.Line;
                if (line != null && line.OriginPort.ParentDevice == device)
                    MoveOriginPointLine(line, deltaPoint);

                if (line != null && line.DestinationPort.ParentDevice == device)
                    MoveDestinationPointLine(line, deltaPoint);
            }

        }

        private static void MoveOriginPointLine(ConnectionOnMapModel line, Point deltaPoint)
        {
            var newX = line.OriginPoint.X + deltaPoint.X;
            var newY = line.OriginPoint.Y + deltaPoint.Y;
            line.OriginPoint = new Point(newX, newY);
        }

        /// <summary>
        /// Перемещает конечную точку линии
        /// </summary>
        private static void MoveDestinationPointLine(ConnectionOnMapModel line, Point deltaPoint)
        {
            var x = line.DestinationPoint.X + deltaPoint.X;
            var y = line.DestinationPoint.Y + deltaPoint.Y;
            line.DestinationPoint = new Point(x, y);
        }

        bool previousLineWasCreated = true;
        PortOnMapModel originPort;
        Point originPoint;
        PortOnMapModel destinationPort;
        Point destinationPoint;
        Point StartLinePoint = new(0, 0);
        Point EndLinePoint = new(0, 0);

    


        /// <summary>
        /// Для установки начальной и конечной точки рисования линии.
        /// </summary>        
        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = (Rectangle)sender;
            PortOnMapModel port = (PortOnMapModel)rect.DataContext;

            if (port.Line != null)
            {
                previousLineWasCreated = false;
                Console.WriteLine("port.Line != null");
                return;
            }

            if (!previousLineWasCreated)
            {
                previousLineWasCreated = true;
                return;
            }

            var posInRect = e.GetPosition(rect); 
            var pos = e.GetPosition(rootCanvas);     

            var centerPosX = (pos.X - posInRect.X + rect.Width / 2) / sliScale.Value; 
            var centerPosY = (pos.Y - posInRect.Y + rect.Height / 2) / sliScale.Value;

            centerPosX = Math.Round(centerPosX);
            centerPosY = Math.Round(centerPosY);

            if (StartLinePoint.X == 0 || StartLinePoint.Y == 0)
            {
                StartLinePoint.X = centerPosX;
                StartLinePoint.Y = centerPosY;

                originPort = port;
                originPoint = new Point(centerPosX, centerPosY);

            }
            else if (EndLinePoint.X == 0 || EndLinePoint.Y == 0)
            {
                EndLinePoint.X = centerPosX;
                EndLinePoint.Y = centerPosY;

                destinationPort = port;
                destinationPoint = new Point(centerPosX, centerPosY);

                ConnectionOnMapModel newConnection = new(originPort, originPoint, destinationPort, destinationPoint);

                originPort.Line = newConnection;
                destinationPort.Line = newConnection;

                if (newConnection.OriginPort.ParentDevice != newConnection.DestinationPort.ParentDevice)
                {
                    for (int i = 1; i < newConnection.NumberOfSegments; i++)
                    {
                        var point = newConnection.Points[i];
                        var holder = new HolderOnMapModel();
                        holder.X = Math.Round(point.X - holder.Width / 2);
                        holder.Y = Math.Round(point.Y - holder.Height / 2);

                        holder.Connection = newConnection;
                        newConnection.Holders.Add(holder);
                        MapViewModel.Holders.Add(holder);
                    }

                    MapViewModel.Connections.Add(newConnection);     

                }
                else
                {              
                    newConnection.OriginPort.Line = null;
                    newConnection.DestinationPort.Line = null;
                }

                StartLinePoint = new Point();
                EndLinePoint = new Point();
            }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var element = (UIElement)sender;
            var position = e.GetPosition(element);
            var transform = (MatrixTransform)element.RenderTransform;
            var matrix = transform.Matrix;
            var scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);         
        }

        public void Dispose() => throw new NotImplementedException();
    }
}
