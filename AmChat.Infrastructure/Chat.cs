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
        public Action<UserInfo, Chat> NewUserInChat;

        public Chat()
        {
            UsersInChat = new ObservableCollection<UserInfo>();

            UsersInChat.CollectionChanged += OnUsersInChatChanged;
        }

        private void OnUsersInChatChanged(object sender, NotifyCollectionChangedEventArgs e)
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
