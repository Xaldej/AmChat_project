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
        protected Dictionary<UserInfo, List<ChatHistoryMessage>> UsersChatHistory;

        public ChatHistoryServise()
        {
            UsersChatHistory = new Dictionary<UserInfo, List<ChatHistoryMessage>>();
        }

        public List<ChatHistoryMessage> GetHistory(UserInfo user)
        {
            List<ChatHistoryMessage> history;

            if (UsersChatHistory.ContainsKey(user))
            {
                history = UsersChatHistory[user];

                if (history.Count() == 0)
                {
                    history = GetHistoryFromServer(user);
                }
            }
            else
            {
                history = GetHistoryFromServer(user);

                UsersChatHistory.Add(user, history);
            }

            return history;
        }

        public void SaveHistory(UserInfo user, string message, bool isMyMessage)
        {
            UsersChatHistory[user].Add(new ChatHistoryMessage(isMyMessage, message));
        }

        private List<ChatHistoryMessage> GetHistoryFromServer(UserInfo user)
        {
            //TO DO

            return new List<ChatHistoryMessage>();
        }
    }
}
