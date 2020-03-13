using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class MessageToChat
    {
        public UserInfo FromUser { get; set; }

        public UserChat ToChat { get; set; }

        public string Text { get; set; }
    }
}
