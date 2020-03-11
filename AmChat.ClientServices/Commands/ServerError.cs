using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AmChat.ClientServices.Commands
{
    public class ServerError : Command
    {
        public Action<string> SendError;

        public override string Name => "ServerError";

        public override void Execute(IMessengerService messenger, string data)
        {
            SendError(data);
        }
    }
}
