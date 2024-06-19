using MahApps.Metro.IconPacks;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TFortisDeviceManager.Converters
{
    public class EventStatusToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Ok" => 0,
                "Warning" => 0,
                "Problem" => PackIconMaterialKind.Alert,
                "Error" => PackIconMaterialKind.Alert,
                "Info" => 0,
                _ => 0,
            }; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}