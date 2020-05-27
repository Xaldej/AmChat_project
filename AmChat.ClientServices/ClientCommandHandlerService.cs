using AmChat.ClientServices.CommandHandlers;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class ClientCommandHandlerService : ICommandHandlerService
    {
        private Dictionary<string, ICommandHandler> CommandHandlers { get; set; }

        private readonly IMessengerService messenger;


        public Action<Chat> ChatAdded;

        public Action CorrectLoginData;

        public Action<string, bool> ErrorIsGotten;

        public Action IncorrectLoginData;

        public Action<ChatMessage, Chat> NewMessageInChat;


        public ClientCommandHandlerService(IMessengerService messenger)
        {
            this.messenger = messenger;
            
            this.messenger.UserChats.CollectionChanged += OnUserChatsChanged;

            InitializeCommandsHandlers();
        }


        private void InitializeCommandsHandlers()
        {
            CommandHandlers = new Dictionary<string, ICommandHandler>();

            var correctLoginHandler = new CorrectLoginHandler();
            correctLoginHandler.UserIsLoggedIn += OnUserIsLoggedIn;

            var incorrectLoginHandler = new IncorrectLoginHandler();
            incorrectLoginHandler.IncorrectLoginData += OnIncorrectLoginData;

            var serverErrorHandler = new ServerErrorHandler();
            serverErrorHandler.NewServerError += OnNewServerError;

            CommandHandlers.Add(nameof(AesKey).ToLower(),               new AesKeyHandler());
            CommandHandlers.Add(nameof(AesVector).ToLower(),            new AesVectorHandler());
            CommandHandlers.Add(nameof(ChatIsAdded).ToLower(),          new ChatIsAddedHandler());
            CommandHandlers.Add(nameof(CorrectContactList).ToLower(),   new CorrectContactListHandler());
            CommandHandlers.Add(nameof(CorrectLogin).ToLower(),         correctLoginHandler);
            CommandHandlers.Add(nameof(IncorrectLogin).ToLower(),       incorrectLoginHandler);
            CommandHandlers.Add(nameof(MessageToCertainChat).ToLower(), new MessageToCertainChatHandler());
            CommandHandlers.Add(nameof(ServerPublicKey).ToLower(),      new ServerPublicKeyHandler());
            CommandHandlers.Add(nameof(ServerError).ToLower(),          serverErrorHandler);
        }

        private void OnChatMessagesChanged(ChatMessage message, Chat chat)
        {
            NewMessageInChat(message, chat);
        }

        private void OnIncorrectLoginData()
        {
            IncorrectLoginData();
        }

        public void ProcessMessage(IMessengerService messenger, string message)
        {
            if(message == string.Empty)
            {
                return;
            }

            var command = JsonParser<BaseCommand>.JsonToOneObject(message);

            var handler = CommandHandlers[command.Name];

            handler.Execute(this.messenger, command.Data);
        }

        private void OnUserChatsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is Chat newChat))
            {
                return;
            }

            newChat.NewMessageInChat += OnChatMessagesChanged;
            ChatAdded(newChat);
        }

        private void OnUserIsLoggedIn()
        {
            CorrectLoginData();
        }

        private void OnNewServerError(string errorText)
        {
            bool closeApp = false;

            if(errorText.Contains("Connection lost"))
            {
                closeApp = true;
            }

            ErrorIsGotten(errorText, closeApp);
        }
    }
}
