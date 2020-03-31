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
        public Action<string> NewServerError;

        public void Execute(IMessengerService messenger, string data)
        {
            NewServerError(data);
        }
    }
}
