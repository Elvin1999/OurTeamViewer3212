using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OurTeamViewer.NetworkHelper
{
    public class Network
    {
        public static string IP = "10.2.22.1";
        public static int PORT = 27001;
        static TcpListener listener = null;
        static BinaryWriter bw = null;
        static BinaryReader br = null;
        public static List<TcpClient> Clients { get; set; }

        public static void Connect()
        {
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
                                        //MessageBox.Show($"CLIENT : {client.Client.RemoteEndPoint} : {msg}");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine($"{item.Client.RemoteEndPoint}  disconnected");
                                        Clients.Remove(item);
                                    }
                                }
                            }).Wait(50);
                        }



                    });

                });
            }
        }
    }
}

