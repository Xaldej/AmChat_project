using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure
{
    public class MessageToUser
    {
        public UserInfo FromUser { get; set; }

        public UserInfo ToUser { get; set; }

        public string Text { get; set; }
    }
}
