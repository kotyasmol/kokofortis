using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TFortisDeviceManager.Converters
{
    public class DeviceStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                true => PackIconMaterialKind.CheckCircle,
                false => PackIconMaterialKind.Alert,
                _ => 0,
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}