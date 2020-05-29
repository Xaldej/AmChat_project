using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.Infrastructure.Interfaces.ServerServices;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class GetChatsHandler : ICommandHandler
    {
        private readonly IChatHistoryService chatHistoryService;

        private readonly IMapper mapper;


        public GetChatsHandler()
        {
            chatHistoryService = new ChatHistoryService();

            var mapperConfig = Mappings.GetGetChatsHandlerConfig();

            mapper = new Mapper(mapperConfig);
        }


        public void Execute(IMessengerService messenger, string data)
        {
            List<DBChat> chats = new List<DBChat>();
            try
            {
                chats = GetChatsFromDb(messenger.User);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);

                SendErrorMessage(messenger);
            }

            if (chats.Count() > 0)
            {
                AddChatToServerMessenger(messenger, chats);

                SendChatToClient(messenger);
            }
        }

        private void AddChatToServerMessenger(IMessengerService messenger, List<DBChat> chats)
        {
            foreach (var chat in chats)
            {
                var userChat = mapper.Map<ServerChat>(chat);
                var usersInChat = new ObservableCollection<UserInfo>(GetUsersInChat(chat));
                usersInChat.CollectionChanged += userChat.OnUsersInChatChanged;
                userChat.UsersInChat = usersInChat;
                messenger.UserChats.Add(userChat);
            }
        }

        private List<DBChat> GetChatsFromDb(UserInfo forUser)
        {
            var chats = new List<DBChat>();

            using (var context = new AmChatContext())
            {
                var chatsIds = context.UserChats.Where(uc => uc.UserId == forUser.Id).Select(uc => uc.ChatId).ToList();

                foreach (var id in chatsIds)
                {
                    var chat = context.Chats.Where(c => c.Id == id).FirstOrDefault();
                    chats.Add(chat);
                }
            }

            return chats;
        }

        private IEnumerable<ChatInfo> GetChatsToSend(IEnumerable<ChatInfo> chats)
        {
            var chatsToSend = new List<ChatInfo>();

            foreach (var chat in chats)
            {
                var chatToSend = new ChatInfo()
                {
                    Id = chat.Id,
                    Name = chat.Name,
                    UsersInChat = chat.UsersInChat,
                    ChatMessages = chatHistoryService.GetChatHistory(chat.Id).ToList(),
                };

                chatsToSend.Add(chatToSend);
            }

            return chatsToSend;
        }

        private List<UserInfo> GetUsersInChat(DBChat chat)
        {
            var users = new List<UserInfo>();
            using (var context = new AmChatContext())
            {
                var userIds = context.ChatUsers.Where(uinc => uinc.ChatId == chat.Id).Select(uinc => uinc.UserId).ToList();

                foreach (var userId in userIds)
                {
                    var user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
                    users.Add(mapper.Map<UserInfo>(user));
                }
            }

            return users;
        }

        private void SendChatToClient(IMessengerService messenger)
        {
            var chatsInfo = messenger.UserChats;

            var chatsToSend = GetChatsToSend(chatsInfo);

            var chatsJson = JsonParser<ChatInfo>.ManyObjectsToJson(chatsToSend);

            var commandJson = CommandMaker.GetCommandJson<CorrectContactList, string>(chatsJson, true);

            messenger.SendMessage(commandJson);
        }

        private void SendErrorMessage(IMessengerService messenger)
        {
            var error = new ServerError() { Data = "Cannot load contact list. Try to restart the app" };
            var errorJson = JsonParser<ServerError>.OneObjectToJson(error);

            messenger.SendMessage(errorJson);
        }

    }
}
