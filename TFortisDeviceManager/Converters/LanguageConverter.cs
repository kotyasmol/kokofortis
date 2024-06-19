using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Data;

namespace TFortisDeviceManager.Converters
{
    public class LanguageConverter : IValueConverter
    {
        public static readonly Dictionary<string, string> translationDictionary = new Dictionary<string, string>
        {
            {"Link на порту #1", "Link on port #1" },
            {"Link на порту #2", "Link on port #2" },
            {"Link на порту #3", "Link on port #3" },
            {"Link на порту #4", "Link on port #4" },
            {"Link на порту #5", "Link on port #5" },
            {"Link на порту #6", "Link on port #6" },
            {"Link на порту #7", "Link on port #7" },
            {"Link на порту #8", "Link on port #8" },
            {"Link на порту #9", "Link on port #9" },
            {"Link на порту #10", "Link on port #10" },
            {"Link на порту #11", "Link on port #11" },
            {"Link на порту #12", "Link on port #12" },
            {"Link на порту #13", "Link on port #13" },
            {"Link на порту #14", "Link on port #14" },
            {"Link на порту #15", "Link on port #15" },
            {"Link на порту #16", "Link on port #16" },
            {"Состояние подачи PoE на порту #1", "PoE status on port #1" },
            {"Состояние подачи PoE на порту #2", "PoE status on port #2" },
            {"Состояние подачи PoE на порту #3", "PoE status on port #3" },
            {"Состояние подачи PoE на порту #4", "PoE status on port #4" },
            {"Состояние подачи PoE на порту #5", "PoE status on port #5" },
            {"Состояние подачи PoE на порту #6", "PoE status on port #6" },
            {"Состояние подачи PoE на порту #7", "PoE status on port #7" },
            {"Состояние подачи PoE на порту #8", "PoE status on port #8" },
            {"Текущее состояние Sensor 1", "Sensor 1 current state" },
            {"Текущее состояние Sensor 2", "Sensor 2 current state" },
            {"Текущее состояние входа #1", "Current input #1 state" },
            {"Текущее состояние входа #2", "Current input #2 state" },
            {"Текущее состояние входа #3", "Current input #3 state" },
            {"Текущее состояние входа #4", "Current input #4 state" },
            {"Текущее состояние входа #5", "Current input #1 state" },
            {"Текущее состояние выхода #1", "Current output #1 state" },
            {"Текущее состояние выхода #2", "Current output #2 state" },
            {"Текущее состояние выхода #3", "Current output #3 state" },
            {"Текущее состояние выхода #4", "Current output #4 state" },
            {"Текущее состояние выхода #5", "Current output #5 state" },
            {"Текущее состояние выхода #6", "Current output #6 state" },
            {"Текущее состояние выхода #7", "Current output #7 state" },
            {"Текущее состояние выхода #8", "Current output #8 state" },
            {"Текущее состояние выхода #9", "Current output #9 state" },
            {"Текущее состояние датчика вскрытия", "Current tamper sensor state" },
            {"Источник питания", "Power supply" },
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US" && translationDictionary.ContainsKey(value.ToString()))
            {
                return translationDictionary[value.ToString()];
            }

            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
