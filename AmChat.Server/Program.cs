using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            var tcpSettings = new TcpSettings(ip, port);

            var thread = new Thread(()=>tcpServer.StartServer(tcpSettings));
            thread.Start();
        }
    }
}
