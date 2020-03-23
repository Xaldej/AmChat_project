﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces
{
    public interface IMessengerService
    {
        User User { get; set; }

        ObservableCollection<Chat> UserChats { get; set; }

        void ListenMessages();

        void SendMessage(string message);
    }
}
