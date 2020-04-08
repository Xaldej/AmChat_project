using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class AesKeyHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            var aesKey = JsonParser<byte[]>.JsonToOneObject(data);

            messenger.Encryptor.Aes.Key = aesKey;
        }
    }
}
