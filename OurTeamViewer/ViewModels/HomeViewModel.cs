using OurTeamViewer.Commands;
using OurTeamViewer.NetworkHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTeamViewer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public RelayCommand StartServerCommand { get; set; }
        public HomeViewModel()
        {
            StartServerCommand = new RelayCommand((obj) =>
            {
                Task.Run(() =>
                {
                    Network.Connect();
                });
            });
        }
    }
}
