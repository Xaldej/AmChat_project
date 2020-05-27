using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces
{
    public interface ICommandHandlerService
    {
        void ProcessMessage(IMessengerService messenger, string message);
    }
}
