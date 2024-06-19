using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace TFortisDeviceManager.Helpers
{
    public static class IpAddressHelper
    {
        public static ArrayList GetIfaceAddrList()
        {
            ArrayList ifaceAddrList = new();
            var netInterface = NetworkInterface.GetAllNetworkInterfaces();

            foreach (NetworkInterface item in netInterface)
            {
                if (item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork & !IPAddress.IsLoopback(ip.Address))
                        {
                            ifaceAddrList.Add(ip.Address);
                        }
                    }
                }
            }
            return ifaceAddrList;
        }

        public static IEnumerable<IPAddress> GetIpAddressList(string startIp, string endIp)
        {
            uint uintStartIp = IpStringToUint(startIp);
            var uintEndIp = IpStringToUint(endIp);

            for (uint ipUint = uintStartIp; ipUint <= uintEndIp; ipUint++)
            {
                yield return IpUintToIPAddress(ipUint);
            }
        }

        public static bool ValidateIPv4(string? ipString, bool emptyIsValid = true)
        {
            // В данном случае считаем, что пустая строка - валидное значение
            if (string.IsNullOrWhiteSpace(ipString))
            {
                return emptyIsValid;
            }            

            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }


            return splitValues.All(r => byte.TryParse(r, out byte tempForParsing));
        }

        public static IPAddress IpStringToIpAddress(string ipString)
        {
            uint ipUint = IpStringToUint( ipString);
            IPAddress ip = IpUintToIPAddress(ipUint);

            return ip;

        }

        public static uint IpStringToUint(string ipString)
        {
            var ipAddress = IPAddress.Parse(ipString);
            var ipBytes = ipAddress.GetAddressBytes();
            var ip = (uint)ipBytes[0] << 24;
            ip += (uint)ipBytes[1] << 16;
            ip += (uint)ipBytes[2] << 8;
            ip += (uint)ipBytes[3];

            return ip;
        }

        private static IPAddress IpUintToIPAddress(uint ipUint)
        {
            var ipBytes = BitConverter.GetBytes(ipUint);

            var ipBytesRevert = new byte[4];
            ipBytesRevert[0] = ipBytes[3];
            ipBytesRevert[1] = ipBytes[2];
            ipBytesRevert[2] = ipBytes[1];
            ipBytesRevert[3] = ipBytes[0];

            return new IPAddress(ipBytesRevert);
        }
    }
}
