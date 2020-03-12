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
    public class GetConactList : Command
    {
        public override string Name => "GetConactList";

        public override void Execute(IMessengerService messenger, string data)
        {
            try
            {
                messenger.UserContacts = GetContactsFromDb(messenger.User);
            }
            catch
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Cannot load contact list. Try to restart the app");
                messenger.SendMessage(error);
            }

            if (messenger.UserContacts.Count() > 0)
            {
                var contactsJson = JsonParser<UserInfo>.ManyObjectsToJson(messenger.UserContacts);

                var command = CommandConverter.CreateJsonMessageCommand("/correctcontactlist", contactsJson);
                messenger.SendMessage(command);
            }
        }

        private List<UserInfo> GetContactsFromDb(UserInfo user)
        {
            var contacts = new List<UserInfo>();

            using (var contect = new AmChatContext())
            {
                var contactsIds = contect.ContactRelationships.Where(cr => cr.UserId == user.Id).Select(cr => cr.ContactId).ToList();

                foreach (var id in contactsIds)
                {
                    var contact = contect.Users.Where(u => u.Id == id).FirstOrDefault();
                    UserInfo contactInfo = UserToUserInfo(contact);
                    contacts.Add(contactInfo);
                }
            }

            return contacts;
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
