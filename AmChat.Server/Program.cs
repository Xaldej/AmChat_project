using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AmChat.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpServer = new TcpServer();

            var thread = new Thread(tcpServer.StartServer);
            thread.Start();
        }
    }
}
