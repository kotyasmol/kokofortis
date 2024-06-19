using Stylet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Converters
{
    class PortPoeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                LinkState link = (LinkState)value;
                PoeState poe = (PoeState)parameter;

                if (link == LinkState.NotUse && poe == PoeState.NotUse)
                {
                    return new SolidColorBrush(Colors.Gray);
                }
                else if (link == LinkState.Down || poe == PoeState.Down)
                {
                    return new SolidColorBrush(Colors.Red);
                }
                else if (link == LinkState.Up && poe == PoeState.Up)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (link == LinkState.NotUse && poe == PoeState.Up)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else if (link == LinkState.Up && poe == PoeState.NotUse)
                {
                    return new SolidColorBrush(Colors.Green);
                }
                else
                {
                    return new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                return new SolidColorBrush(Colors.Red);
            }
            /* return value switch
             {
                 "Down" => "IndianRed",
                 "Up" => "DarkSeaGreen",
                 "NotUse" => "Gray",
                 _ => ""
             };*/
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
