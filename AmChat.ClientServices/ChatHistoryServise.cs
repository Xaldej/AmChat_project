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

        public List<ChatHistoryMessage> GetHistory(UserChat chat, ClientMessengerService messenger)
        {
            if (chat.ChatMessages.Count() == 0)
            {
                GetHistoryFromServer(chat);
            }
            var history = new List<ChatHistoryMessage>();

            foreach (var message in chat.ChatMessages)
            {
                bool isMyMessage = message.FromUser.Equals(messenger.User);
                var historyMessage = new ChatHistoryMessage(isMyMessage, message.Text);

                history.Add(historyMessage);
            }

            return history;
        }

        private void GetHistoryFromServer(UserChat user)
        {
            //TO DO
        }
    }
}
