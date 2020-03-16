using AlexeyMelentyevProject_ChatServer;
using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
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

        List<UserChat> UsersChats { get; set; }

        public ServerMessenger Messenger { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public TcpServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();

            UsersChats = new List<UserChat>();
        }

        public TcpServer(string ip, int port)
        {
            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();

            UsersChats = new List<UserChat>();
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
            var client = new ServerMessenger(tcpClient, ConnectedClients);
            client.UserChats.CollectionChanged += AddChat;
            client.ClientDisconnected += DeleteDisconnectedClient;
            client.NewChatIsCreated += SendNotificationsAboutNewChat;

            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void SendNotificationsAboutNewChat(UserChat chat)
        {
            foreach (var user in chat.UsersInChat)
            {
                var message = new MessageToChat()
                {
                    ToChat = chat,
                    Text = "New chat is created"
                };

                SendMessageToCertainUser(user, message);
            }
        }

        private void AddChat(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is UserChat chat))
            {
                return;
            }

            if (!UsersChats.Contains(chat))
            {
                UsersChats.Add(chat);
                chat.ChatMessages.CollectionChanged += SendMessagesToUser;
            }
        }

        private void SendMessagesToUser(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is MessageToChat messageToChat))
            {
                return;
            }

            var chat = UsersChats.Where(c => c.Equals(messageToChat.ToChat)).FirstOrDefault();

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

        private void SendMessageToCertainUser(UserInfo userToSend, MessageToChat messageToChat)
        {

            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend == null)
            {
                //TO DO: save to DB
            }
            else
            {
                if (!clientToSend.UserChats.Contains(messageToChat.ToChat))
                {
                    clientToSend.UserChats.Add(messageToChat.ToChat);
                }
                clientToSend.SendMessageToExistingChat(messageToChat);
            }
        }
    }
}