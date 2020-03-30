using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AmChat.ServerServices.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SendMessageToExistingChat(ChatMessage message)
        {
            var messageToUserJson = JsonParser<ChatMessage>.OneObjectToJson(message);

            var command = new MessageToCertainChat() { Data = messageToUserJson };
            var commandJson = JsonParser<MessageToCertainChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }


        private void InitializeCommandHandlers()
        {
            CommandHandlers = new Dictionary<string, ICommandHandler>();

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
    }
}
