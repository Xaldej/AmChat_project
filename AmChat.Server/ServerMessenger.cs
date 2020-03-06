using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using AmChat.Server.Commands.FromClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace AlexeyMelentyevProject_ChatServer
{
    public class ServerMessenger : IMessengerService
    {
        public UserInfo User { get; set; }

        public List<UserInfo> UserContacts { get; set; }

        public List<ServerMessenger> ConnectedClients { get; set; }

        public TcpClient TcpClient { get; set; }

        NetworkStream Stream { get; set; }

        public List<Command> Commands { get; }

        public Action<MessageToUser> NewMwssageForCertainUserIsGotten;


        ServerMessenger()
        {

        }

        public ServerMessenger(TcpClient tcpClient, List<ServerMessenger> connectedClients)
        {
            TcpClient = tcpClient;
            ConnectedClients = connectedClients;

            User = new UserInfo();

            UserContacts = new List<UserInfo>();

            Commands = new List<Command>();

            InitializeCommands();
           
        }

        private void InitializeCommands()
        {
            var sendMessageToUser = new SendMessageToUser();
            sendMessageToUser.MessageToUserIsGotten += SendMessageToContact;


            Commands.Add(new AddContact());
            Commands.Add(new GetConactList());
            Commands.Add(new Login());
            Commands.Add(sendMessageToUser);
        }

        private void SendMessageToContact(MessageToUser messageToSent)
        {
            NewMwssageForCertainUserIsGotten(messageToSent);
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
                        var client = ConnectedClients.Where(m => m.Equals(this)).FirstOrDefault();
                        ConnectedClients.Remove(client);

                        break;
                    }

                    string message = builder.ToString();

                    if (CommandIdentifier.IsMessageACommand(message))
                    {
                        ExecuteCommands(message);
                    }
                    else
                    {
                        //SendMessage(message, new Guid());
                    }
                }
            }
        }

        private void ExecuteCommands(string message)
        {
            var commandAndData = CommandIdentifier.GetCommandAndDataFromMessage(message);

            var commandsToExecute = Commands.Where(c => c.CheckIsCalled(commandAndData.Command));

            if (commandsToExecute.Count() == 0)
            {
                SendCommand("/servererror:Unknown command" + commandAndData.Command);
                return;
            }

            foreach (var command in commandsToExecute)
            {
                command.Execute(this, commandAndData.Data);
            }
        }

        public void SendCommand(string command)
        {
            byte[] data = new byte[TcpClient.ReceiveBufferSize];
            data = Encoding.Unicode.GetBytes(command);

            Stream.Write(data, 0, data.Length);
        }

        public void SendMessage(MessageToUser message)
        {
            byte[] data = new byte[TcpClient.ReceiveBufferSize];
            data = Encoding.Unicode.GetBytes(message.Text);

            Stream.Write(data, 0, data.Length);
        }
    }
}
