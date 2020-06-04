using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ServerChat : ChatInfo
    {
        public Action<ChatMessage, ChatInfo> NewMessageInChat;

        public Action<UserInfo, ChatInfo> NewUserInChat;


        public void OnNewMessageInChat(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is ChatMessage newMessage))
                {
                    return;
                }
                NewMessageInChat(newMessage, this);

                (sender as ICollection<ChatMessage>)?.Remove(newMessage);
            }
        }

        public void OnUsersInChatChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                if (!(e.NewItems[0] is UserInfo newUser))
                {
                    return;
                }
                NewUserInChat(newUser, this);
            }
        }

        public override bool Equals(object obj)
        {
            if(obj==null)
            {
                return false;
            }

            if (!(obj is ChatInfo chatToCompare))
            {
                return false;
            }

            return Id == chatToCompare.Id;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
