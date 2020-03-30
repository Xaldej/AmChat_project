using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using AmChat.ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AmChat.Server
{
    public class TcpServer
    {
        List<ServerMessengerService> ConnectedClients { get; set; }

        List<Chat> ActiveChats { get; set; }


        ChatMaintenanceService ChatMaintenanceService { get; set; }

        ServerSenderService ServerSender { get; set; }


        public TcpServer()
        {
            ConnectedClients = new List<ServerMessengerService>();

            ActiveChats = new List<Chat>();

            ServerSender = new ServerSenderService(ActiveChats, ConnectedClients);

            ChatMaintenanceService = new ChatMaintenanceService(ActiveChats, ConnectedClients, ServerSender);
        }


        public void StartServer(TcpSettings tcpSettings)
        {
            TcpListener server = null;

            try
            {
                server = new TcpListener(tcpSettings.EndPoint);

                server.Start();

                Console.WriteLine("Waiting for connections");

                while (true)
                {
                    TcpClient tcpClient = server.AcceptTcpClient();
                    
                    Task.Run(()=>AddClient(tcpClient));
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
            var client = new ServerMessengerService(tcpClient);
            client.UserChats.CollectionChanged += ChatMaintenanceService.OnUserChatsChanged;
            client.ClientDisconnected += RemoveClient;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void RemoveClient(IMessengerService client)
        {
            var clientToRemove = ConnectedClients.Where(c => c.Equals(client)).FirstOrDefault();
            ConnectedClients.Remove(clientToRemove);

            ChatMaintenanceService.ChangeChatListenersAmount(client);
        }
    }
}