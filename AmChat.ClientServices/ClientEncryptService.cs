using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class ClientEncryptService
    {
        public Encryptor Encrypter { get; set; }

        IMessengerService Messenger { get; set; }

        public ClientEncryptService(IMessengerService messenger)
        {
            Messenger = messenger;

            Encrypter = new Encryptor();
        }

        public void GetKeyFromServer()
        {
            var command = new GetKey() { Data = string.Empty };
            var commandJson = JsonParser<GetKey>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }
    }
}
