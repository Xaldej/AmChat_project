using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    class ServerPublicKeyHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            SetExternalPublickKey(messenger, data);
            SendClientPublicKey(messenger);
        }

        private static void SendClientPublicKey(IMessengerService messenger)
        {
            var rsaXml = messenger.Encryptor.Rsa.ToXmlString(false);

            var commandJson = CommandExtentions.GetCommandJson<ClientPublicKey, string>(rsaXml, true);

            messenger.SendMessage(commandJson);

            messenger.Encryptor.UseRsaEncryption = true;
        }

        private static void SetExternalPublickKey(IMessengerService messenger, string data)
        {
            RSAParameters serverPublicKey;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(data);
                serverPublicKey = rsa.ExportParameters(false);
            }

            messenger.Encryptor.ExternalPublicKey = serverPublicKey;
            
        }
    }
}
