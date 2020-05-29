using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using AmChat.Infrastructure.Interfaces.ServerServices;
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
        private List<IMessengerService> ConnectedClients { get; set; }

        private List<ChatInfo> ActiveChats { get; set; }
        
        private IChatMaintenanceService ChatMaintenanceService { get; set; }
        
        private IServerSenderService ServerSender { get; set; }


        public TcpServer()
        {
            Logger.InitLogger();

            ConnectedClients = new List<IMessengerService>();

            ActiveChats = new List<ChatInfo>();

            ServerSender = new ServerSenderService(ConnectedClients);

            ChatMaintenanceService = new ChatMaintenanceService(ActiveChats, ServerSender);
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
                Logger.Log.Error(e.Message);
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
            client.UserChats.CollectionChanged += ChatMaintenanceService.ProcessChatChange;

            var commandHandler = new ServerCommandHandlerService();
            commandHandler.ClientDisconnected += RemoveClient;

            client.CommandHandler = commandHandler;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void RemoveClient(IMessengerService client)
        {
            if(client != null)
            {
                ConnectedClients.Remove(client);

                ChatMaintenanceService.ChangeChatListenersAmount(client);
            }
        }
    }
}