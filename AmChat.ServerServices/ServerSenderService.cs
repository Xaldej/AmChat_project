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


        public ServerSenderService(List<IMessengerService> connectedClients)
        {
            ConnectedClients = connectedClients;

            GetServerNotificationUser();
        }


        public void SendCommandToCertainUser(UserInfo user, string command)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessage(command);
            }
        }

        public void SendMessageToCertainUser(UserInfo user, ChatMessage message)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            if (clientToSend != null)
            {   
                var commandJson = CommandExtentions.GetCommandJson<MessageToCertainChat, ChatMessage>(message);

                clientToSend.SendMessage(commandJson);
            }
        }

        public void SendNewMessageToUsersInChat(ChatMessage message, Chat chat)
        {
            var usersToSend = chat.UsersInChat.Where(u => !u.Equals(message.FromUser)).ToList();

            foreach (var user in usersToSend)
            {
                SendMessageToCertainUser(user, message);
            }
        }

        public void SendNotificationToChat(Chat chat, string notification)
        {
            var message = new ChatMessage()
            {
                FromUser = ServerNotificationUser,
                ToChatId = chat.Id,
                DateAndTime = DateTime.Now,
                Text = notification,
            };

            chat.ChatMessages.Add(message);
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
