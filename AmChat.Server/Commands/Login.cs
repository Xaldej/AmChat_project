using AmChat.Data;
using AmChat.Data.Entitites;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.Server.Commands
{
    public class Login : Command
    {
        public override string Name => "Login";

        public override void Execute(IMessengerService messenger, string data)
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
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Login problems. Try to reconnect");
                messenger.SendMessage(error);
                return;
            }

            if (loginData.PasswordHash == dbUser.PasswordHash)
            {
                UserInfo userInfo = UserToUserInfo(dbUser);
                messenger.User = userInfo;

                var userInfoJson = JsonParser<UserInfo>.OneObjectToJson(messenger.User);
                var command = CommandConverter.CreateJsonMessageCommand("/correctlogin", userInfoJson);
                Console.WriteLine("User is got from DB");
                messenger.SendMessage(command);
            }
            else
            {
                var command = CommandConverter.CreateJsonMessageCommand("/incorrectlogin", string.Empty);
                messenger.SendMessage(command);
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
