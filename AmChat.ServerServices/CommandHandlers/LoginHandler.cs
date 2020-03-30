using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromServerToClient;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ServerServices.CommandHandlers
{
    public class LoginHandler : ICommandHandler
    {
        public void Execute(IMessengerService messenger, string data)
        {
            var loginData = JsonParser<LoginData>.JsonToOneObject(data);
            DBUser dbUser;

            try
            {
                dbUser = GetUserFromDB(loginData);
            }
            catch
            {
                Console.WriteLine("User is not logged in");

                var error = new ServerError() { Data = "Login problems. Try to reconnect" };
                var errorJson = JsonParser<ServerError>.OneObjectToJson(error);
                
                messenger.SendMessage(errorJson);

                return;
            }

            if (loginData.PasswordHash == dbUser.PasswordHash)
            {
                UserInfo userInfo = UserToUserInfo(dbUser);
                messenger.User = userInfo;

                var userInfoJson = JsonParser<UserInfo>.OneObjectToJson(messenger.User);

                var command = new CorrectLogin() { Data = userInfoJson };
                var commandJson = JsonParser<CorrectLogin>.OneObjectToJson(command);
                
                Console.WriteLine("User is got from DB");

                messenger.SendMessage(commandJson);
            }
            else
            {
                var command = new IncorrectLogin() { Data = string.Empty };
                var commandJson = JsonParser<IncorrectLogin>.OneObjectToJson(command);
                
                messenger.SendMessage(commandJson);
            }

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

        private UserInfo UserToUserInfo(DBUser user)
        {
            return new UserInfo()
            {
                Id = user.Id,
                Login = user.Login
            };
        }
    }
}
