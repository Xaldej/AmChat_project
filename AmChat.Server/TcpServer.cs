using AlexeyMelentyevProject_ChatServer;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace AmChat.Server
{
    public class TcpServer
    {
        List<Chat> ActiveChats { get; set; }

        Dictionary<Chat, int> ChatListenersAmount { get; set; }

        public List<ServerMessenger> ConnectedClients { get; set; }

        ChatsMaintenanceService ChatsMaintenanceService { get; set; }

        public ServerMessenger Messenger { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public TcpServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();

            ActiveChats = new List<Chat>();

            ChatListenersAmount = new Dictionary<Chat, int>();

            ChatsMaintenanceService = new ChatsMaintenanceService(ActiveChats, ConnectedClients);
        }

        public void StartServer()
        {
            TcpListener server = null;

            try
            {
                server = new TcpListener(TcpSettings.EndPoint);

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
            var client = new ServerMessenger(tcpClient);
            client.UserChats.CollectionChanged += OnUserChatsChanged;
            client.ClientDisconnected += DeleteDisconnectedClient;
            client.NewChatIsCreated += ChatsMaintenanceService.AddChatsForUsers;
            client.UnreadMessagesAreAsked += ChatsMaintenanceService.SendUnreadMessages;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void OnUserChatsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddActiveChat(sender, e);
                    break;
                    
                default:
                    break;
            }
            
        }

        private void AddActiveChat(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is Chat chat))
            {
                return;
            }

            if (ActiveChats.Contains(chat))
            {
                if (!(sender is ObservableCollection<Chat> chatsCollection))
                {
                    return;
                }

                var existingChat = ActiveChats.Where(c => c.Equals(chat)).FirstOrDefault();

                var i = chatsCollection.IndexOf(chat);
                chatsCollection[i] = existingChat;
                ChatListenersAmount[existingChat]++;
            }
            else
            {
                chat.ChatMessages = ChatsMaintenanceService.GetChatHistory(chat);
                ActiveChats.Add(chat);
                chat.ChatMessages.CollectionChanged += ChatsMaintenanceService.SendNewMessageToUsers;
                //chat.NewUserInChat+=
                ChatListenersAmount[chat] = 1;
            }
        }

        private void DeleteDisconnectedClient(IMessengerService client)
        {
            var clientToRemove = ConnectedClients.Where(c => c.Equals(client)).FirstOrDefault();
            ConnectedClients.Remove(clientToRemove);

            ChangeChatListenersAmount(client);
        }

        private void ChangeChatListenersAmount(IMessengerService client)
        {
            foreach (var chat in client.UserChats)
            {
                ChatListenersAmount[chat]--;
                if(ChatListenersAmount[chat]==0)
                {
                    Task.Run(()=>RemoveInactiveChat(chat));
                }
            }
        }

        private void RemoveInactiveChat(Chat chat)
        {
            ChatsMaintenanceService.SaveChatHistory(chat);
            ActiveChats.Remove(chat);
            ChatListenersAmount.Remove(chat);
        }
    }
}