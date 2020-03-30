using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class MessageToCertainChatHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            var message = JsonParser<ChatMessage>.JsonToOneObject(data);

            var chat = messenger.UserChats.Where(c => c.Id == message.ToChatId).FirstOrDefault();
            chat.ChatMessages.Add(message);
        }
    }
}
