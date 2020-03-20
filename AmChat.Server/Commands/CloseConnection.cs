﻿using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server.Commands
{
    public class CloseConnection : Command
    {
        public Action<IMessengerService> ConnectionIsClosed;

        public override string Name => "CloseConnection";

        public override void Execute(IMessengerService messenger, string data)
        {
            ConnectionIsClosed(messenger);
        }
    }
}