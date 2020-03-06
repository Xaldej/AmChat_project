using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands.FromServer
{
    class MessageFromContact : Command
    {
        public Action<MessageToUser> NewMessageIsGotten;

        public override string Name => "MessageFromContact";

        public override void Execute(IMessengerService messenger, string data)
        {
            var message = JsonParser<MessageToUser>.JsonToOneObjects(data);

            NewMessageIsGotten(message);
        }
    }
}
