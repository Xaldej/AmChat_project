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
    public class CorrectLogin : Command
    {
        public override string Name => "CorrectLogin";

        public override void Execute(IMessengerService messenger, string data)
        {
            messenger.User.Id = Guid.Parse(data);

            var command = CommandConverter.CreateJsonMessageCommand("/getconactlist", data);

            messenger.SendMessage(command);
        }
    }
}
