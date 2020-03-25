using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands
{
    public class CorrectContactList : Command
    {
        public override string Name => "CorrectContactList";

        public override void Execute(IMessengerService messenger, string data)
        {   
            var chatsInfo = new ObservableCollection<ChatInfo>(JsonParser<IEnumerable<ChatInfo>>.JsonToOneObject(data));
            var chats = ChatsIonfoToChats(chatsInfo);

            foreach (var chat in chats)
            {
                if(chat.ChatMessages==null)
                {
                    chat.ChatMessages = new ObservableCollection<ChatMessage>();
                }

                messenger.UserChats.Add(chat);
            }
        }

        private ObservableCollection<Chat> ChatsIonfoToChats(ObservableCollection<ChatInfo> chatsInfo)
        {
            var chats = new ObservableCollection<Chat>();

            foreach (var chatInfo in chatsInfo)
            {
                var chat = new Chat()
                {
                    Id = chatInfo.Id,
                    Name = chatInfo.Name,
                    UsersInChat = chatInfo.UsersInChat,
                    ChatMessages = chatInfo.ChatMessages,
                };
                chats.Add(chat);
            }

            return chats;
        }
    }
}
