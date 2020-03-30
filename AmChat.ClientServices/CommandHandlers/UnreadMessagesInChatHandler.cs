using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class UnreadMessagesInChatHandler : ICommandHandler
    {
        public Action<Guid> NewUnreadNotification;

        public void Execute(IMessengerService messenger, string data)
        {
            var chatId = Guid.Parse(data);

            NewUnreadNotification(chatId);
        }
    }
}
