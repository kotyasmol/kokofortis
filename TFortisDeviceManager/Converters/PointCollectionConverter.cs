using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace TFortisDeviceManager.Converters
{
    class PointCollectionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var regPtsColl = new PointCollection(); //regular points collection.
            var obsPtsColl = (ObservableCollection<Point>)value; //observable which is used to raise INCC event.
            foreach (var point in obsPtsColl)
                regPtsColl.Add(point);
            return regPtsColl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
