using AmChat.ClientServices.Commands;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class ClientMessengerService : IMessengerService
    {
        public UserInfo User { get; set; }

        public List<UserInfo> UserContacts { get; set; }

        public TcpClient TcpClient { get; set; }

        public TcpSettings TcpSettings { get; set; }

        public List<Command> Commands { get; }

        NetworkStream Stream { get; set; }

        public UserInfo ChosenUser { get; set; }

        public Action<string> MessageForCurrentContactIsGotten;

        public Action<string, bool> ErrorIsGotten;

        public Action<List<UserInfo>> ContactsReceived;

        public Action<UserInfo> ContactAdded;

        public Action<MessageToUser> MessageFromNewContactIsGotten;

        public Action<MessageToUser> MessageForOtherContactIsGotten;


        ClientMessengerService()
        {
        }

        public ClientMessengerService(TcpSettings tcpSettings)
        {
            TcpSettings = tcpSettings;
            User = new UserInfo();

            UserContacts = new List<UserInfo>();

            Commands = new List<Command>();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            var correctAddingContact = new CorrectAddingContact();
            correctAddingContact.ContactIsGotten += AddOneContact;

            var correctContactList = new CorrectContactList();
            correctContactList.ContactListIsUpdated += UpdateContacts;

            var messageFromContact = new MessageFromContact();
            messageFromContact.NewMessageIsGotten += ShowNewMessage;

            var serverError = new ServerError();
            serverError.SendError += ShowError;

            Commands.Add(correctAddingContact);
            Commands.Add(correctContactList);
            Commands.Add(new CorrectLogin());
            Commands.Add(messageFromContact);
            Commands.Add(serverError);
        }

        private void AddOneContact(UserInfo user)
        {
            ContactAdded(user);
        }

        private void ShowNewMessage(MessageToUser messageToShow)
        {
            if (ChosenUser == null || !ChosenUser.Equals(messageToShow.FromUser)) 
            {
                var userToShowMessage = UserContacts.Where(u => u.Equals(messageToShow.FromUser)).FirstOrDefault();

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

            if (CommandIdentifier.IsMessageACommand(message))
            {
                ExecuteCommands(message);
            }
            else
            {
                MessageForCurrentContactIsGotten(message);
            }
        }

        public void ExecuteCommands(string message)
        {   
            var commandAndData = CommandIdentifier.GetCommandAndDataFromMessage(message);

            var commandsToExecute = Commands.Where(c => c.CheckIsCalled(commandAndData.Command));

            foreach (var command in commandsToExecute)
            {
                command.Execute(this, commandAndData.Data);
            }
        }

        public void SendMessageToUser(string message)
        {
            var messageToUser = new MessageToUser()
            {
                FromUser = User,
                ToUser = ChosenUser,
                Text = message,
            };

            var command = "/sendmessagetouser:" + JsonParser<MessageToUser>.OneObjectToJson(messageToUser);

            SendMessage(command);
        }

        public void Process()
        {
            //Commands.Where(c => c.CheckIsCalled("/connect")).FirstOrDefault().Execute(this, string.Empty);
            ConnectToServer();

            using (Stream = TcpClient.GetStream())
            {
                while (true)
                {
                    ListenMessages();
                }
            }
        }

        public void Login()
        {
            var command = $"/Login:{User.Login}";
            SendMessage(command);
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

        public void SendMessage(string command)
        {
            byte[] data = Encoding.Unicode.GetBytes(command);
            Stream.Write(data, 0, data.Length);
        }

        private void UpdateContacts()
        {
            ContactsReceived(UserContacts);
        }

        private void UpdateContacts(UserInfo user)
        {
            ContactAdded(user);
        }

        private void ShowError(string errorText)
        {
            ErrorIsGotten(errorText, false);
        }

        public void AddContact(string userName)
        {   
            var command = "/addcontact:" + userName;
            SendMessage(command);
        }
    }
}
