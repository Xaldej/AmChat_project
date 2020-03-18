using AlexeyMelentyevProject_ChatServer;
using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AmChat.Server
{
    public class TcpServer
    {
        public List<ServerMessenger> ConnectedClients { get; set; }

        List<UserChat> ActiveChats { get; set; }

        UserInfo ServerNotificationUser { get; set; }

        public ServerMessenger Messenger { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public TcpServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();

            ActiveChats = new List<UserChat>();

            ServerNotificationUser = new UserInfo()
            {
                Id = Guid.NewGuid(),
                Login = "Server Notification",
            };
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
            var client = new ServerMessenger(tcpClient);
            client.UserChats.CollectionChanged += AddChat;
            client.ClientDisconnected += DeleteDisconnectedClient;
            client.NewChatIsCreated += AddChatsForUsers;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void AddChatsForUsers(UserChat chat)
        {
            foreach (var user in chat.UsersInChat)
            {
                AddChatToClientAndServerMessengers(chat, user);
                SendNotificationAboutNewChat(chat, user);
            }
        }

        private void SendNotificationAboutNewChat(UserChat chat, UserInfo user)
        {
            var message = new MessageToChat()
            {
                FromUser = ServerNotificationUser,
                ToChatId = chat.Id,
                Text = "New chat is created"
            };

            SendMessageToCertainUser(user, message);
        }

        private void AddChatToClientAndServerMessengers(UserChat chat, UserInfo user)
        {
            var serverChat = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            var isChatAlreadyInUserChats = serverChat.UserChats.Contains(chat);
            if(!isChatAlreadyInUserChats)
            {
                serverChat.UserChats.Add(chat);
            }

            var chatJson = JsonParser<UserChat>.OneObjectToJson(chat);
            var command = CommandConverter.CreateJsonMessageCommand("/chatisadded", chatJson);
            SendCommandToCertainUser(user, command);
        }

        private void AddChat(object sender, NotifyCollectionChangedEventArgs e)
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
            if (!(e.NewItems[0] is UserChat chat))
            {
                return;
            }

            if (ActiveChats.Contains(chat))
            {
                if (!(sender is ObservableCollection<UserChat> chatsCollection))
                {
                    return;
                }

                var existingChat = ActiveChats.Where(c => c.Equals(chat)).FirstOrDefault();

                var i = chatsCollection.IndexOf(chat);
                chatsCollection[i] = existingChat;
            }
            else
            {
                chat.ChatMessages = new ObservableCollection<MessageToChat>();
                ActiveChats.Add(chat);
                chat.ChatMessages.CollectionChanged += SendMessagesToUser;
            }
        }

        private void SendMessagesToUser(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is MessageToChat messageToChat))
            {
                return;
            }

            var chat = ActiveChats.Where(c => c.Id ==messageToChat.ToChatId).FirstOrDefault();

            var usersToSend = chat.UsersInChat.Where(u => !u.Equals(messageToChat.FromUser)).ToList();

            foreach(var user in usersToSend)
            {
                SendMessageToCertainUser(user, messageToChat);
            }
        }

        private void DeleteDisconnectedClient(ServerMessenger client)
        {
            ConnectedClients.Remove(client);
        }



        private void SendCommandToCertainUser(UserInfo userToSend, string command)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessage(command);
            }
        }

        private void SendMessageToCertainUser(UserInfo userToSend, MessageToChat messageToChat)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend == null)
            {
                //TO DO: save to DB
            }
            else
            {
                clientToSend.SendMessageToExistingChat(messageToChat);
            }
        }
    }
}