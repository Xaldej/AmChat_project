using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AmChat.ClientServices
{
    public class ClientMessengerService : IMessengerService
    {
        public ICommandHandlerService CommandHandler { get; set; }

        public IEncryptor Encryptor { get; set; }

        public UserInfo User { get; set; }

        public ObservableCollection<Chat> UserChats { get; set; }


        private TcpClient TcpClient { get; set; }

        private NetworkStream Stream { get; set; }


        public ClientMessengerService(TcpClient tcpClient)
        {
            TcpClient = tcpClient;

            Encryptor = new Encryptor();

            User = new UserInfo();

            UserChats = new ObservableCollection<Chat>();
        }


        public void ListenMessages()
        {
            try
            {
                using (Stream = TcpClient.GetStream())
                {
                    while (true)
                    {
                        ListenForNewMessages();
                    }
                }
            }
            catch
            {
                string errorMessage = "Connection lost. Check your internet connection and try to restart the app";
                var message = new ServerError() { Data = errorMessage };
                var messageJson = JsonParser<ServerError>.OneObjectToJson(message);

                CommandHandler.ProcessMessage(this, messageJson);
            }
           
        }

        public void SendMessage(string message)
        {
            byte[] data;

            var messageInBytes = Encoding.Unicode.GetBytes(message);

            data = Encryptor.Encrypt(messageInBytes);

            Stream.Write(data, 0, data.Length);
        }


        private void ListenForNewMessages()
        {
            byte[] data = new byte[TcpClient.ReceiveBufferSize];
            StringBuilder builder = new StringBuilder();
            

            var bytesAmount = Stream.Read(data, 0, data.Length);

            var cutData = new byte[bytesAmount];
            Array.Copy(data, cutData, bytesAmount);

            var decryptedData = Encryptor.Decrypt(cutData);

            builder.Append(Encoding.Unicode.GetString(decryptedData, 0, decryptedData.Length));

            var message = builder.ToString();

            CommandHandler.ProcessMessage(this, message);
        }
    }
}
