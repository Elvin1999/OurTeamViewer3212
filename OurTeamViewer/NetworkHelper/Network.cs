using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OurTeamViewer.NetworkHelper
{
    public class Network
    {
        public static string IP = "10.2.11.19";
        public static int PORT = 27001;
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;
        public static List<TcpClient> Clients { get; set; }
        public static string GetHostName()
        {
            string hostName = Dns.GetHostName();
            return hostName;
        }
        [Obsolete]
        public static string GetIpAdress(string HostName)
        {
            string IP = Dns.GetHostByName(HostName).AddressList[0].ToString();
            return IP;
        }
        public static string GetLocalIpAddress()
        {
            UnicastIPAddressInformation mostSuitableIp = null;
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (var network in networkInterfaces)
            {
                if (network.OperationalStatus != OperationalStatus.Up)
                    continue;
                var properties = network.GetIPProperties();
                if (properties.GatewayAddresses.Count == 0)
                    continue;
                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;
                    if (IPAddress.IsLoopback(address.Address))
                        continue;
                    if (!address.IsDnsEligible)
                    {
                        if (mostSuitableIp == null)
                            mostSuitableIp = address;
                        continue;
                    }
                    // The best IP is the IP got from DHCP server
                    if (address.PrefixOrigin != PrefixOrigin.Dhcp)
                    {
                        if (mostSuitableIp == null || !mostSuitableIp.IsDnsEligible)
                            mostSuitableIp = address;
                        continue;
                    }
                    return address.Address.ToString();
                }
            }
            return mostSuitableIp != null
                ? mostSuitableIp.Address.ToString()
                : "";
        }

        public static void Connect()
        {
            IP = GetLocalIpAddress();
            var ip = IPAddress.Parse(IP);
            Clients = new List<TcpClient>();

            var ep = new IPEndPoint(ip, PORT);
            listener = new TcpListener(ep);
            listener.Start();
            MessageBox.Show($"Listening on {listener.LocalEndpoint}");
            while (true)
            {
                var client = listener.AcceptTcpClient();
                Clients.Add(client);

                Task.Run(() =>
                {

                    var reader = Task.Run(() =>
                    {
                        foreach (var item in Clients)
                        {
                            Task.Run(() =>
                            {
                                var stream = item.GetStream();
                                br = new BinaryReader(stream);
                                while (true)
                                {
                                    try
                                    {
                                        var msg = br.ReadString();
                                        br = new BinaryReader(stream);
                                        var bw = new BinaryWriter(stream);
                                        try
                                        {
                                            bw.Write("A");
                                        }
                                        catch (Exception)
                                        {
                                            Clients.Remove(item);
                                            break;
                                        }

                                        //MessageBox.Show($"CLIENT : {client.Client.RemoteEndPoint} : {msg}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{item.Client.RemoteEndPoint}  disconnected");
                                        //Clients.Remove(item);
                                    }
                                }
                            });
                        }



                    });

                });
            }
        }
    }
}

