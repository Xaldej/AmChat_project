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

        public ObservableCollection<Chat> UserChats { get; set; }

        public TcpClient TcpClient { get; set; }

        NetworkStream Stream { get; set; }

        public Dictionary<string, ICommandHandler> CommandHandlers { get; }

        public Action<IMessengerService> ClientDisconnected;


        ServerMessengerService()
        {

        }

        public ServerMessengerService(TcpClient tcpClient)
        {
            TcpClient = tcpClient;

            User = new UserInfo();

            UserChats = new ObservableCollection<Chat>();

            CommandHandlers = new Dictionary<string, ICommandHandler>();

            InitializeCommandHandlers();

        }

        public void ListenMessages()
        {
            using (Stream = TcpClient.GetStream())
            {
                byte[] data = new byte[TcpClient.ReceiveBufferSize];
                while (true)
                {
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    try
                    {
                        do
                        {
                            bytes = Stream.Read(data, 0, data.Length);
                            builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                        }
                        while (Stream.DataAvailable);
                    }
                    catch
                    {
                        ClientDisconnected(this);
                        break;
                    }

                    string message = builder.ToString();

                    ProcessMessage(message);
                }
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        public void SendMessageToExistingChat(ChatMessage message)
        {
            var messageToUserJson = JsonParser<ChatMessage>.OneObjectToJson(message);

            var command = new MessageToCertainChat() { Data = messageToUserJson };
            var commandJson = JsonParser<MessageToCertainChat>.OneObjectToJson(command);
            
            SendMessage(commandJson);
        }


        private void InitializeCommandHandlers()
        {
            var closeConnectionHandler = new CloseConnectionHandler();
            closeConnectionHandler.ConnectionIsClosed += DisconnectClient;

            CommandHandlers.Add(nameof(AddOrUpdateChat).ToLower(), new AddOrUpdateChatHandler());
            CommandHandlers.Add(nameof(CloseConnection).ToLower(), closeConnectionHandler);
            CommandHandlers.Add(nameof(GetChats).ToLower(), new GetChatsHandler());
            CommandHandlers.Add(nameof(Login).ToLower(), new LoginHandler());
            CommandHandlers.Add(nameof(SendMessageToChat).ToLower(), new SendMessageToChatHandler());
        }

        private void DisconnectClient(IMessengerService messenger)
        {
            ClientDisconnected(messenger);
        }

        private void ProcessMessage(string message)
        {
            var command = new Command();
            try
            {
                command = JsonParser<Command>.JsonToOneObject(message);
            }
            catch
            {
                //TO DO: log errors
                return;
            }

            if (command == null)
            {
                //TO DO: log errors
                return;
            }

            ICommandHandler handler;
            CommandHandlers.TryGetValue(command.Name, out handler);

            if (handler == null)
            {
                var error = new ServerError() { Data = "Unknown command" };
                var errorJson = JsonParser<ServerError>.OneObjectToJson(error);

                SendMessage(errorJson);

                return;
            }

            handler.Execute(this, command.Data);
        }
    }
}
