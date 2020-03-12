using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands
{
    public class CorrectAddingContact : Command
    {
        public Action<UserInfo> ContactIsGotten;

        public override string Name => "CorrectAddingContact";

        public override void Execute(IMessengerService messenger, string data)
        {
            var userToAdd = JsonParser<UserInfo>.JsonToOneObject(data);

            messenger.UserContacts.Add(userToAdd);
            ContactIsGotten(userToAdd);
        }
    }
}
