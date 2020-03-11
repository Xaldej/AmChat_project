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
            catch (Exception e)
            {
                var errorMessage = "/servererror:" +
                    "Error adding contact\n\n" +
                    "Details:\n" + e.Message;
                messenger.SendCommand(errorMessage);
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
                    messenger.SendCommand("/servererror:Contact is not found");
                }
                else
                {
                    UserInfo userInfoToAdd = UserToUserInfo(userToAdd);
                    if (messenger.UserContacts.Contains(userInfoToAdd))
                    {
                        messenger.SendCommand("/servererror:Contact is already in your list");
                    }
                    else if(messenger.User.Equals(userInfoToAdd))
                    {
                        messenger.SendCommand("/servererror:Connot add yourself. Check contact name and try again");
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

                        var command = "/correctaddingcontact:" + JsonParser<UserInfo>.OneObjectToJson(userInfoToAdd);
                        messenger.SendCommand(command);
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
