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
        ObservableCollection<ChatMessage> GetChatHistory(Chat chat);

        void SaveChatHistory(Chat chat);
    }
}
