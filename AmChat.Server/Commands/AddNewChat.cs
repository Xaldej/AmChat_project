using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server.Commands
{
    public class AddNewChat : Command
    {
        public override string Name => "AddNewChat";

        public NewChatInfo NewChatInfo { get; set; }

        public Action<UserChat> NewChatIsCreated;

        public override void Execute(IMessengerService messenger, string data)
        {
            NewChatInfo = JsonParser<NewChatInfo>.JsonToOneObject(data);

            try
            {
                CreateChat(messenger);
            }
            catch
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Error adding chat try again");
                messenger.SendMessage(error);
            }
        }

        private void CreateChat(IMessengerService messenger)
        {
            var usersToAdd = GetUsersFromDB(NewChatInfo.LoginsToAdd);

            var usersInfoToAdd = new List<UserInfo>();
            usersInfoToAdd.Add(messenger.User);

            foreach (var userToAdd in usersToAdd)
            {
                var user = UserToUserInfo(userToAdd);
                if(!usersInfoToAdd.Contains(user))
                {
                    usersInfoToAdd.Add(user);
                }
            }

            var chat = AddChatAndRelationshipsToDb(messenger, usersInfoToAdd);
            var userChat = ChatToUserChat(chat, messenger, usersInfoToAdd);
            messenger.UserChats.Add(userChat);
            NewChatIsCreated(userChat);
        }

        private Chat AddChatAndRelationshipsToDb(IMessengerService messenger, List<UserInfo> usersInfoToAdd)
        {
            using (var context = new AmChatContext())
            {
                var chat = AddChatToDb(context, usersInfoToAdd);

                AddChatsToDbForUsers(usersInfoToAdd, chat);

                AddUsersInChat(context, usersInfoToAdd, chat);

                context.SaveChanges();

                return chat;
            }
        }

        private void AddUsersInChat(AmChatContext context, List<UserInfo> usersToAdd, Chat chat)
        {
            foreach (var user in usersToAdd)
            {
                var userInChat = new UserInChat()
                {
                    Chat = chat,
                    UserId = user.Id,
                };

                context.UsersInChat.Add(userInChat);
            }
        }

        private void AddChatsToDbForUsers(List<UserInfo> users, Chat chat)
        {
            using (var context = new AmChatContext())
            {
                foreach (var user in users)
                {
                    var usersChat = new UsersChat()
                    {
                        UserId = user.Id,
                        ChatId = chat.Id,
                    };

                    context.UsersChats.Add(usersChat);
                }

                context.SaveChanges();
            }
        }

        private Chat AddChatToDb(AmChatContext context, List<UserInfo> usersToAdd)
        {
            var id = Guid.NewGuid();
            var userIds = new List<Guid>();

            foreach (var user in usersToAdd)
            {   
                userIds.Add(user.Id);
            }
            var chat = new Chat()
            {
                Id = id,
                Name = NewChatInfo.Name,
            };

            context.Chats.Add(chat);

            return chat;
        }

        private List<User> GetUsersFromDB(List<string> logins)
        {
            var users = new List<User>();

            foreach (var login in logins)
            {
                using (var context = new AmChatContext())
                {
                    var user = context.Users.Where(u => u.Login == login).FirstOrDefault();
                    if(user!=null)
                    {
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        private UserChat ChatToUserChat(Chat chat, IMessengerService messenger, List<UserInfo> usersToAdd)
        {
            
            return new UserChat()
            {
                Id = chat.Id,
                Name = chat.Name,
                UsersInChat = usersToAdd,
            };
        }

        private UserInfo UserToUserInfo(User user)
        {
            return new UserInfo()
            {
                Id = user.Id,
                Login = user.Login
            };
        }
    }
}
