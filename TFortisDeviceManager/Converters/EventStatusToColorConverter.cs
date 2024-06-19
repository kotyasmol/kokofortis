using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TFortisDeviceManager.Converters
{ 
    public class EventStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                "Ok" => Color.Empty,
                "Warning" => Color.Empty,
                "Problem" => "Orange",
                "Info" => Color.Empty,
                "Error" => "Red",
                _ => Color.Empty,
            };
        }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return DependencyProperty.UnsetValue;
    }
    }
}