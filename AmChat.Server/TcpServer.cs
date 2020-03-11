using AlexeyMelentyevProject_ChatServer;
using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
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

        public ServerMessenger Messenger { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public TcpServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();
        }

        public TcpServer(string ip, int port)
        {
            TcpSettings = new TcpSettings(ip, port);

            ConnectedClients = new List<ServerMessenger>();
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
            client.NewMwssageForCertainUserIsGotten += SendMessageToCertainUser;
            ConnectedClients.Add(client);

            var thread = new Thread(new ThreadStart(client.ListenMessages));
            thread.Start();

            Console.WriteLine("client is connected");
        }

        private void SendMessageToCertainUser(MessageToUser messageToSend)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(messageToSend.ToUser)).FirstOrDefault();

            if (clientToSend == null)
            {
                //TO DO: save to DB
            }
            else
            {
                if(clientToSend.UserContacts.Contains(messageToSend.FromUser))
                {
                    clientToSend.SendMessage(messageToSend);
                }
                else
                {
                    try
                    {
                        AddSenderToContacts(messageToSend, clientToSend);
                        clientToSend.SendMessage(messageToSend);
                    }
                    catch
                    {
                        var clientToSendError = ConnectedClients.Where(c => c.User.Equals(messageToSend.FromUser)).FirstOrDefault();
                        clientToSendError.SendCommand("/servererror:Error senging message. Try again");
                    }
                }
                
            }
        }

        private static void AddSenderToContacts(MessageToUser messageToSend, ServerMessenger clientToSend)
        {
            using (var context = new AmChatContext())
            {
                clientToSend.UserContacts.Add(messageToSend.FromUser);

                var contactRelationship = new ContactRelationship()
                {
                    Id = Guid.NewGuid(),
                    UserId = clientToSend.User.Id,
                    ContactId = messageToSend.FromUser.Id,
                };

                context.ContactRelationships.Add(contactRelationship);

                context.SaveChanges();
            }
        }
    }
}