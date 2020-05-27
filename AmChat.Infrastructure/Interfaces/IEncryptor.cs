using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces
{
    public interface IEncryptor
    {
        AesCryptoServiceProvider Aes { get; set; }

        RSACryptoServiceProvider Rsa { get; set; }

        bool UseRsaEncryption { get; set; }

        bool HandshakeComplete { get; set; }

        RSAParameters ExternalPublicKey { get; set; }

        RSAParameters PublicKey { get; set; }


        byte[] Decrypt(byte[] data);

        byte[] Encrypt(byte[] data);
    }
}
