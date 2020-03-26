using AlexeyMelentyevProject_ChatServer;
using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace AmChat.Server
{
    public class ChatsMaintenanceService
    {
        List<Chat> ActiveChats { get; set; }

        public List<ServerMessenger> ConnectedClients { get; set; }

        UserInfo ServerNotificationUser { get; set; }

        public ChatsMaintenanceService(List<Chat> activeChats, List<ServerMessenger> connectedClients)
        {
            ActiveChats = activeChats;
            
            ConnectedClients = connectedClients;

            GetServerNotificationUser();
            
        }

        private void GetServerNotificationUser()
        {
            var id = Guid.Parse("212d2a32-79fa-46ca-9a42-46c823c675af");

            using (var context = new AmChatContext())
            {
                var dbUser = context.Users.Where(u => u.Id == id).FirstOrDefault();
                if(dbUser==null)
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

        private ChatInfo ChatToChatInfo(Chat chat)
        {
            return new ChatInfo()
            {
                Id = chat.Id,
                Name = chat.Name,
                ChatMessages = chat.ChatMessages,
                UsersInChat = chat.UsersInChat,
            };
        }

        public void SendCommandToCertainUser(UserInfo userToSend, string command)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessage(command);
            }
        }

        public void SendMessageToCertainUser(UserInfo userToSend, ChatMessage messageToChat)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessageToExistingChat(messageToChat);

            }
        }

        public void AddChatToClientAndServer(UserInfo newUser, Chat chat)
        {
            AddChatToServer(newUser, chat);
            AddChatToClient(newUser, chat);

            string notification = $"{newUser.Login} is added";
            SendNotificationToChat(chat, notification);
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

        private void AddChatToClient(UserInfo newUser, Chat chat)
        {
            var chatInfo = ChatToChatInfo(chat);
            var chatInfoJson = JsonParser<ChatInfo>.OneObjectToJson(chatInfo);
            var command = CommandConverter.CreateJsonMessageCommand("/chatisadded", chatInfoJson);
            SendCommandToCertainUser(newUser, command);
        }

        private void AddChatToServer(UserInfo user, Chat chat)
        {
            var serverChat = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            if (serverChat == null)
            {
                return;
            }

            var isChatAlreadyInUserChats = serverChat.UserChats.Contains(chat);
            if (!isChatAlreadyInUserChats)
            {
                serverChat.UserChats.Add(chat);
            }
        }



        public void SendNewMessageToUsers(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action==NotifyCollectionChangedAction.Add)
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
    }
}
