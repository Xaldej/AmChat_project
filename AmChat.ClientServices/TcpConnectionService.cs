using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class TcpConnectionService
    {
        private TcpSettings TcpSettings { get; set; }


        public Action<string, bool> ErrorIsGotten;


        public TcpConnectionService(TcpSettings tcpSettings)
        {
            TcpSettings = tcpSettings;
        }


        public TcpClient Connect()
        {
            var TcpClient = new TcpClient();

            try
            {
                TcpClient.Connect(TcpSettings.EndPoint);
            }
            catch
            {
                var error = "Cannot connect to server. Check your interner connection and restart the app";
                ErrorIsGotten(error, true);
            }

            return TcpClient;
        }
    }
}
