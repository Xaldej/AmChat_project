using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces
{
    public interface ICommandHandler
    {
        void Execute(IMessengerService messenger, string data);
    }
}
