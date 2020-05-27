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
    public class ServerCommandHandlerService : ICommandHandlerService
    {
        Dictionary<string, ICommandHandler> CommandHandlers { get; set; }

        public Action<IMessengerService> ClientDisconnected;


        public ServerCommandHandlerService()
        {
            InitializeCommandHandlers();
        }


        public void ProcessMessage(IMessengerService messenger, string message)
        {
            if (message == string.Empty)
            {
                return;
            }

            BaseCommand command;
            try
            {
                command = JsonParser<BaseCommand>.JsonToOneObject(message);
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

                messenger.SendMessage(errorJson);

                return;
            }

            handler.Execute(messenger, command.Data);
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
