using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class TcpSettings
    {
        public string Ip { get; set; }

        public int Port { get; set; }

        public IPAddress IpAddr => IPAddress.Parse(Ip);

        private IPEndPoint _endPoint;

        public IPEndPoint EndPoint => _endPoint = _endPoint ?? new IPEndPoint(IpAddr, Port);

        TcpSettings()
        {

        }

        public TcpSettings(string ip, int port)
        {
            Ip = ip;
            Port = port;
        }
    }
}
