using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ChatHistoryMessage
    {
        public bool IsMyMessage { get; set; }

        public string Message { get; set; }

        public ChatHistoryMessage(bool isMyMessage, string message)
        {
            IsMyMessage = isMyMessage;
            Message = message;
        }
    }
}
