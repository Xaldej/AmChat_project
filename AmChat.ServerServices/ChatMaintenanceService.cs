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
        private List<ChatInfo> ActiveChats { get; set; }

        private Dictionary<Guid, int> ChatListenersAmount { get; set; }

        private readonly List<IMessengerService> connectedClients;

        private readonly IChatHistoryService chatHistoryService;

        private readonly IServerSenderService serverSender;


        public ChatMaintenanceService(List<ChatInfo> activeChats, List<IMessengerService> connectedClients, IServerSenderService serverSender)
        {
            ActiveChats = activeChats;

            this.connectedClients = connectedClients;

            this.serverSender = serverSender;

            ChatListenersAmount = new Dictionary<Guid, int>();

            chatHistoryService = new ChatHistoryService();
        }


        public void ChangeChatListenersAmount(IMessengerService client)
        {
            foreach (var chat in client.UserChats)
            {
                if (!ChatListenersAmount.ContainsKey(chat.Id))
                {
                    break;
                }

                ChatListenersAmount[chat.Id]--;

                if (ChatListenersAmount[chat.Id] == 0)
                {
                    RemoveInactiveChat(chat);
                }
            }
        }

        public void ProcessChatsChange(object sender, NotifyCollectionChangedEventArgs e)
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
            if (!(e.NewItems[0] is ChatInfo chat))
            {
                return;
            }

            var existingChat = ActiveChats.Where(c => c.Id == chat.Id).FirstOrDefault();
            if (existingChat != null)
            {
                if (!(sender is ObservableCollection<ChatInfo> chatsCollection))
                {
                    return;
                }

                var i = chatsCollection.IndexOf(chat);
                chatsCollection[i] = existingChat;
                ChatListenersAmount[existingChat.Id]++;
            }
            else
            {
                var serverChat = chat as ServerChat;
                var chatMessages = new ObservableCollection<ChatMessage>();
                chatMessages.CollectionChanged += serverChat.OnNewMessageInChat;
                chat.ChatMessages = chatMessages;
                ActiveChats.Add(chat);

                serverChat.NewMessageInChat += serverSender.SendNewMessageToUsersInChat;
                serverChat.NewUserInChat += AddChatToClientAndServer;
                ChatListenersAmount[chat.Id] = 1;
            }
        }

        private void AddChatToClientAndServer(UserInfo newUser, ChatInfo chat)
        {
            AddChatToServer(newUser, chat);
            AddChatToClient(newUser, chat);
            

            string notification = $"{newUser.Login} is added";
            serverSender.SendNotificationToChat(chat, notification);
        }

        private void AddChatToClient(UserInfo newUser, ChatInfo chat)
        {
            var chatToSend = new ChatInfo()
            {
                Id = chat.Id,
                Name = chat.Name,
                UsersInChat = chat.UsersInChat,
                ChatMessages = chatHistoryService.GetChatHistory(chat.Id).ToList(),
            };

            var commandJson = CommandMaker.GetCommandJson<ChatIsAdded, ChatInfo>(chatToSend);

            serverSender.SendCommandToCertainUser(newUser, commandJson);
        }

        private void AddChatToServer(UserInfo user, ChatInfo chat)
        {
            var serverChat = connectedClients.Where(c => c.User.Equals(user)).FirstOrDefault();

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

        private void RemoveInactiveChat(ChatInfo chat)
        {
            ActiveChats.Remove(chat);
            ChatListenersAmount.Remove(chat.Id);
        }
    }
}
