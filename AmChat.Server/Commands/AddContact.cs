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
    public class AddContact : Command
    {
        public override string Name => "AddContact";

        private string ErrorMessage { get; set; }

        public Action<UserChat> NewChatIsCreated;

        public override void Execute(IMessengerService messenger, string data)
        {
            try
            {
                AddContactForUser(messenger, data);
            }
            catch
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Error adding contact. Check user login and try again");
                messenger.SendMessage(error);
            }
        }

        private void AddContactForUser(IMessengerService messenger, string loginToAdd)
        {
            if (IsLoginToAddCorrect(messenger, loginToAdd))
            {
                User userToAdd = GetUserFromDB(loginToAdd);
                var userInfoToAdd = UserToUserInfo(userToAdd);
                
                if (CanUserBeAdded(messenger, userToAdd))
                {
                    var chat = AddChatAndRelationshipsToDb(messenger, userInfoToAdd);
                    var userChat = ChatToUserChat(chat, messenger, userInfoToAdd);
                    messenger.UserChats.Add(userChat);
                    NewChatIsCreated(userChat);
                }
                else
                {
                    var error = CommandConverter.CreateJsonMessageCommand("/servererror", ErrorMessage);
                    messenger.SendMessage(error);
                }
            }
            else
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", ErrorMessage);
                messenger.SendMessage(error);
                return;
            }
        }

        private Chat AddChatAndRelationshipsToDb(IMessengerService messenger, UserInfo userInfoToAdd)
        {
            using (var context = new AmChatContext())
            {
                var usersToAdd = new List<UserInfo>()
                        {
                            messenger.User,
                            userInfoToAdd
                        };

                var chat = AddChat(context, usersToAdd);

                var users = new List<UserInfo>()
                {
                    messenger.User,
                    userInfoToAdd
                };

                AddChatsForUsersInDb(users, chat);

                AddUsersInChat(context, usersToAdd, chat);

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

        private void AddChatsForUsersInDb(List<UserInfo> users, Chat chat)
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

        private Chat AddChat(AmChatContext context, List<UserInfo> usersToAdd)
        {
            var id = Guid.NewGuid();
            var name = string.Empty;
            var userIds = new List<Guid>();

            foreach (var user in usersToAdd)
            {
                name += user.Login;
                userIds.Add(user.Id);
            }
            var chat = new Chat()
            {
                Id = id,
                Name = name,
                IsMultipleUsersChat = false,
            };

            context.Chats.Add(chat);

            return chat;
        }

        private User GetUserFromDB(string login)
        {
            User user;
            using (var context = new AmChatContext())
            {
                user = context.Users.Where(u => u.Login == login).FirstOrDefault();
            }

            return user;
        }


        private bool CanUserBeAdded(IMessengerService messenger, User userToAdd)
        {
            if (userToAdd == null)
            {
                ErrorMessage = "Contact is not found";
                return false;
            }

            return true;
        }

        private bool IsLoginToAddCorrect(IMessengerService messenger, string loginToAdd)
        {
            if (loginToAdd == messenger.User.Login)
            {
                ErrorMessage = "Connot add yourself. Check contact login and try again";
                return false;
            }

            var isContactInUserChats = messenger.UserChats.Where(uc => uc.Name == loginToAdd).FirstOrDefault() != null;
            if (isContactInUserChats)
            {
                ErrorMessage = "Contact is already in your list";
                return false;
            }

            return true;
        }


        private UserChat ChatToUserChat(Chat chat, IMessengerService messenger, UserInfo userToAdd)
        {
            List<UserInfo> usersInChat = new List<UserInfo>()
            {
                messenger.User,
                userToAdd,
            };
            return new UserChat()
            {
                Id = chat.Id,
                Name = chat.Name,
                UsersInChat = usersInChat,
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
