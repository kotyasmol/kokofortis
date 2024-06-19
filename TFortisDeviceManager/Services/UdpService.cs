using System.Collections.Generic;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using TFortisDeviceManager.Helpers;
using System;

namespace TFortisDeviceManager.Services;

public class UdpService
{
    public static void SendBroadcastMessage(int port, byte[] data)
    {
        IPEndPoint targetEndPoint = new(IPAddress.Broadcast, port);

        ArrayList ifaceAddrList = IpAddressHelper.GetIfaceAddrList();

        foreach (IPAddress ipAddr in ifaceAddrList)
        {
            IPEndPoint localEndPoint = new(ipAddr, port);
            UdpClient client = new(localEndPoint);

            client.Send(data, data.Length, targetEndPoint);
            client.Close();
        }
    }

    public static void SendUnicastMessage(IEnumerable<IPAddress> ipAddressRange,  int port, byte[] message)
    {
        ArrayList ifaceAddrList = IpAddressHelper.GetIfaceAddrList();

        foreach (var ip in ipAddressRange)
        {
            IPEndPoint targetEndPoint = new(ip, port);
            foreach (IPAddress localIp in ifaceAddrList)
            {
                IPEndPoint localEndPoint = new(localIp, port);
                UdpClient client = new(localEndPoint);
                try
                {
                    client.Send(message, message.Length, targetEndPoint);
                }
                catch (SocketException)
                {
                    // ignore
                }
                finally
                {
                    client.Close();
                }
            }
        }
    }    

    public static IReadOnlyList<byte[]> ReceiveDataArray(byte response, UdpClient udpClient, CancellationToken token)
    {
        List<byte[]> result = new();
        IPEndPoint remoteIpEndPoint = new(IPAddress.Any, 0);

        udpClient.Client.ReceiveTimeout = 1000;
        while (true)
        {
            try
            {
                var data = udpClient.Receive(ref remoteIpEndPoint);
                if (data[0] == response)
                {
                    result.Add(data);
                }
            }
            catch (SocketException)
            {
                // ignore
            }

            if (token.IsCancellationRequested)
            {
                break;
            }
        }
        return result;
    }

    public static byte[] ReceiveSingleData(byte response, UdpClient udpClient, CancellationToken token)
    {
        byte[] data;
        udpClient.Client.ReceiveTimeout = 1000;
        IPEndPoint RemoteIpEndPoint = new (IPAddress.Any, 0);

        while (true)
        {
            try
            {
                if (udpClient.Available > 0)
                {
                    data = udpClient.Receive(ref RemoteIpEndPoint);
                    if (data[0] == response)
                    {                        
                        return data;
                    }
                }
            }
            catch (SocketException)
            {
                // ignore
            }

            if (token.IsCancellationRequested)
            {
                break;
            }
        }
        
        return Array.Empty<byte>();
    }
}