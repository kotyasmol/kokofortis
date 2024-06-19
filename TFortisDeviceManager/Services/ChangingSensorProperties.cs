using System;
using TFortisDeviceManager.Models;

namespace TFortisDeviceManager.Services
{
    public class ChangingSensorPropertiesEventArgs : EventArgs
    {
        public EventModel Evnt { get; private set; }

        public ChangingSensorPropertiesEventArgs(EventModel evnt)
        {
            Evnt = evnt;
        }
    }
}
