using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Converters
{
    class LineColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            LinkState originPortLink = (LinkState)values[0];
            LinkState destinationLink = (LinkState)values[1];

            if (originPortLink == LinkState.Down || destinationLink == LinkState.Down)
                return new SolidColorBrush(Colors.Firebrick);

            return new SolidColorBrush(Colors.LimeGreen);
        }


        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

