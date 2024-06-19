using System.Collections.Generic;

namespace TFortisDeviceManager.Models
{
    public  class TimezoneDictionary
    {
        private static readonly Dictionary<string, int> Timezones = new()
        {
            { "UTC - 12:00", -12},
            { "UTC - 11:00", -11},
            { "UTC - 10:00", -10},
            { "UTC - 9:00", -9},
            { "UTC - 8:00", -8},
            { "UTC - 7:00", -7},
            { "UTC - 6:00", -6},
            { "UTC - 5:00", -5},
            { "UTC - 4:00", -4},
            { "UTC - 3:00", -3},
            { "UTC - 2:00", -2},
            { "UTC - 1:00", -1},
            { "UTC ± 0:00", 0},
            { "UTC + 1:00", 1},
            { "UTC + 2:00", 2},
            { "UTC + 3:00", 3},
            { "UTC + 4:00", 4},
            { "UTC + 5:00", 5},
            { "UTC + 6:00", 6},
            { "UTC + 7:00", 7},
            { "UTC + 8:00", 8},
            { "UTC + 9:00", 9},
            { "UTC + 10:00", 10},
            { "UTC + 11:00", 11},
            { "UTC + 12:00", 12},
            { "UTC + 13:00", 13},

        };

        public static Dictionary<string, int> GetTimezones()
        {
            return Timezones;
        }
    }
}
