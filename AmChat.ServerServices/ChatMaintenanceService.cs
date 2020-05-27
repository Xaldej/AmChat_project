using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.Infrastructure.Interfaces.ServerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ChatMaintenanceService : IChatMaintenanceService
    {
        List<Chat> ActiveChats { get; set; }

        Dictionary<Chat, int> ChatListenersAmount { get; set; }

        List<IMessengerService> ConnectedClients { get; set; }

        IChatHistoryService ChatHistoryService { get; set; }

        IServerSenderService ServerSender { get; set; }


        public ChatMaintenanceService(List<Chat> activeChats, List<IMessengerService> connectedClients, IServerSenderService serverSender)
        {
            ActiveChats = activeChats;

            ConnectedClients = connectedClients;

            ServerSender = serverSender;

            ChatListenersAmount = new Dictionary<Chat, int>();

            ChatHistoryService = new ChatHistoryService();
        }


        public void ChangeChatListenersAmount(IMessengerService client)
        {
            foreach (var chat in client.UserChats)
            {
                if (!ChatListenersAmount.ContainsKey(chat))
                {
                    break;
                }

                ChatListenersAmount[chat]--;

                if (ChatListenersAmount[chat] == 0)
                {
                    RemoveInactiveChat(chat);
                }
            }
        }

        public void ProcessChatChange(object sender, NotifyCollectionChangedEventArgs e)
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
                var chatMessages = ChatHistoryService.GetChatHistory(chat);
                chatMessages.CollectionChanged += chat.OnNewMessageInChat;
                chat.ChatMessages = chatMessages;
                ActiveChats.Add(chat);

                chat.NewMessageInChat += ServerSender.SendNewMessageToUsersInChat;
                chat.NewUserInChat += AddChatToClientAndServer;
                ChatListenersAmount[chat] = 1;
            }
        }

        private void AddChatToClientAndServer(UserInfo newUser, Chat chat)
        {
            AddChatToServer(newUser, chat);
            AddChatToClient(newUser, chat);

            string notification = $"{newUser.Login} is added";
            ServerSender.SendNotificationToChat(chat, notification);
        }

        private void AddChatToClient(UserInfo newUser, Chat chat)
        {
            var chatInfo = ChatToChatInfo(chat);

            var commandJson = CommandExtentions.GetCommandJson<ChatIsAdded, ChatInfo>(chatInfo);

            ServerSender.SendCommandToCertainUser(newUser, commandJson);
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

        private void RemoveInactiveChat(Chat chat)
        {
            Task.Run(() => ChatHistoryService.SaveChatHistory(chat));
            ActiveChats.Remove(chat);
            ChatListenersAmount.Remove(chat);
        }
    }
}
