using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ChatMaintenanceService
    {
        public List<IMessengerService> ConnectedClients { get; set; }

        public ServerSenderService ServerSender { get; set; }

        List<Chat> ActiveChats { get; set; }

        Dictionary<Chat, int> ChatListenersAmount { get; set; }

        ChatHistoryService ChatHistoryService { get; set; }


        public ChatMaintenanceService(List<Chat> activeChats, List<IMessengerService> connectedClients, ServerSenderService serverSender)
        {
            ActiveChats = activeChats;

            ConnectedClients = connectedClients;

            ServerSender = serverSender;

            ChatListenersAmount = new Dictionary<Chat, int>();

            ChatHistoryService = new ChatHistoryService();
        }


        public void AddChatToClientAndServer(UserInfo newUser, Chat chat)
        {
            AddChatToServer(newUser, chat);
            AddChatToClient(newUser, chat);

            string notification = $"{newUser.Login} is added";
            ServerSender.SendNotificationToChat(chat, notification);
        }

        public void ChangeChatListenersAmount(IMessengerService client)
        {
            foreach (var chat in client.UserChats)
            {
                ChatListenersAmount[chat]--;
                if (ChatListenersAmount[chat] == 0)
                {
                    RemoveInactiveChat(chat);
                }
            }
        }

        public void OnUserChatsChanged(object sender, NotifyCollectionChangedEventArgs e)
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
                chat.ChatMessages = ChatHistoryService.GetChatHistory(chat);
                ActiveChats.Add(chat);

                chat.ChatMessages.CollectionChanged += ServerSender.SendNewMessageToUsers;
                chat.NewUserInChat += AddChatToClientAndServer;
                ChatListenersAmount[chat] = 1;
            }
        }

        private void AddChatToClient(UserInfo newUser, Chat chat)
        {
            var chatInfo = ChatToChatInfo(chat);
            var chatInfoJson = JsonParser<ChatInfo>.OneObjectToJson(chatInfo);

            var command = new ChatIsAdded() { Data = chatInfoJson };
            var commandJson = JsonParser<ChatIsAdded>.OneObjectToJson(command);

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
