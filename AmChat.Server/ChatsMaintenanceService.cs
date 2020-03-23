﻿using AlexeyMelentyevProject_ChatServer;
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

        User ServerNotificationUser { get; set; }

        List<UnreadNotification> UnreadNotifications { get; set; }

        public ChatsMaintenanceService(List<Chat> activeChats, List<ServerMessenger> connectedClients)
        {
            ActiveChats = activeChats;
            
            ConnectedClients = connectedClients;

            UnreadNotifications = new List<UnreadNotification>();

            ServerNotificationUser = new User()
            {
                Id = Guid.NewGuid(),
                Login = "Server Notification",
            };
        }

        public void AddChatsForUsers(Chat chat)
        {
            foreach (var user in chat.UsersInChat)
            {
                AddChatToClientAndServerMessengers(chat, user);
                SendNotificationAboutNewChat(chat, user);
            }
        }

        public void AddChatToClientAndServerMessengers(Chat chat, User user)
        {
            var serverChat = ConnectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

            if(serverChat==null)
            {
                return;
            }

            var isChatAlreadyInUserChats = serverChat.UserChats.Contains(chat);
            if (!isChatAlreadyInUserChats)
            {
                serverChat.UserChats.Add(chat);
            }

            var chatJson = JsonParser<Chat>.OneObjectToJson(chat);
            var command = CommandConverter.CreateJsonMessageCommand("/chatisadded", chatJson);
            SendCommandToCertainUser(user, command);
        }

        public void SendCommandToCertainUser(User userToSend, string command)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend != null)
            {
                clientToSend.SendMessage(command);
            }
        }

        

        public void SendMessageToCertainUser(User userToSend, MessageToChat messageToChat)
        {
            var clientToSend = ConnectedClients.Where(c => c.User.Equals(userToSend)).FirstOrDefault();

            if (clientToSend == null)
            {
                var unreadNotification = new UnreadNotification(userToSend, messageToChat.ToChatId);

                UnreadNotifications.Add(unreadNotification);
            }
            else
            {
                clientToSend.SendMessageToExistingChat(messageToChat);
            }
        }

        public void SendUnreadMessages(User user)
        {
            var unreadNotificationForUser = UnreadNotifications.Where(m => m.ForUser.Equals(user));

            foreach (var unreadNotification in unreadNotificationForUser)
            {
                unreadNotification.IsSent = true;
                try
                {
                    var command = CommandConverter.CreateJsonMessageCommand("/unreadmessagesinchat", unreadNotification.ToChatId.ToString());
                    SendCommandToCertainUser(user, command);
                }
                catch
                {
                    unreadNotification.IsSent = false;
                }
            }

            UnreadNotifications.RemoveAll(m => m.IsSent);
        }

        public void SendNotificationAboutNewChat(Chat chat, User user)
        {
            var message = new MessageToChat()
            {
                FromUser = ServerNotificationUser,
                ToChatId = chat.Id,
                Text = "New chat is created"
            };

            SendMessageToCertainUser(user, message);
        }

        internal ObservableCollection<MessageToChat> GetChatHistory(Chat chat)
        {
            //TO DO

            return new ObservableCollection<MessageToChat>();
        }

        public void SendNewMessageToUsers(object sender, NotifyCollectionChangedEventArgs e)
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

        internal void SaveChatHistory(Chat chat)
        {
            //TO DO:
        }
    }


}
