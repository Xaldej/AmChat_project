using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class MessageToChat
    {
        public User FromUser { get; set; }

        public Guid ToChatId { get; set; }

        public string Text { get; set; }
    }
}
