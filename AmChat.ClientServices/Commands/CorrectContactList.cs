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
        public override string Name => "CorrectContactList";

        public override void Execute(IMessengerService messenger, string data)
        {   
            var chats = new ObservableCollection<UserChat>(JsonParser<IEnumerable<UserChat>>.JsonToOneObject(data));

            foreach (var chat in chats)
            {
                if(chat.ChatMessages==null)
                {
                    chat.ChatMessages = new ObservableCollection<MessageToChat>();
                }

                messenger.UserChats.Add(chat);
            }

            var command = CommandConverter.CreateJsonMessageCommand("/getunreadmessages", string.Empty);
            messenger.SendMessage(command);
        }
    }
}
