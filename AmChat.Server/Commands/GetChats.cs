using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server.Commands
{
    public class GetChats : Command
    {
        public override string Name => "GetChats";

        public override void Execute(IMessengerService messenger, string data)
        {
            List<DBChat> chats = new List<DBChat>();
            try
            {
                chats = GetChatsFromDb(messenger.User);
            }
            catch
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Cannot load contact list. Try to restart the app");
                messenger.SendMessage(error);
            }

            if (chats.Count() > 0)
            {
                foreach(var chat in chats)
                {
                    var userChat = ChatToUserChat(chat);
                    messenger.UserChats.Add(userChat);
                }

                var chatsInfo = ChatsToChatsInfo(messenger.UserChats);
                var chatsInfoJson = JsonParser<ChatInfo>.ManyObjectsToJson(chatsInfo);

                var command = CommandConverter.CreateJsonMessageCommand("/correctcontactlist", chatsInfoJson);
                messenger.SendMessage(command);
            }
        }

        private ObservableCollection<ChatInfo> ChatsToChatsInfo(ObservableCollection<Chat> chats)
        {
            var chatsInfo = new ObservableCollection<ChatInfo>();

            foreach (var chat in chats)
            {
                var chatInfo = new ChatInfo()
                {
                    Id = chat.Id,
                    Name = chat.Name,
                    ChatMessages = chat.ChatMessages,
                    UsersInChat = chat.UsersInChat,
                };
                chatsInfo.Add(chatInfo);
            }

            return chatsInfo;
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

        private Chat ChatToUserChat(DBChat chat)
        {
            List<UserInfo> usersInChat = GetUsersInChat(chat);

            return new Chat()
            {
                Id = chat.Id,
                Name = chat.Name,
                UsersInChat = new ObservableCollection<UserInfo>(usersInChat),
            };
        }

        private List<UserInfo> GetUsersInChat(DBChat chat)
        {
            List<UserInfo> users = new List<UserInfo>();
            using (var context = new AmChatContext())
            {
                var userIds = context.ChatUsers.Where(uinc => uinc.ChatId == chat.Id).Select(uinc => uinc.UserId).ToList();

                foreach (var userId in userIds)
                {
                    var user = context.Users.Where(u => u.Id == userId).FirstOrDefault();
                    users.Add(UserToUserInfo(user));
                }
            }

            return users;
        }

        private UserInfo UserToUserInfo(DBUser user)
        {
            return new UserInfo()
            {
                Id = user.Id,
                Login = user.Login,
            };
        }
    }
}
