﻿using AmChat.ClientServices.Commands;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
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
        public UserInfo User { get; set; }

        public ObservableCollection<Chat> UserChats { get; set; }

        public TcpClient TcpClient { get; set; }

        public List<Command> Commands { get; }

        NetworkStream Stream { get; set; }

        public Chat ChosenChat { get; set; }

        public Action<Chat> ChatAdded;

        public Action<string> MessageToCurrentChatIsGotten;

        public Action<ChatMessage> MessageToOtherChatIsGotten;

        public Action<string, bool> ErrorIsGotten;

        public Action<string> MessageCorretlySend;

        public Action<Guid> NewUnreadNotification;


        public ClientMessengerService()
        {
            User = new UserInfo();

            UserChats = new ObservableCollection<Chat>();
            UserChats.CollectionChanged += AddChatToContactList;

            Commands = new List<Command>();
            InitializeCommands();
        }

        private void AddChatToContactList(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(e.NewItems[0] is Chat newChat))
            {
                return;
            }

            newChat.ChatMessages.CollectionChanged += ShowNewMessage;
            ChatAdded(newChat);
        }

        public void AddUsersToChat(Chat chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);
            var newChatInfoJsont = JsonParser<NewChatInfo>.OneObjectToJson(newChatInfo);
            var command = CommandConverter.CreateJsonMessageCommand("/addorupdatechat", newChatInfoJsont);
            SendMessage(command);
        }

        public void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            var newInfoChat = new NewChatInfo(chatName, userLoginsToAdd);
            var newChatInfoJson = JsonParser<NewChatInfo>.OneObjectToJson(newInfoChat);
            var command = CommandConverter.CreateJsonMessageCommand("/addorupdatechat", newChatInfoJson);
            SendMessage(command);
        }

        public void ListenMessages()
        {
            byte[] data = new byte[TcpClient.ReceiveBufferSize];
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
                string errorMessage = "Connection lost. Check your internet connection and try to restart the app";
                ErrorIsGotten(errorMessage, true);
            }

            var message = builder.ToString();

            ProcessMessage(message);

        }

        public void Login()
        {
            var command = CommandConverter.CreateJsonMessageCommand("/login", User.Login);
            SendMessage(command);
        }

        public void Process(TcpSettings tcpSettings)
        {
            ConnectToServer(tcpSettings);

            using (Stream = TcpClient.GetStream())
            {
                while (true)
                {
                    ListenMessages();
                }
            }
        }

        public void SendMessageToChat(string message)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = User,
                ToChatId = ChosenChat.Id,
                Text = message,
            };

            var messageToUserJson = JsonParser<ChatMessage>.OneObjectToJson(messageToChat);
            var command = CommandConverter.CreateJsonMessageCommand("/sendmessagetochat", messageToUserJson);

            SendMessage(command);

            ChosenChat.ChatMessages.Add(messageToChat);
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }


        private void InitializeCommands()
        {
            var serverError = new ServerError();
            serverError.SendError += ShowError;

            var unreadMessagesInChat = new UnreadMessagesInChat();
            unreadMessagesInChat.NewUnreadNotification += OnNewUnreadNotification;


            Commands.Add(new ChatIsAdded());
            Commands.Add(new CorrectContactList());
            Commands.Add(new CorrectLogin());
            Commands.Add(new MessageToCertainChat());
            Commands.Add(serverError);
            Commands.Add(unreadMessagesInChat);
        }

        private void OnNewUnreadNotification(Guid chatId)
        {
            NewUnreadNotification(chatId);
        }

        private void ConnectToServer(TcpSettings tcpSettings)
        {
            TcpClient = new TcpClient();

            try
            {
                TcpClient.Connect(tcpSettings.EndPoint);
            }
            catch
            {
                var error = "Cannot connect to server. Check your interner connection and restart the app";
                ErrorIsGotten(error, true);
            }
        }

        private void ShowError(string errorText)
        {
            ErrorIsGotten(errorText, false);
        }


        private void ShowNewMessage(object sender, NotifyCollectionChangedEventArgs e)
        {

            if (!(e.NewItems[0] is ChatMessage message))
            {
                return;
            }

            

            if(message.FromUser.Equals(User))
            {
                MessageCorretlySend(message.Text);
            }
            else
            {
                
                if (ChosenChat == null || ChosenChat.Id != message.ToChatId)
                {
                    var chatToShowMessage = UserChats.Where(c => c.Id == message.ToChatId).FirstOrDefault();

                    MessageToOtherChatIsGotten(message);
                }
                else
                {
                    var messageToShow = message.FromUser.Login + ":\n" + message.Text;
                    MessageToCurrentChatIsGotten(messageToShow);
                }
            }
        }

        private void ProcessMessage(string message)
        {
            var commandMessage = CommandConverter.GetCommandMessage(message);

            var commandsToExecute = Commands.Where(c => c.CheckIsCalled(commandMessage.CommandName));

            foreach (var command in commandsToExecute)
            {
                command.Execute(this, commandMessage.CommandData);
            }
        }

        public void CloseConnection()
        {
            var command = CommandConverter.CreateJsonMessageCommand("/closeconnection", string.Empty);
            SendMessage(command);
        }
    }
}
