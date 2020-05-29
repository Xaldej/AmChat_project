using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ChatHistory
    {
        public Guid ChatId { get; set; }

        public IEnumerable<ChatMessage> ChatMessages { get; set; }


        public ChatHistory()
        {

        }

        public ChatHistory(Guid chatId, IEnumerable<ChatMessage> chatMessages)
        {
            ChatId = chatId;

            chatMessages = ChatMessages;
        }
    }
}
