using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ServerSenderService
    {
        List<Chat> ActiveChats { get; set; }

        public List<IMessengerService> ConnectedClients { get; set; }

        UserInfo ServerNotificationUser { get; set; }


        public ServerSenderService(List<Chat> activeChats, List<IMessengerService> connectedClients)
        {
            ActiveChats = activeChats;

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
                var messageJson = JsonParser<ChatMessage>.OneObjectToJson(message);
                var command = new MessageToCertainChat() { Data = messageJson };
                var commandJson = JsonParser<MessageToCertainChat>.OneObjectToJson(command);

                clientToSend.SendMessage(commandJson);
            }
        }

        public void SendNewMessageToUsers(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is ChatMessage messageToChat))
                {
                    return;
                }

                var chat = ActiveChats.Where(c => c.Id == messageToChat.ToChatId).FirstOrDefault();

                var usersToSend = chat.UsersInChat.Where(u => !u.Equals(messageToChat.FromUser)).ToList();

                foreach (var user in usersToSend)
                {
                    SendMessageToCertainUser(user, messageToChat);
                }
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
