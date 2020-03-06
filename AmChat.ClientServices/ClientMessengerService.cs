using AmChat.ClientServices.Commands.FromServer;
using AmChat.ClientServices.Commands.ToServer;
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

        public List<Command> Commands { get; }

        NetworkStream Stream { get; set; }

        public UserInfo ChosenUser { get; set; }

        public Action<string> MessageIsGotten;

        public Action<string, bool> ErrorIsGotten;

        public Action<List<UserInfo>> ContactsAreUpdated;


        public ClientMessengerService()
        {
        }

        public ClientMessengerService(string userLogin)
        {
            User = new UserInfo()
            {
                Login = userLogin
            };

            UserContacts = new List<UserInfo>();

            Commands = new List<Command>();
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            var correctAddingContact = new CorrectAddingContact();
            correctAddingContact.ContactListIsUpdated += UpdateContacts;

            var correctContactList = new CorrectContactList();
            correctContactList.ContactListIsUpdated += UpdateContacts;

            var serverError = new ServerError();
            serverError.SendError += ShowError;

            var addContact = new AddContact();
            addContact.SendError = ShowError;

            var connect = new Connect();
            connect.SendError += ShowError;

            var getContactList = new GetConactList();
            getContactList.SendError = ShowError;

            Commands.Add(correctAddingContact);
            Commands.Add(correctContactList);
            Commands.Add(new CorrectLogin());
            Commands.Add(serverError);

            Commands.Add(addContact);
            Commands.Add(connect);
            Commands.Add(getContactList);
            Commands.Add(new Login());
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
                MessageIsGotten(message);
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

        public void SendMessage(string message)
        {
            var messageToUser = new MessageToUser()
            {
                FromUserId = User.Id,
                ToUserId = ChosenUser.Id,
                Text = message,
            };

            var command = "/sendmessagetouser:" + JsonParser<MessageToUser>.OneObjectToJson(messageToUser);

            byte[] data = Encoding.Unicode.GetBytes(command);
            Stream.Write(data, 0, data.Length);
        }

        public void Process()
        {
            Commands.Where(c => c.CheckIsCalled("/connect")).FirstOrDefault().Execute(this, string.Empty);

            using (Stream = TcpClient.GetStream())
            {
                ExecuteCommands($"/Login:{User.Login}");

                while (true)
                {
                    ListenMessages();
                }
            }
        }

        public void SendCommand(string command)
        {
            byte[] data = Encoding.Unicode.GetBytes(command);
            Stream.Write(data, 0, data.Length);
        }

        private void UpdateContacts()
        {
            ContactsAreUpdated(UserContacts);
        }

        private void ShowError(string errorText)
        {
            ErrorIsGotten(errorText, false);
        }
    }
}
