using System;

namespace TFortisDeviceManager.Services
{
    public class ChangingDeviceStateEventArgs : EventArgs
    {

        public string Ip { get; private set; }
        public string State { get; private set; }
        public ChangingDeviceStateEventArgs(string ip, string state)
        {
            Ip = ip;
            State = state;
        }

        public ChangingDeviceStateEventArgs(string ip)
        {
            Ip = ip;
        }

    }
}
