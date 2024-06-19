using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace TFortisDeviceManager.Converters
{
    class AvailableToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isAvailable = (bool)value;

            if (isAvailable)
            {
                return null;
            }
            else
            {
                return new SolidColorBrush(Colors.Firebrick);
            }
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
