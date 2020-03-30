using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class ServerErrorHandler : ICommandHandler
    {
        public Action<string> SendError;

        public void Execute(IMessengerService messenger, string data)
        {
            SendError(data);
        }
    }
}
