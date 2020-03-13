using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class ChatHistoryServise
    {
        protected Dictionary<UserChat, List<ChatHistoryMessage>> UsersChatHistory;

        public ChatHistoryServise()
        {
            UsersChatHistory = new Dictionary<UserChat, List<ChatHistoryMessage>>();
        }

        public List<ChatHistoryMessage> GetHistory(UserChat chat)
        {
            List<ChatHistoryMessage> history;

            if (UsersChatHistory.ContainsKey(chat))
            {
                history = UsersChatHistory[chat];

                if (history.Count() == 0)
                {
                    history = GetHistoryFromServer(chat);
                }
            }
            else
            {
                history = GetHistoryFromServer(chat);

                UsersChatHistory.Add(chat, history);
            }

            return history;
        }

        public void SaveHistory(UserChat chat, string message, bool isMyMessage)
        {
            if(!UsersChatHistory.ContainsKey(chat))
            {
                UsersChatHistory.Add(chat, new List<ChatHistoryMessage>());
            }

            UsersChatHistory[chat].Add(new ChatHistoryMessage(isMyMessage, message));
        }

        private List<ChatHistoryMessage> GetHistoryFromServer(UserChat user)
        {
            //TO DO

            return new List<ChatHistoryMessage>();
        }
    }
}
