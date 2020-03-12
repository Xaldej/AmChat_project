using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public class CommandMessage
    {
        public string CommandName;

        public string CommandData;

        public CommandMessage(string command, string data)
        {
            CommandName = command;
            CommandData = data;
        }
    }
}
