using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.Infrastructure.Interfaces.ServerServices;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ServerSenderService : IServerSenderService
    {
        private List<IMessengerService> ConnectedClients { get; set; }

        private UserInfo ServerNotificationUser { get; set; }

        private readonly IChatHistoryService chatHistoryService;


        public ServerSenderService(List<IMessengerService> connectedClients)
        {
            ConnectedClients = connectedClients;

            GetServerNotificationUser();

            chatHistoryService = new ChatHistoryService();
        }


        public void SendCommandToCertainUser(UserInfo user, string command)
        {
            var clientsToSend = ConnectedClients.Where(c => c.User.Equals(user));

            if (clientsToSend != null && clientsToSend.Any())
            {
                foreach (var client in clientsToSend)
                {
                    client.SendMessage(command);
                }
            }
        }

        public void SendMessageToCertainUser(UserInfo user, ChatMessage message)
        {
            var clientsToSend = ConnectedClients.Where(c => c.User.Equals(user));

            if (clientsToSend != null && clientsToSend.Any())
            {
                var commandJson = CommandMaker.GetCommandJson<MessageToCertainChat, ChatMessage>(message);
                foreach (var client in clientsToSend)
                {
                    client.SendMessage(commandJson);
                }
            }
        }

        public void SendNewMessageToUsersInChat(ChatMessage message, ChatInfo chat)
        {
            var usersToSend = chat.UsersInChat.Where(u => !u.Equals(message.FromUser)).ToList();

            foreach (var user in usersToSend)
            {
                SendMessageToCertainUser(user, message);
            }
        }

        public void SendNotificationToChat(ChatInfo chat, string notification)
        {
            var message = new ChatMessage()
            {
                FromUser = ServerNotificationUser,
                ToChatId = chat.Id,
                DateAndTime = DateTime.Now,
                Text = notification,
            };

            chat.ChatMessages.Add(message);

            Task.Run(() => chatHistoryService.AddNewMessageToChatHistory(message));

        }


        private void GetServerNotificationUser()
        {
            var id = Guid.Parse("212d2a32-79fa-46ca-9a42-46c823c675af");

            using (var context = new AmChatContext())
            {
                var dbUser = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if (dbUser == null)
                {
                    dbUser = new DBUser()
                    {
                        Id = id,
                        Login = "Chat Notification",
                    };
                    context.Users.Add(dbUser);
                    context.SaveChanges();
                }

                ServerNotificationUser = new UserInfo()
                {
                    Id = dbUser.Id,
                    Login = dbUser.Login,
                };
            }
        }
    }
}
