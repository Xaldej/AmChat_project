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
    class MessageToCertainChat : Command
    {
        public Action<MessageToChat> NewMessageIsGotten;

        public override string Name => "MessageToCertainChat";

        public override void Execute(IMessengerService messenger, string data)
        {
            var message = JsonParser<MessageToChat>.JsonToOneObject(data);

            NewMessageIsGotten(message);
        }
    }
}
