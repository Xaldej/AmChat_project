using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public class CommandMessage
    {
        public string Command;

        public string Data;

        public CommandMessage(string command, string data)
        {
            Command = command;
            Data = data;
        }
    }
}
