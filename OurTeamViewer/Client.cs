using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace OurTeamViewer
{
    public class Client
    {
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public TcpClient TcpClient { get; set; }
    }
}
