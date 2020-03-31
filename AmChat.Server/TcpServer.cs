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
        List<IMessengerService> ConnectedClients { get; set; }

        List<Chat> ActiveChats { get; set; }


        ChatMaintenanceService ChatMaintenanceService { get; set; }

        ServerSenderService ServerSender { get; set; }


        public TcpServer()
        {
            ConnectedClients = new List<IMessengerService>();

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
            IMessengerService client = new ServerMessengerService(tcpClient);
            client.UserChats.CollectionChanged += ChatMaintenanceService.OnUserChatsChanged;

            client.NewEvent += RemoveClient;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void RemoveClient(string userId)
        {
            var id = Guid.Parse(userId);

            var clientToRemove = ConnectedClients.Where(c => c.User.Id == id).FirstOrDefault();
            ConnectedClients.Remove(clientToRemove);

            ChatMaintenanceService.ChangeChatListenersAmount(clientToRemove);
        }
    }
}