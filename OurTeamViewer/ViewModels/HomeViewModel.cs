using OurTeamViewer.Commands;
using OurTeamViewer.NetworkHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                       await Task.Delay(500);
                       await Task.Run(() =>
                        {
                            App.Current.Dispatcher.Invoke(() =>
                            {
                                try
                                {

                                AllClients = new ObservableCollection<Client>();
                                foreach (var c in Network.Clients)
                                {
                                    AllClients.Add(new Client
                                    {
                                        TcpClient = c,
                                        Title = "Monitor " + c.Client.RemoteEndPoint.ToString(),
                                        ImagePath = ""
                                    });
                                }
                            }
                                catch (Exception)
                                {
                                    AllClients.Clear();
                                }
                            });
                        });
                    }
                });
            });
        }
    }
}
