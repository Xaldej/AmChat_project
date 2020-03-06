using AlexeyMelentyevProject_ChatServer;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AmChat.Server
{
    public class TcpServer
    {
        public int Port { get; set; }
        public string Ip { get; set; }

        public List<ServerMessenger> ConnectedClients { get; set; }

        public ServerMessenger Messenger { get; set; }

        TcpServer()
        {
            Ip = "127.0.0.1";
            Port = 8888;
        }

        public TcpServer(string ip, int port)
        {
            Ip = ip;
            Port = port;

            ConnectedClients = new List<ServerMessenger>();
        }

        public void StartServer()
        {
            TcpListener server = null;

            try
            {
                //to do: get from config
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                server = new TcpListener(localAddr, Port);

                server.Start();

                Console.WriteLine("Waiting for connections");

                while (true)
                {
                    TcpClient tcpClient = server.AcceptTcpClient();

                    // async?
                    AddClient(tcpClient);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }

        private void AddClient(TcpClient tcpClient)
        {
            var client = new ServerMessenger(tcpClient, ConnectedClients);
            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }
    }
}