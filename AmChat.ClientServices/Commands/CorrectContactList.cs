using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands
{
    public class CorrectContactList : Command
    {
        public Action ContactListIsUpdated;

        public override string Name => "CorrectContactList";

        public override void Execute(IMessengerService messenger, string data)
        {   
            var chats = new ObservableCollection<UserChat>(JsonParser<IEnumerable<UserChat>>.JsonToOneObject(data));
            messenger.UserChats = chats;
            ContactListIsUpdated();
        }
    }
}
