using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using AmChat.Server.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AlexeyMelentyevProject_ChatServer
{
    public class ServerMessenger : IMessengerService
    {
        public UserInfo User { get; set; }

        public ObservableCollection<UserChat> UserChats { get; set; }

        public TcpClient TcpClient { get; set; }

        NetworkStream Stream { get; set; }

        public List<Command> Commands { get; }

        public Action<ServerMessenger> ClientDisconnected;

        public Action<UserChat> NewChatIsCreated;


        ServerMessenger()
        {

        }

        public ServerMessenger(TcpClient tcpClient, List<ServerMessenger> connectedClients)
        {
            TcpClient = tcpClient;

            User = new UserInfo();

            UserChats = new ObservableCollection<UserChat>();

            Commands = new List<Command>();

            InitializeCommands();
           
        }

        public void ListenMessages()
        {
            using (Stream = TcpClient.GetStream())
            {
                byte[] data = new byte[TcpClient.ReceiveBufferSize];
                while (true)
                {
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
                        ClientDisconnected(this);
                        break;
                    }

                    string message = builder.ToString();

                    ProcessMessage(message);
                }
            }
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        public void SendMessageToExistingChat(MessageToChat message)
        {
            var messageToUser = JsonParser<MessageToChat>.OneObjectToJson(message);
            var command = CommandConverter.CreateJsonMessageCommand("/messagetocertainchat", messageToUser);
            SendMessage(command);
        }


        private void InitializeCommands()
        {
            var addContact = new AddContact();
            addContact.NewChatIsCreated += OnNewChatIsCreated;

            Commands.Add(addContact);
            Commands.Add(new GetChats());
            Commands.Add(new Login());
            Commands.Add(new SendMessageToChat());
        }

        private void OnNewChatIsCreated(UserChat chat)
        {
            NewChatIsCreated(chat);
        }

        private void ProcessMessage(string message)
        {
            CommandMessage commandMessage;
            try
            {
                commandMessage = CommandConverter.GetCommandMessage(message);
            }
            catch
            {
                //TO DO: log errors
                return;
            }
            

            var commandsToExecute = Commands.Where(c => c.CheckIsCalled(commandMessage.CommandName));

            if (commandsToExecute.Count() == 0)
            {
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Unknown command");
                SendMessage(error);
                return;
            }

            foreach (var command in commandsToExecute)
            {
                command.Execute(this, commandMessage.CommandData);
            }
        }
    }
}
