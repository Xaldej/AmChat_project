using AmChat.Infrastructure;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public static class Mappings
    {
        public static MapperConfiguration GetChatIsAddedHandlerConfig()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<ChatInfo, Chat>());
        }

        public static MapperConfiguration GetCorrectContactListHandlerConfig()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<ChatInfo, Chat>());
        }
    }
}
