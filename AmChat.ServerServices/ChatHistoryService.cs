using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces.ServerServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public class ChatHistoryService : IChatHistoryService
    {
        public ChatHistoryService()
        {

        }


        public ObservableCollection<ChatMessage> GetChatHistory(Chat chat)
        {
            var dbChatMessage = new List<DBChatMessage>();

            using (var context = new AmChatContext())
            {
                dbChatMessage = context.ChatMessages
                                        .Where(cm => cm.ChatId == chat.Id)
                                        .OrderBy(cm=>cm.DateAndTime)
                                        .ToList();
            }

            var lastNMessages = GetLastNMessages(dbChatMessage, 100);

            var chatHistory = DbMessagesToMessages(lastNMessages);

            var chatMessages = new ObservableCollection<ChatMessage>(chatHistory);

            return chatMessages;
        }

        public void SaveChatHistory(Chat chat)
        {

            var newMessages = GetNewMessages(chat, chat.ChatMessages);

            var newDbChatMessages = MessagesToDbMessages(newMessages);

            using (var context = new AmChatContext())
            {
                context.ChatMessages.AddRange(newDbChatMessages);
                context.SaveChanges();
            }
        }


        private List<ChatMessage> DbMessagesToMessages(List<DBChatMessage> dbMessages)
        {
            var messages = new List<ChatMessage>();

            foreach (var dbMessage in dbMessages)
            {
                var message = new ChatMessage()
                {
                    FromUser = GetUserFromDb(dbMessage.FromUserId),
                    ToChatId = dbMessage.ChatId,
                    DateAndTime = dbMessage.DateAndTime,
                    Text = dbMessage.Text,
                };
                messages.Add(message);
            }

            return messages;
        }

        private List<DBChatMessage> GetLastNMessages(List<DBChatMessage> messages, int n)
        {
            var lastNMessages = new List<DBChatMessage>();

            lastNMessages = messages.Skip(Math.Max(0, messages.Count() - n)).ToList();

            return lastNMessages;
        }

        private ICollection<ChatMessage>GetNewMessages(Chat chat, ICollection<ChatMessage> messages)
        {
            var newMessages = messages;
            var historyMessages = GetChatHistory(chat);

            foreach (var hm in historyMessages)
            {
                newMessages.Remove(hm);
            }

            return newMessages;
        }

        private UserInfo GetUserFromDb(Guid id)
        {
            var dbUser = new DBUser();

            using (var context = new AmChatContext())
            {
                dbUser = context.Users.Where(u => u.Id == id).FirstOrDefault();
            }

            return new UserInfo()
            {
                Id = dbUser.Id,
                Login = dbUser.Login,
            };
        }

        private List<DBChatMessage> MessagesToDbMessages(ICollection<ChatMessage> messages)
        {
            var chatMessages = new List<DBChatMessage>();

            foreach (var message in messages)
            {
                var chatMessage = new DBChatMessage()
                {
                    Id = Guid.NewGuid(),
                    ChatId = message.ToChatId,
                    FromUserId = message.FromUser.Id,
                    DateAndTime = message.DateAndTime,
                    Text = message.Text,
                };

                chatMessages.Add(chatMessage);
            }

            return chatMessages;
        }
    }
}
