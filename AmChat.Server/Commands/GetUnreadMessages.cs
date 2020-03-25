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
    public class GetUnreadMessages : Command
    {
        public Action<User> UnreadMessagesAreAsked;
        public override string Name => "GetUnreadMessages";

        public override void Execute(IMessengerService messenger, string data)
        {
            //TO DO: delete class
        }
    }
}
