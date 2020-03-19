using AlexeyMelentyevProject_ChatServer;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server
{
    public class ChatsMaintenanceService
    {
        List<UserChat> ActiveChats { get; set; }

        public List<ServerMessenger> ConnectedClients { get; set; }

        UserInfo ServerNotificationUser { get; set; }

        List<MessageToChat> UnreadMessages { get; set; }

        public ChatsMaintenanceService(List<UserChat> activeChats, List<ServerMessenger> connectedClients)
        {
            ActiveChats = activeChats;
            
            ConnectedClients = connectedClients;

            UnreadMessages = new List<MessageToChat>();

            ServerNotificationUser = new UserInfo()
            {
                Id = Guid.NewGuid(),
                Login = "Server Notification",
            };
        }

        public void AddChatsForUsers(UserChat chat)
        {
            foreach (var user in chat.UsersInChat)
            {
                AddChatToClientAndServerMessengers(chat, user);
                SendNotificationAboutNewChat(chat, user);
            }
        }

        public void AddChatToClientAndServerMessengers(UserChat chat, UserInfo user)
        {
            var serverChat = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            var isChatAlreadyInUserChats = serverChat.UserChats.Contains(chat);
            if (!isChatAlreadyInUserChats)
            {
                serverChat.UserChats.Add(chat);
            }

            var chatJson = JsonParser<UserChat>.OneObjectToJson(chat);
            var command = CommandConverter.CreateJsonMessageCommand("/chatisadded", chatJson);
            SendCommandToCertainUser(user, command);
        }

        public void SendCommandToCertainUser(UserInfo userToSend, string command)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessage(command);
            }
        }

        public void SendMessageToCertainUser(UserInfo userToSend, MessageToChat messageToChat)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend == null)
            {
                UnreadMessages.Add(messageToChat);
                //TO DO: save messages to DB if thera too many messages
            }
            else
            {
                clientToSend.SendMessageToExistingChat(messageToChat);
            }
        }

        public void SendNotificationAboutNewChat(UserChat chat, UserInfo user)
        {
            var message = new MessageToChat()
            {
                FromUser = ServerNotificationUser,
                ToChatId = chat.Id,
                Text = "New chat is created"
            };

            SendMessageToCertainUser(user, message);
        }

        public void SendMessagesToUser(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is MessageToChat messageToChat))
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
