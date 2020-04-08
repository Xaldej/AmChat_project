using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class ClientPublicKeyHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            RSAParameters clientKey;

            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(data);
                clientKey = rsa.ExportParameters(false);
            }

            messenger.Encryptor.ExternalPublicKey = clientKey;
            messenger.Encryptor.UseRsaEncryption = true;

            messenger.Encryptor.GenerateAesKey();

            SendAesKey(messenger);

            SendAesVecotr(messenger);

            messenger.Encryptor.HandshakeComplete = true;
        }


        private static void SendAesKey(IMessengerService messenger)
        {
            var aesKey = messenger.Encryptor.Aes.Key;
            var aesKeyJson = JsonParser<byte[]>.OneObjectToJson(aesKey);

            var command = new AesKey() { Data = aesKeyJson };
            var commandJson = JsonParser<AesKey>.OneObjectToJson(command);

            messenger.SendMessage(commandJson);
        }

        private void SendAesVecotr(IMessengerService messenger)
        {
            var aesVector = messenger.Encryptor.Aes.IV;
            var aesVectorJson = JsonParser<byte[]>.OneObjectToJson(aesVector);

            var command = new AesVector() { Data = aesVectorJson };
            var commandJson = JsonParser<AesVector>.OneObjectToJson(command);

            messenger.SendMessage(commandJson);
        }
    }
}
