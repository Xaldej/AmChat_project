using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using AmChat.Infrastructure.Interfaces.ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class SendMessageToChatHandler : ICommandHandler
    {
        private readonly IChatHistoryService chatHistoryService;


        public SendMessageToChatHandler()
        {
            chatHistoryService = new ChatHistoryService();
        }


        public void Execute(IMessengerService messenger, string data)
        {
            var messageToChat = JsonParser<ChatMessage>.JsonToOneObject(data);

            var chat = messenger.UserChats.Where(c => c.Id == messageToChat.ToChatId).FirstOrDefault();

            chat.ChatMessages.Add(messageToChat);

            Task.Run(() => chatHistoryService.AddNewMessageToChatHistory(messageToChat));
        }
    }
}
