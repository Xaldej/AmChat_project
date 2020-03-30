using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class IncorrectLoginHandler : ICommandHandler
    {
        public Action IncorrectLoginData;

        public void Execute(IMessengerService messenger, string data)
        {
            IncorrectLoginData();
        }
    }
}
