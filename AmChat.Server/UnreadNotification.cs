using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server
{
    public class UnreadNotification
    {
        public User ForUser { get; set; }

        public Guid ToChatId { get; set; }

        public bool IsSent { get; set; }

        public UnreadNotification(User forUser, Guid toChatId)
        {
            ForUser = forUser;
            ToChatId = toChatId;
            IsSent = false;
        }
    }
}
