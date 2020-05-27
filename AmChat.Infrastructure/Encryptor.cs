using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class Encryptor : IEncryptor
    {
        public AesCryptoServiceProvider Aes { get; set; }

        public RSACryptoServiceProvider Rsa { get; set; }

        public bool UseRsaEncryption { get; set; }

        public bool HandshakeComplete { get; set; }
        
        public RSAParameters ExternalPublicKey { get; set; }

        public RSAParameters PublicKey { get; set; }

        private RSAParameters PrivateKey { get; set; }

        private int RsaKeySize => 2048;


        public Encryptor()
        {
            Aes = new AesCryptoServiceProvider();

            Rsa = new RSACryptoServiceProvider(RsaKeySize);
            Rsa.PersistKeyInCsp = false;

            UseRsaEncryption = false;
            HandshakeComplete = false;

            GetOrGenerateRSAKeys();
            GenerateAesKey();
        }


        public byte[] Decrypt(byte[] data)
        {
            byte[] decryptedData;

            if (HandshakeComplete)
            {
                decryptedData = DecryptAes(data);
            }
            else
            {
                if (UseRsaEncryption)
                {
                    decryptedData = DecryptRsa(data);
                }
                else
                {
                    decryptedData = data;
                }
            }

            return decryptedData;
        }

        public byte[] Encrypt(byte[] data)
        {
            byte[] encryptedData;

            if (HandshakeComplete)
            {
                encryptedData = EncryptAes(data);
            }
            else
            {
                if (UseRsaEncryption)
                {
                    encryptedData = EncryptRsa(data);
                }
                else
                {
                    encryptedData = data;
                }
            }

            return encryptedData;
        }


        private byte[] DecryptAes(byte[] data)
        {
            byte[] decryptedData;

            using (var decryptor = Aes.CreateDecryptor())
            {
                decryptedData = decryptor.TransformFinalBlock(data, 0, data.Length);
            }

            return decryptedData;
        }

        private byte[] EncryptAes(byte[] data)
        {
            byte[] encryptedData;

            using (var encrytpor = Aes.CreateEncryptor())
            {   
                encryptedData = encrytpor.TransformFinalBlock(data, 0, data.Length);
            }

            return encryptedData;
        }


        private byte[] DecryptRsa(byte[] input)
        {
            byte[] output;

            using (var rsa = new RSACryptoServiceProvider(RsaKeySize))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(PrivateKey);
                output = rsa.Decrypt(input, true);
            }

            return output;
        }

        private byte[] EncryptRsa(byte[] input)
        {
            byte[] output;

            using (var rsa = new RSACryptoServiceProvider(RsaKeySize))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(ExternalPublicKey);
                output = rsa.Encrypt(input, true);
           
            }

            return output;
        }


        private void GenerateAesKey()
        {
            Aes.GenerateKey();
            Aes.GenerateIV();
        }

        private void GenerateRSAKeys()
        {
            PublicKey = Rsa.ExportParameters(false);
            PrivateKey = Rsa.ExportParameters(true);
        }

        private void GetRSAKeysFromFile()
        {
            using (var streamReader = new StreamReader("rsakeys.txt"))
            {
                var rsaXml = streamReader.ReadToEnd();

                Rsa.FromXmlString(rsaXml);

                GenerateRSAKeys();
            }
        }

        private void GetOrGenerateRSAKeys()
        {
            if(File.Exists("rsakeys.txt"))
            {
                GetRSAKeysFromFile();
            }
            else
            {
                GenerateRSAKeys();
                SaveRSAKeysToFile();
            }
        }

        private void SaveRSAKeysToFile()
        {
            using (var streamWriter = new StreamWriter("rsakeys.txt"))
            {
                var rsaXml = Rsa.ToXmlString(true);

                streamWriter.Write(rsaXml);
            }
        }
    }
}
