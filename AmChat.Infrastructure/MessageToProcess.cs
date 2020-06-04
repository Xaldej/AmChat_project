using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class MessageToProcess
    {
        public IMessengerService Messenger { get; set; }

        public string Message { get; set; }

        public MessageToProcess(IMessengerService messenger, string message)
        {
            Messenger = messenger;
            Message = message;
        }
    }
}
