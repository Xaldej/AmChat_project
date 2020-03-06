using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public static class CommandIdentifier
    {
        public static bool IsMessageACommand(string command)
        {
            var result = false;

            if (command == string.Empty)
            {
                return false;
            }

            if (command[0] == '/')
            {
                result = true;
            }

            return result;
        }

        public static CommandNameAndData GetCommandAndDataFromMessage(string fullCommand)
        {
            string commandName = string.Empty;
            string commandData = string.Empty;

            var wasDividerInLine = false;

            foreach (var ch in fullCommand)
            {
                if (wasDividerInLine)
                {
                    commandData += ch;
                }
                else
                {
                    if (ch == ':')
                    {
                        wasDividerInLine = true;
                    }
                    else
                    {
                        commandName += ch;
                    }
                }
            }

            return new CommandNameAndData(commandName, commandData);
        }
    }
}
