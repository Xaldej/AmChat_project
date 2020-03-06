using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands.ToServer
{
    public class Login : Command
    {
        public Action<string> SendError;

        public override string Name => "Login";

        public override void Execute(IMessengerService messenger, string data)
        {
            var command = "/" + Name.ToLower() + ":" + data;
            try
            {
                messenger.SendCommand(command);
            }
            catch
            {
                var error = "Cannot initialize the user. Try to restart the app";
                SendError(error);
            }
        }
    }
}
