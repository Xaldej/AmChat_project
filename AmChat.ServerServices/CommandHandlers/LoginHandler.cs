using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class LoginHandler : ICommandHandler
    {
        private readonly IMapper mapper;


        public LoginHandler()
        {   
            var mapperConfig = Mappings.GetLoginHandlerConfig();

            mapper = new Mapper(mapperConfig);
        }


        public void Execute(IMessengerService messenger, string data)
        {
            var loginData = JsonParser<LoginData>.JsonToOneObject(data);
            DBUser dbUser;

            try
            {
                dbUser = GetUserFromDB(loginData);
            }
            catch (Exception e)
            {
                Logger.Log.Error(e.Message);

                Console.WriteLine("User is not logged in");
                SendErrorToClient(messenger);

                return;
            }

            if (loginData.PasswordHash == dbUser.PasswordHash)
            {
                UserInfo userInfo = mapper.Map<UserInfo>(dbUser);

                SendCorrectLoginMessage(messenger, userInfo);

                Console.WriteLine("User is got from DB");
            }
            else
            {
                SendIncorrectLoginError(messenger);
            }

        }

        private void SendIncorrectLoginError(IMessengerService messenger)
        {
            var commandJson = CommandExtentions.GetCommandJson<IncorrectLogin, string>(string.Empty);

            messenger.SendMessage(commandJson);
        }

        private void SendCorrectLoginMessage(IMessengerService messenger, UserInfo userInfo)
        {
            messenger.User = userInfo;

            var commandJson = CommandExtentions.GetCommandJson<CorrectLogin, UserInfo>(messenger.User);

            messenger.SendMessage(commandJson);
        }

        private void SendErrorToClient(IMessengerService messenger)
        {
            var error = new ServerError() { Data = "Login problems. Try to reconnect" };
            var errorJson = JsonParser<ServerError>.OneObjectToJson(error);

            messenger.SendMessage(errorJson);
        }

        private DBUser GetUserFromDB(LoginData loginData)
        {
            DBUser user;
            var userLogin = loginData.Login;
            var passwordHash = loginData.PasswordHash;

            using (var context = new AmChatContext())
            {
                user = context.Users.Where(u => u.Login == userLogin).FirstOrDefault();

                if (user == null)
                {
                    user = new DBUser()
                    {
                        Id = Guid.NewGuid(),
                        Login = userLogin,
                        PasswordHash = passwordHash,
                    };

                    context.Users.Add(user);

                    context.SaveChanges();

                    Console.WriteLine("User added to DB");
                }
            }
            return user;
        }
    }
}
