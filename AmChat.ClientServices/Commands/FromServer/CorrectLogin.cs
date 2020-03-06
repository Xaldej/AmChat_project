using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands.FromServer
{
    public class CorrectLogin : Command
    {
        public override string Name => "CorrectLogin";

        public override void Execute(IMessengerService messenger, string data)
        {
            messenger.User.Id = Guid.Parse(data);
            var message = "/getconactlist:" + data;
            messenger.SendCommand(message);
        }
    }
}
