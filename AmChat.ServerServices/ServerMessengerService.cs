using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.ServerServices.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ServerMessengerService : IMessengerService
    {
        public UserInfo User { get; set; }

        public ObservableCollection<ChatInfo> UserChats { get; set; }

        public ICommandHandlerService CommandHandler { get; set; }

        public IEncryptor Encryptor { get; set; }


        private TcpClient TcpClient { get; set; }

        private NetworkStream Stream { get; set; }


        public ServerMessengerService(TcpClient tcpClient)
        {
            TcpClient = tcpClient;

            User = new UserInfo();

            Encryptor = new Encryptor();

            UserChats = new ObservableCollection<ChatInfo>();
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
                var message = new CloseConnection() { Data = "ConnectionLost"};
                var messageJson = JsonParser<CloseConnection>.OneObjectToJson(message);
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
