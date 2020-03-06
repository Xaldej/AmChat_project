using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public bool CheckIsCalled(string command) => command.ToLower().Contains("/" + Name.ToLower());

        public abstract void Execute(IMessengerService messenger, string data);
    }
}
