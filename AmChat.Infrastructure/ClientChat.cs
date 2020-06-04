using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ClientChat : ChatInfo
    {
        public Action<ChatMessage, ClientChat> NewMessageInChat;


        public virtual void OnNewMessageInChat(object sender, NotifyCollectionChangedEventArgs e)
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

        public override bool Equals(object obj)
        {
            if (obj == null)
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
