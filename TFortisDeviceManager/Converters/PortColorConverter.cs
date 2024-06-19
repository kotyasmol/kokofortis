using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Converters
{
    class PortColorConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                 LinkState link = (LinkState)values[0];
                PoeState poe = (PoeState)values[1];

                if (link == LinkState.NotUse && poe == PoeState.NotUse)
                    return new SolidColorBrush(Colors.Gray);

                if (link == LinkState.Down || poe == PoeState.Down)              
                    return new SolidColorBrush(Colors.Firebrick);
                
                return new SolidColorBrush(Colors.LimeGreen);
    
            }
            catch(Exception)
            {
                return new SolidColorBrush(Colors.Firebrick);
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
