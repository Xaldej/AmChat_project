using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class ChatMessage
    {
        public UserInfo FromUser { get; set; }

        public Guid ToChatId { get; set; }

        public DateTime DateAndTime { get; set; }

        public string Text { get; set; }

        public override bool Equals(object obj)
        {   
            if (!(obj is ChatMessage messageToCompare))
            {
                return false;
            }

            return FromUser.Equals(messageToCompare.FromUser)
                    &&ToChatId==messageToCompare.ToChatId
                    &&Text==messageToCompare.Text;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
