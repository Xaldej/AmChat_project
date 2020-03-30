using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class CloseConnectionHandler : ICommandHandler
    {
        public Action<IMessengerService> ConnectionIsClosed;

        public void Execute(IMessengerService messenger, string data)
        {
            ConnectionIsClosed(messenger);
        }
    }
}
