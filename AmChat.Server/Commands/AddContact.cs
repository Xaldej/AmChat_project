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

        public override void Execute(IMessengerService messenger, string data)
        {
            try
            {
                AddContactFromDb(messenger, data);
            }
            catch
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Error adding contact. Check user login and try again");
                messenger.SendMessage(error);
            }

        }

        private void AddContactFromDb(IMessengerService messenger, string data)
        {
            var user = messenger.User;

            User userToAdd;
            using (var context = new AmChatContext())
            {
                userToAdd = context.Users.Where(u => u.Login == data).FirstOrDefault();

                if (userToAdd == null)
                {
                    var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Contact is not found");
                    messenger.SendMessage(error);
                }
                else
                {
                    UserInfo userInfoToAdd = UserToUserInfo(userToAdd);
                    if (messenger.UserContacts.Contains(userInfoToAdd))
                    {
                        var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Contact is already in your list");
                        messenger.SendMessage(error);
                    }
                    else if(messenger.User.Equals(userInfoToAdd))
                    {
                        var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Connot add yourself. Check contact login and try again");
                        messenger.SendMessage(error);
                    }
                    else
                    {
                        messenger.UserContacts.Add(userInfoToAdd);

                        var contactRelationship = new ContactRelationship()
                        {
                            Id = Guid.NewGuid(),
                            UserId = messenger.User.Id,
                            ContactId = userToAdd.Id,
                        };

                        context.ContactRelationships.Add(contactRelationship);

                        context.SaveChanges();

                        var commandData = JsonParser<UserInfo>.OneObjectToJson(userInfoToAdd);
                        var command = CommandConverter.CreateJsonMessageCommand("/correctaddingcontact", commandData);
                        messenger.SendMessage(command);
                    }

                }
            }
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
