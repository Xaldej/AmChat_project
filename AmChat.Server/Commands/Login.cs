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
            var userName = data;
            try
            {
                messenger.User = GetUserFromDB(userName);
                var id = messenger.User.Id;
                messenger.SendCommand($"/correctlogin:{id}");
            }
            catch (Exception e)
            {
                Console.WriteLine("User is not logged in");
                var errorMessage = "Login problems. Try to reconnect\n" + "Detailed error: " + e.Message;
                messenger.SendCommand(errorMessage);
            }
        }

        private UserInfo GetUserFromDB(string userLogin)
        {
            User user;

            using (var context = new AmChatContext())
            {
                user = context.Users.Where(u => u.Login == userLogin).FirstOrDefault();

                if (user == null)
                {
                    user = new User()
                    {
                        Id = Guid.NewGuid(),
                        Login = userLogin,
                    };

                    context.Users.Add(user);

                    context.SaveChanges();

                    Console.WriteLine("User added to DB");
                }
                else
                {
                    Console.WriteLine("User is got from DB");
                }
            }

            UserInfo userInfo = UserToUserInfo(user);

            return userInfo;
        }

        private UserInfo UserToUserInfo(User user)
        {
            return new UserInfo()
            {
                Id = user.Id,
                Login = user.Login
            };
        }
    }
}
