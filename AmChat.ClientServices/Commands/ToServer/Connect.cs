using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands.ToServer
{
    public class Connect : Command
    {
        public Action<string> SendError;

        public override string Name => "Connect";

        public override void Execute(IMessengerService messenger, string data)
        {
            var tcpClient = messenger.TcpClient = new TcpClient();

            //todo: ger from config
            var Ip = "127.0.0.1";
            var Port = 8888;

            IPAddress IpAddr = IPAddress.Parse(Ip);
            IPEndPoint EndPoint = new IPEndPoint(IpAddr, Port);

            try
            {
                tcpClient.Connect(EndPoint);
            }
            catch
            {
                var error = "Cannot connect to server. Check your interner connection and restart the app";
                SendError(error);
            }
        }
    }
}
