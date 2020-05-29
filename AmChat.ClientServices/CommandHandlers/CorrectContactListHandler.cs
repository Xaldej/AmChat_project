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
    public class CorrectContactListHandler : ICommandHandler
    {
        private readonly IMapper mapper;


        public CorrectContactListHandler()
        {
            var mapperConfig = Mappings.GetCorrectContactListHandlerConfig();

            mapper = new Mapper(mapperConfig);
        }


        public void Execute(IMessengerService messenger, string data)
        {
            var chatsInfo = new ObservableCollection<ChatInfo>(JsonParser<IEnumerable<ChatInfo>>.JsonToOneObject(data));
            var chats = mapper.Map<IEnumerable<ClientChat>>(chatsInfo);

            foreach (var chat in chats)
            {
                ObservableCollection<ChatMessage> chatMessages;
                var chatHistory = chat.ChatMessages;

                if (chatHistory == null)
                {
                    chatMessages = new ObservableCollection<ChatMessage>();
                }
                else
                {
                    chatMessages = new ObservableCollection<ChatMessage>(chatHistory);
                }

                chatMessages.CollectionChanged += chat.OnNewMessageInChat;
                chat.ChatMessages = chatMessages;

                messenger.UserChats.Add(chat);
            }
        }
    }
}
