using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

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
            var history = new List<ChatHistoryMessage>();

            foreach (var message in chat.ChatMessages)
            {
                string messageToShow;
                bool isMyMessage = message.FromUser.Equals(messenger.User);
                if(isMyMessage)
                {
                    messageToShow = message.Text;
                }
                else
                {
                    messageToShow = message.FromUser.Login + ":\n" + message.Text; ;
                }
                var historyMessage = new ChatHistoryMessage(isMyMessage, messageToShow);

                history.Add(historyMessage);
            }

            return history;
        }
    }
}
