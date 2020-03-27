using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.Commands
{
    class AddUsersToChat : Command
    {
        public override string Name => "AddUsersToChat";

        public override void Execute(IMessengerService messenger, string data)
        {
            //TO DO
        }
    }
}
