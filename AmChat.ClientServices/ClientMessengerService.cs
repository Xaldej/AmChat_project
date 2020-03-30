﻿using AmChat.Infrastructure;
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

        TcpClient TcpClient { get; set; }

        NetworkStream Stream { get; set; }

        public Action<string> NewMessage;




        public ClientMessengerService(TcpClient tcpClient)
        {
            TcpClient = tcpClient;

            User = new UserInfo();
            
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
                //string errorMessage = "Connection lost. Check your internet connection and try to restart the app";
                //ErrorIsGotten(errorMessage, true);
            }

            var message = builder.ToString();

            NewMessage(message);
        }


        public void Process()
        {
            using (Stream = TcpClient.GetStream())
            {
                while (true)
                {
                    ListenMessages();
                }
            }
        }

       

        public void SendMessage(string message)
        {
            byte[] data = Encoding.Unicode.GetBytes(message);
            Stream.Write(data, 0, data.Length);
        }
        
    }
}
