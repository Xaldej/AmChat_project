using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Infrastructure.Interfaces.ServerServices
{
    public interface IServerSenderService
    {
        void SendCommandToCertainUser(UserInfo user, string command);

        void SendMessageToCertainUser(UserInfo user, ChatMessage message);

        void SendNewMessageToUsersInChat(ChatMessage message, ChatInfo chat);

        void SendNotificationToChat(ChatInfo chat, string notification);
    }
}
