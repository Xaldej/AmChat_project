using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces.ServerServices;
using AutoMapper;
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

        public void AddNewMessageToChatHistory(ChatMessage message)
        {
            var dbMessage = MessageToDbMessage(message);

            try
            {
                using (var context = new AmChatContext())
                {
                    context.ChatMessages.Add(dbMessage);

                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);
            }

        }

        private DBChatMessage MessageToDbMessage(ChatMessage message)
        {
            return new DBChatMessage()
            {
                Id = Guid.NewGuid(),
                ChatId = message.ToChatId,
                FromUserId = message.FromUser.Id,
                DateAndTime = message.DateAndTime,
                Text = message.Text,
            };
        }

        public IEnumerable<ChatMessage> GetChatHistory(Guid chatId)
        {
            var dbChatMessage = new List<DBChatMessage>();

            var chatHistory = new List<ChatMessage>();
            try
            {
                using (var context = new AmChatContext())
                {
                    dbChatMessage = context.ChatMessages
                                            .Where(cm => cm.ChatId == chatId)
                                            .OrderBy(cm => cm.DateAndTime)
                                            .ToList();
                }

                var lastNMessages = GetLastNMessages(dbChatMessage, 100);

                chatHistory = DbMessagesToMessages(lastNMessages);

            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);
            }

            return chatHistory;
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
            return messages.Skip(Math.Max(0, messages.Count() - n)).ToList();
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
    }
}
