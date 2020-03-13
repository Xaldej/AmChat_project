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
                    AddChatForUser(messenger, userInfoToAdd, chat);
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


        private void AddChatForUser(IMessengerService messenger, UserInfo userToAdd, Chat chat)
        {
            var userChat = ChatToUserChat(chat, messenger, userToAdd);

            messenger.UserChats.Add(userChat);

            var commandData = JsonParser<UserChat>.OneObjectToJson(userChat);
            var command = CommandConverter.CreateJsonMessageCommand("/correctaddingcontact", commandData);
            messenger.SendMessage(command);
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

                AddChatForUser(context, messenger, chat);

                AddUsersInChat(context, usersToAdd, chat);

                context.SaveChanges();

                return chat;
            }
        }

        private void AddUsersInChat(AmChatContext context, List<UserInfo> usersToAdd, Chat chat)
        {
            foreach (var user in usersToAdd)
            {
                var userInChat = new UsersInChat()
                {
                    Id = Guid.NewGuid(),
                    ChatId = chat.Id,
                    UserId = user.Id,
                };

                context.UsersInChat.Add(userInChat);
            }
        }

        private void AddChatForUser(AmChatContext context, IMessengerService messenger, Chat chat)
        {
            var usersChat = new UsersChats()
            {
                Id = Guid.NewGuid(),
                UserId = messenger.User.Id,
                ChatId = chat.Id,
            };

            context.UsersChats.Add(usersChat);
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
