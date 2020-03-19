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
    public class UnreadMessagesInChat : Command
    {
        public Action<Guid> NewUnreadNotification;
        public override string Name => "UnreadMessagesInChat";

        public override void Execute(IMessengerService messenger, string data)
        {
            var chatId = Guid.Parse(data);

            NewUnreadNotification(chatId);
        }
    }
}
