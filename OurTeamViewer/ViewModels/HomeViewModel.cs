using OurTeamViewer.Commands;
using OurTeamViewer.Helpers;
using OurTeamViewer.NetworkHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace OurTeamViewer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public RelayCommand StartServerCommand { get; set; }
        private ObservableCollection<Client> allClients;

        public ObservableCollection<Client> AllClients
        {
            get { return allClients; }
            set { allClients = value; OnPropertyChanged(); }
        }
        public HomeViewModel()
        {

            StartServerCommand = new RelayCommand((obj) =>
            {
                Task.Run(() =>
                {
                    Network.Connect();

                }).Wait(100);
                Task.Run(async() =>
                {
                    while (true)
                    {
                            await Task.Delay(2000);
                            await App.Current.Dispatcher.BeginInvoke(() =>
                            {
                                try
                                {

                                AllClients = new ObservableCollection<Client>();



                                foreach (var item in Network.Clients)
                                {
                                        Task.Run(() =>
                                        {
                                            var stream = item.GetStream();
                                            var br = new BinaryReader(stream);
                                            while (true)
                                            {
                                                try
                                                {
                                                    br = new BinaryReader(stream);
                                                    var imageBytes = br.ReadBytes(500000);
                                                    ImageHelper helper = new ImageHelper();
                                                    var path = helper.GetImagePath(imageBytes);

                                                    AllClients.Add(new Client
                                                    {
                                                        TcpClient = item,
                                                        Title = "Monitor " + item.Client.RemoteEndPoint.ToString(),
                                                        ImagePath = path
                                                    });
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine($"{item.Client.RemoteEndPoint}  disconnected");
                                                }
                                            }
                                        });

                                       
                                }
                            }
                                catch (Exception)
                                {
                                    AllClients.Clear();
                                }
                            });
                    }
                });
            });
        }
    }
}
