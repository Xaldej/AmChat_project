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
    public class CommandHandlerService
    {
        Dictionary<string, ICommandHandler> CommandHandlers { get; set; }

        IMessengerService Messenger { get; set; }

        public Chat ChosenChat { get; set; }

        public ClientEncryptService EncryptServce { get; set; }

        public Action<string> MessageToCurrentChatIsGotten;

        public Action<ChatMessage> MessageToOtherChatIsGotten;

        public Action<string> MessageCorretlySend;

        public Action<Chat> ChatAdded;

        public Action CorrectLoginData;

        public Action<string, bool> ErrorIsGotten;

        public Action IncorrectLoginData;


        public CommandHandlerService(IMessengerService messenger)
        {
            Messenger = messenger;
            
            Messenger.UserChats.CollectionChanged += OnUserChatsChanged;
            Messenger.NewCommand += OnNewMessengerEvent;

            InitializeCommandsHandlers();
        }


        public void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            var newInfoChat = new NewChatInfo(chatName, userLoginsToAdd);
            var newChatInfoJson = JsonParser<NewChatInfo>.OneObjectToJson(newInfoChat);

            var command = new AddOrUpdateChat() { Data = newChatInfoJson };
            var commandJson = JsonParser<AddOrUpdateChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }

        public void AddUsersToChat(Chat chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);
            var newChatInfoJson = JsonParser<NewChatInfo>.OneObjectToJson(newChatInfo);

            var command = new AddOrUpdateChat() { Data = newChatInfoJson };
            var commandJson = JsonParser<AddOrUpdateChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }

        public void CloseConnection()
        {
            try
            {
                var command = new CloseConnection() { Data = string.Empty };
                var commandJson = JsonParser<CloseConnection>.OneObjectToJson(command);

                Messenger.SendMessage(commandJson);
            }
            catch
            {

            }
        }

        public void GetChats()
        {
            var command = new GetChats() { Data = string.Empty };
            var commandJson = JsonParser<GetChats>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);
        }

        public void SendMessageToChat(string message)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = Messenger.User,
                ToChatId = ChosenChat.Id,
                DateAndTime = DateTime.Now,
                Text = message,
            };

            var messageToUserJson = JsonParser<ChatMessage>.OneObjectToJson(messageToChat);

            var command = new SendMessageToChat() { Data = messageToUserJson };
            var commandJson = JsonParser<SendMessageToChat>.OneObjectToJson(command);

            Messenger.SendMessage(commandJson);

            ChosenChat.ChatMessages.Add(messageToChat);
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

        private void OnChatMessagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (!(e.NewItems[0] is ChatMessage message))
            {
                return;
            }



            if (message.FromUser.Equals(Messenger.User))
            {
                MessageCorretlySend(message.Text);
            }
            else
            {

                if (ChosenChat == null || ChosenChat.Id != message.ToChatId)
                {
                    var chatToShowMessage = Messenger.UserChats.Where(c => c.Id == message.ToChatId).FirstOrDefault();

                    MessageToOtherChatIsGotten(message);
                }
                else
                {
                    var messageToShow = message.FromUser.Login + ":\n" + message.Text;
                    MessageToCurrentChatIsGotten(messageToShow);
                }
            }
        }

        private void OnIncorrectLoginData()
        {
            IncorrectLoginData();
        }

        private void OnNewMessengerEvent(string message)
        {
            if(message == string.Empty)
            {
                return;
            }

            var command = JsonParser<Command>.JsonToOneObject(message);

            var handler = CommandHandlers[command.Name];

            handler.Execute(Messenger, command.Data);
        }

        private void OnUserChatsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is Chat newChat))
            {
                return;
            }

            newChat.ChatMessages.CollectionChanged += OnChatMessagesChanged;
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
