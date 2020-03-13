﻿using AmChat.ClientServices.Commands;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class ClientMessengerService : IMessengerService
    {
        public UserInfo User { get; set; }

        public ObservableCollection<UserChat> UserChats { get; set; }

        public TcpClient TcpClient { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public List<Command> Commands { get; }

        NetworkStream Stream { get; set; }

        public UserChat ChosenChat { get; set; }

        public Action<string> MessageForCurrentContactIsGotten;

        public Action<string, bool> ErrorIsGotten;

        public Action<ObservableCollection<UserChat>> ContactsReceived;

        public Action<UserChat> ContactAdded;

        public Action<MessageToChat> MessageFromNewContactIsGotten;

        public Action<MessageToChat> MessageForOtherContactIsGotten;


        ClientMessengerService()
        {
        }

        public ClientMessengerService(TcpSettings tcpSettings)
        {
            TcpSettings = tcpSettings;
            User = new UserInfo();

            UserChats = new ObservableCollection<UserChat>();

            Commands = new List<Command>();
            InitializeCommands();
        }

        public void AddContact(string userName)
        {
            var command = CommandConverter.CreateJsonMessageCommand("/addcontact", userName);
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

        public void Process()
        {
            ConnectToServer();

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
            var messageToChat = new MessageToChat()
            {
                FromUser = User,
                ToChat = ChosenChat,
                Text = message,
            };

            var messageToUserJson = JsonParser<MessageToChat>.OneObjectToJson(messageToChat);
            var command = CommandConverter.CreateJsonMessageCommand("/sendmessagetochat", messageToUserJson);

            SendMessage(command);
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }


        private void InitializeCommands()
        {
            var correctAddingContact = new CorrectAddingContact();
            correctAddingContact.ContactIsGotten += AddOneContact;

            var correctContactList = new CorrectContactList();
            correctContactList.ContactListIsUpdated += UpdateContacts;

            var messageFromContact = new MessageToCertainChat();
            messageFromContact.NewMessageIsGotten += ShowNewMessage;

            var serverError = new ServerError();
            serverError.SendError += ShowError;

            Commands.Add(correctAddingContact);
            Commands.Add(correctContactList);
            Commands.Add(new CorrectLogin());
            Commands.Add(messageFromContact);
            Commands.Add(serverError);
        }

        private void AddOneContact(UserChat user)
        {
            ContactAdded(user);
        }

        private void ConnectToServer()
        {
            TcpClient = new TcpClient();

            try
            {
                TcpClient.Connect(TcpSettings.EndPoint);
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

        private void ShowNewMessage(MessageToChat messageToShow)
        {
            // TO FIX
            if (ChosenChat == null || !ChosenChat.Equals(messageToShow.ToChat)) 
            {
                var userToShowMessage = UserChats.Where(u => u.Equals(messageToShow.ToChat)).FirstOrDefault();

                if (userToShowMessage == null)
                {
                    MessageFromNewContactIsGotten(messageToShow);
                }
                else
                {
                    MessageForOtherContactIsGotten(messageToShow);
                }
            }
            else
            {
                MessageForCurrentContactIsGotten(messageToShow.Text);
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

        private void UpdateContacts()
        {
            ContactsReceived(UserChats);
        }
    }
}
