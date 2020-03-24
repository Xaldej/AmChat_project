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
    public class AddNewChat : Command
    {
        public override string Name => "AddNewChat";

        public NewChatInfo NewChatInfo { get; set; }

        public Action<Chat> NewChatIsCreated;

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
            var dbUsersToAdd = GetUsersFromDB(NewChatInfo.LoginsToAdd);

            var usersToAdd = new List<User>();
            usersToAdd.Add(messenger.User);

            foreach (var userToAdd in dbUsersToAdd)
            {
                var user = UserToUserInfo(userToAdd);
                if(!usersToAdd.Contains(user))
                {
                    usersToAdd.Add(user);
                }
            }

            var chat = AddChatAndRelationshipsToDb(usersToAdd);
            var userChat = DbChatToChat(chat, usersToAdd);
            messenger.UserChats.Add(userChat);
            NewChatIsCreated(userChat);
        }

        private DBChat AddChatAndRelationshipsToDb(List<User> usersInfoToAdd)
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

        private void AddUsersInChat(AmChatContext context, List<User> usersToAdd, DBChat chat)
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

        private void AddChatsToDbForUsers(List<User> users, DBChat chat)
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

        private DBChat AddChatToDb(AmChatContext context, List<User> usersToAdd)
        {
            var id = Guid.NewGuid();
            var userIds = new List<Guid>();

            foreach (var user in usersToAdd)
            {   
                userIds.Add(user.Id);
            }
            var chat = new DBChat()
            {
                Id = id,
                Name = NewChatInfo.Name,
            };

            context.Chats.Add(chat);

            return chat;
        }

        private List<DBUser> GetUsersFromDB(List<string> logins)
        {
            var users = new List<DBUser>();

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

        private Chat DbChatToChat(DBChat chat, List<User> usersToAdd)
        {
            
            return new Chat()
            {
                Id = chat.Id,
                Name = chat.Name,
                UsersInChat = new ObservableCollection<User>(usersToAdd),
            };
        }

        private User UserToUserInfo(DBUser user)
        {
            return new User()
            {
                Id = user.Id,
                Login = user.Login
            };
        }
    }
}
