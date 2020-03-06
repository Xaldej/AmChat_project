using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpServer = new TcpServer("127.0.0.1", 8888);
            tcpServer.StartServer();
        }
    }
}
