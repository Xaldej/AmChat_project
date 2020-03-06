using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server.Commands.FromClient
{
    public class SendMessageToUser : Command
    {
        public override string Name => "SendMessageToUser";

        public override void Execute(IMessengerService messenger, string data)
        {
            var messageToUser = JsonParser<MessageToUser>.JsonToOneObjects(data);

            //TO DO
        }
    }
}
