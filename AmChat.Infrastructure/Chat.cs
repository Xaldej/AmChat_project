using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class Chat : ChatInfo
    {
        public Action<ChatMessage, Chat> NewMessageInChat;

        public Action<UserInfo, Chat> NewUserInChat;


        public void OnNewMessageInChat(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is ChatMessage newMessage))
                {
                    return;
                }
                NewMessageInChat(newMessage, this);
            }
        }

        public void OnUsersInChatChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is UserInfo newUser))
                {
                    return;
                }
                NewUserInChat(newUser, this);
            }
        }
    }
}
