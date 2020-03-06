using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands.ToServer
{
    public class AddContact : Command
    {
        public Action<string> SendError;

        public override string Name => "AddContact";

        public override void Execute(IMessengerService messenger, string data)
        {
            var command = "/" + Name.ToLower() + ":" + data;
            try
            {
                messenger.SendCommand(command);
            }
            catch
            {
                string error = "Error adding contact. Try again";

                SendError(error);
            }
        }
    }
}
