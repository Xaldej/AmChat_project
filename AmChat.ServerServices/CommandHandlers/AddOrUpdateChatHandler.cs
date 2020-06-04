using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class AddOrUpdateChatHandler : ICommandHandler
    {
        public NewChatInfo NewChatInfo { get; set; }

        private bool CreateNewChat { get; set; }

        private readonly IMapper mapper;


        public AddOrUpdateChatHandler()
        {
            var mapperConfig = Mappings.GetAddOrUpdateChatHandlerConfig();

            mapper = new Mapper(mapperConfig);
        }


        public void Execute(IMessengerService messenger, string data)
        {
            NewChatInfo = JsonParser<NewChatInfo>.JsonToOneObject(data);

            if (NewChatInfo.Id == null || NewChatInfo.Id == Guid.Empty)
            {
                CreateNewChat = true;
            }
            else
            {
                CreateNewChat = false;
            }

            try
            {
                ProcessNewChatInfo(messenger);
            }
            catch(Exception e)
            {
                Logger.Log.Error(e.Message);

                var error = new ServerError() { Data = "Error adding chat try again" };
                var errorJson = JsonParser<ServerError>.OneObjectToJson(error);
                
                messenger.SendMessage(errorJson);
            }
        }


        private void AddChatsForUsersInDb(List<UserInfo> users, DBChat chat)
        {
            using (var context = new AmChatContext())
            {
                foreach (var user in users)
                {
                    var usersChat = new UserChat()
                    {
                        UserId = user.Id,
                        ChatId = chat.Id,
                    };

                    context.UserChats.Add(usersChat);
                }

                context.SaveChanges();
            }
        }

        private void AddUsersToChatInDB(List<UserInfo> usersToAdd, DBChat chat)
        {
            using (var context = new AmChatContext())
            {
                foreach (var user in usersToAdd)
                {
                    var userInChat = new ChatUser()
                    {
                        Id = Guid.NewGuid(),
                        ChatId = chat.Id,
                        UserId = user.Id,
                    };
                    context.ChatUsers.Add(userInChat);
                }
                context.SaveChanges();
            }
        }

        private DBChat AddChatToDb()
        {
            DBChat chat;
            using (var context = new AmChatContext())
            {
                var id = Guid.NewGuid();
                var userIds = new List<Guid>();

                chat = new DBChat()
                {
                    Id = id,
                    Name = NewChatInfo.Name,
                };

                context.Chats.Add(chat);

                context.SaveChanges();
            }

            return chat;
        }

        private async void AddNewUserToMessengerChat(List<UserInfo> users, ChatInfo chat)
        {
            foreach (var user in users)
            {
                chat.UsersInChat.Add(user);
                await Task.Delay(500);
            }
        }

        private void ChooseNewChatUsers(List<UserInfo> users, DBChat chat)
        {
            List<Guid> usersIdInChat;
            using (var context = new AmChatContext())
            {
                usersIdInChat = context.ChatUsers.Where(cu => cu.ChatId == chat.Id).Select(cu => cu.UserId).ToList();
            }

            foreach (var user in users)
            {
                if (usersIdInChat.Contains(user.Id))
                {
                    users.Remove(user);
                }
            }
        }

        private DBChat GetChatFromDB(Guid id)
        {
            DBChat chat;

            using (var context = new AmChatContext())
            {
                chat = context.Chats.Where(c => c.Id == id).FirstOrDefault();
            }

            return chat;
        }

        private List<DBUser> GetUsersFromDB(List<string> logins)
        {
            var users = new List<DBUser>();

            foreach (var login in logins)
            {
                using (var context = new AmChatContext())
                {
                    var user = context.Users.Where(u => u.Login == login).FirstOrDefault();
                    if (user != null)
                    {
                        users.Add(user);
                    }
                }
            }

            return users;
        }

        private List<UserInfo> GetUserToAdd(List<DBUser> dbUsersToAdd)
        {
            var usersToAdd = new List<UserInfo>();

            foreach (var userToAdd in dbUsersToAdd)
            {
                var user = mapper.Map<UserInfo>(userToAdd);
                if (!usersToAdd.Contains(user))
                {
                    usersToAdd.Add(user);
                }
            }

            return usersToAdd;
        }

        private void ProcessNewChatInfo(IMessengerService messenger)
        {
            DBChat dbChat;

            var dbUsersToAdd = GetUsersFromDB(NewChatInfo.LoginsToAdd);

            List<UserInfo> usersToAdd = GetUserToAdd(dbUsersToAdd);

            if (CreateNewChat)
            {
                dbChat = AddChatToDb();
                if (!usersToAdd.Contains(messenger.User))
                {
                    usersToAdd.Add(messenger.User);
                }
            }
            else
            {
                dbChat = GetChatFromDB(NewChatInfo.Id);
                ChooseNewChatUsers(usersToAdd, dbChat);
            }

            AddUsersToChatInDB(usersToAdd, dbChat);

            AddChatsForUsersInDb(usersToAdd, dbChat);

            ChatInfo chat;
            if (CreateNewChat)
            {
                chat = mapper.Map<ServerChat>(dbChat);

                messenger.UserChats.Add(chat);
            }
            else
            {
                chat = messenger.UserChats.Where(uc => uc.Id == dbChat.Id).FirstOrDefault();
            }

            AddNewUserToMessengerChat(usersToAdd, chat);
        }
    }
}
