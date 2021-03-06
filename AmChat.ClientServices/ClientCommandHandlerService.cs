﻿using AmChat.ClientServices.CommandHandlers;
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
        public ICollection<MessageToProcess> MessagesToProcess { get; set; }


        private Dictionary<string, ICommandHandler> CommandHandlers { get; set; }


        public Action<ClientChat> ChatAdded;

        public Action CorrectLoginData;

        public Action<string, bool> ErrorIsGotten;

        public Action IncorrectLoginData;

        public Action<ChatMessage, ChatInfo> NewMessageInChat;


        public ClientCommandHandlerService()
        {
            var messagesToProcess = new ObservableCollection<MessageToProcess>();
            messagesToProcess.CollectionChanged += OnNewMessageToProcess;

            MessagesToProcess = messagesToProcess;

            InitializeCommandsHandlers();
        }


        public void ProcessMessage(MessageToProcess message)
        {
            if(message.Message == string.Empty)
            {
                return;
            }

            try
            {
                var command = JsonParser<BaseCommand>.JsonToOneObject(message.Message);

                var handler = CommandHandlers[command.Name];

                handler.Execute(message.Messenger, command.Data);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);
            }
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

        private void OnChatMessagesChanged(ChatMessage message, ChatInfo chat)
        {
            NewMessageInChat(message, chat);
        }

        private void OnIncorrectLoginData()
        {
            IncorrectLoginData();
        }

        private void OnNewMessageToProcess(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add)
            {
                return;
            }

            if (!(e.NewItems[0] is MessageToProcess message))
            {
                return;
            }

            ProcessMessage(message);
            MessagesToProcess.Remove(message);
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
