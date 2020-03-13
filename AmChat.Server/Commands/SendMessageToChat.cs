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
    public class SendMessageToChat : Command
    {
        public override string Name => "SendMessageToChat";

        public override void Execute(IMessengerService messenger, string data)
        {
            var messageToChat = JsonParser<MessageToChat>.JsonToOneObject(data);

            var chat = messenger.UserChats.Where(c => c.Equals(messageToChat.ToChat)).FirstOrDefault();

            chat.ChatMessages.Add(messageToChat);
        }
    }
}
