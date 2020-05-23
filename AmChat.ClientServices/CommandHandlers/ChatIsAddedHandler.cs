using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class ChatIsAddedHandler : ICommandHandler
    {
        private readonly IMapper mapper;


        public ChatIsAddedHandler()
        {
            var mapperConfig = Mappings.GetChatIsAddedHandlerConfig();

            mapper = new Mapper(mapperConfig);
        }


        public void Execute(IMessengerService messenger, string data)
        {
            var chatInfo = JsonParser<ChatInfo>.JsonToOneObject(data);

            var chatToAdd = mapper.Map<Chat>(chatInfo);

            ObservableCollection<ChatMessage> chatMessages;
            var chatHistory = chatToAdd.ChatMessages;

            if (chatHistory == null)
            {
                chatMessages = new ObservableCollection<ChatMessage>();
            }
            else
            {
                chatMessages = new ObservableCollection<ChatMessage>(chatHistory);
            }

            chatMessages.CollectionChanged += chatToAdd.OnNewMessageInChat;
            chatToAdd.ChatMessages = chatMessages;

            messenger.UserChats.Add(chatToAdd);
        }
    }
}
