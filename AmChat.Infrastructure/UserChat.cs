using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class UserChat
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<UserInfo> UsersInChat;

        public ObservableCollection<MessageToChat> ChatMessages;

        public UserChat()
        {
            UsersInChat = new List<UserInfo>();
        }

        public override bool Equals(object obj)
        {
            if (!(obj is UserChat chatToCompare))
            {
                return false;
            }

            return Id == chatToCompare.Id;
        }

        public override int GetHashCode()
        {
            var hashCode = -1118474401;
            hashCode = hashCode * -1521134295 + EqualityComparer<Guid>.Default.GetHashCode(Id);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<UserInfo>>.Default.GetHashCode(UsersInChat);
            hashCode = hashCode * -1521134295 + EqualityComparer<ObservableCollection<MessageToChat>>.Default.GetHashCode(ChatMessages);
            return hashCode;
        }
    }
}
