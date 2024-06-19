using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using MahApps.Metro.IconPacks;

namespace TFortisDeviceManager.Converters
{
    public class MonitoringStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                true => PackIconMaterialKind.MonitorEye,
                false => 0,
                _ => 0
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
