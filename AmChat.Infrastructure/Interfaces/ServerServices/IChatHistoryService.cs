using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces.ServerServices
{
    public interface IChatHistoryService
    {
        void AddNewMessageToChatHistory(ChatMessage message);

        IEnumerable<ChatMessage> GetChatHistory(Guid chatId);
    }
}
