using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TFortisDeviceManager.Converters
{
    public class ColumnSettingsConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values.Clone();
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return (object[])DependencyProperty.UnsetValue;
        }
    }
}
