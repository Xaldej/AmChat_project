﻿using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using AmChat.Server.Commands;
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

        public TcpClient TcpClient { get; set; }

        NetworkStream Stream { get; set; }

        public List<Command> Commands { get; }

        public Action<MessageToUser> NewMwssageForCertainUserIsGotten;

        public Action<ServerMessenger> ClientDisconnected;


        ServerMessenger()
        {

        }

        public ServerMessenger(TcpClient tcpClient, List<ServerMessenger> connectedClients)
        {
            TcpClient = tcpClient;

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
                        ClientDisconnected(this);
                        break;
                    }

                    string message = builder.ToString();

                    ProcessMessage(message);
                }
            }
        }

        private void ProcessMessage(string message)
        {
            var commandMessage = CommandConverter.GetCommandMessage(message);

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

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }

        public void SendMessageToOtherUser(MessageToUser message)
        {
            var messageToUser = JsonParser<MessageToUser>.OneObjectToJson(message);
            var command = CommandConverter.CreateJsonMessageCommand("/messagefromcontact", messageToUser);
            SendMessage(command);
        }
    }
}
