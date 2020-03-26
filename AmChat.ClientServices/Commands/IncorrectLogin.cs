using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands
{
    public class IncorrectLogin : Command
    {
        public override string Name => "IncorrectLogin";

        public Action IncorrectLoginData;

        public override void Execute(IMessengerService messenger, string data)
        {
            IncorrectLoginData();
        }
    }
}
