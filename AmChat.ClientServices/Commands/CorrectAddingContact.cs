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
    public class CorrectAddingContact : Command
    {
        public override string Name => "CorrectAddingContact";

        public override void Execute(IMessengerService messenger, string data)
        {
            var chatToAdd = JsonParser<UserChat>.JsonToOneObject(data);

            if(chatToAdd.ChatMessages==null)
            {
                chatToAdd.ChatMessages = new ObservableCollection<MessageToChat>();
            }

            messenger.UserChats.Add(chatToAdd);
        }
    }
}
