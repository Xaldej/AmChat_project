using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices
{
    public static class Mappings
    {
        public static MapperConfiguration GetAddOrUpdateChatHandlerConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBUser, UserInfo>();
                cfg.CreateMap<DBChat, Chat>()
                    .AfterMap((src, dest) =>
                    {
                        var usersInChat = new ObservableCollection<UserInfo>();
                        usersInChat.CollectionChanged += dest.OnUsersInChatChanged;
                        dest.UsersInChat = usersInChat;
                    });
            });
        }

        public static MapperConfiguration GetLoginHandlerConfig()
        {
            return new MapperConfiguration(cfg => cfg.CreateMap<DBUser, UserInfo>());
        }

        public static MapperConfiguration GetGetChatsHandlerConfig()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBUser, UserInfo>();
                cfg.CreateMap<Chat, ChatInfo>();
                cfg.CreateMap<DBChat, Chat>();
            });
        }
    }
}
