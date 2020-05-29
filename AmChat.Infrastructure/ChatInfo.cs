using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ChatInfo
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public ICollection<UserInfo> UsersInChat;

        public ICollection<ChatMessage> ChatMessages;

        public ChatInfo()
        {

        }

        public override bool Equals(object obj)
        {
            if (!(obj is ClientChat chatToCompare))
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
            hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<UserInfo>>.Default.GetHashCode(UsersInChat);
            hashCode = hashCode * -1521134295 + EqualityComparer<ICollection<ChatMessage>>.Default.GetHashCode(ChatMessages);
            return hashCode;
        }
    }
}
