using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class GetKeyHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {   
            var rsaXml = messenger.Encryptor.Rsa.ToXmlString(false);

            var command = new ServerPublicKey() { Data = rsaXml };

            var commandJson = JsonParser<ServerPublicKey>.OneObjectToJson(command);

            messenger.SendMessage(commandJson);
        }
    }
}
