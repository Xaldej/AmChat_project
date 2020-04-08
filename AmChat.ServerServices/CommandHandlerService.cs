using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.ServerServices.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class CommandHandlerService
    {
        Dictionary<string, ICommandHandler> CommandHandlers { get; set; }

        IMessengerService Messenger { get; set; }

        public Action<IMessengerService> ClientDisconnected;

        public CommandHandlerService(IMessengerService messenger)
        {
            Messenger = messenger;

            InitializeCommandHandlers();
        }


        public void ProcessMessage(string message)
        {
            if (message == string.Empty)
            {
                return;
            }

            Command command;
            try
            {
                command = JsonParser<Command>.JsonToOneObject(message);
            }
            catch
            {
                return;
            }

            CommandHandlers.TryGetValue(command.Name, out ICommandHandler handler);

            if (handler == null)
            {
                var error = new ServerError()
                {
                    Data = "Unknown command",
                };
                var errorJson = JsonParser<ServerError>.OneObjectToJson(error);

                Messenger.SendMessage(errorJson);

                return;
            }

            handler.Execute(Messenger, command.Data);
        }


        private void DisconnectClient(IMessengerService messenger)
        {
            ClientDisconnected(messenger);
        }

        private void InitializeCommandHandlers()
        {
            CommandHandlers = new Dictionary<string, ICommandHandler>();

            var closeConnectionHandler = new CloseConnectionHandler();
            closeConnectionHandler.ConnectionIsClosed += DisconnectClient;

            CommandHandlers.Add(nameof(AddOrUpdateChat).ToLower(),      new AddOrUpdateChatHandler());
            CommandHandlers.Add(nameof(ClientPublicKey).ToLower(),      new ClientPublicKeyHandler());
            CommandHandlers.Add(nameof(CloseConnection).ToLower(),      closeConnectionHandler);
            CommandHandlers.Add(nameof(GetChats).ToLower(),             new GetChatsHandler());
            CommandHandlers.Add(nameof(GetKey).ToLower(),               new GetKeyHandler());
            CommandHandlers.Add(nameof(Login).ToLower(),                new LoginHandler());
            CommandHandlers.Add(nameof(SendMessageToChat).ToLower(),    new SendMessageToChatHandler());
        }

    }
}
