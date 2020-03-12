using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Commands
{
    public static class CommandConverter
    {
        public static CommandMessage GetCommandMessage(string message)
        {
            CommandMessage commandMessage;

            try
            {
                commandMessage = JsonParser<CommandMessage>.JsonToOneObject(message);
            }
            catch
            {
                commandMessage = new CommandMessage("/unknowncommand", string.Empty);
            }

            return commandMessage;
        }

        public static string CreateJsonMessageCommand(string command, string data)
        {
            var commandMessage = new CommandMessage(command, data);

            return JsonParser<CommandMessage>.OneObjectToJson(commandMessage);
        }
    }
}
