﻿using AmChat.Data;
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
                var command = CommandConverter.CreateJsonMessageCommand("/correctlogin", id.ToString());
                messenger.SendMessage(command);
            }
            catch
            {
                Console.WriteLine("User is not logged in");
                var error = CommandConverter.CreateJsonMessageCommand("/servererror", "Login problems. Try to reconnect");
                messenger.SendMessage(error);
            }
        }

        private User GetUserFromDB(string userLogin)
        {
            DBUser user;

            using (var context = new AmChatContext())
            {
                user = context.Users.Where(u => u.Login == userLogin).FirstOrDefault();

                if (user == null)
                {
                    user = new DBUser()
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

            User userInfo = UserToUserInfo(user);

            return userInfo;
        }

        private User UserToUserInfo(DBUser user)
        {
            return new User()
            {
                Id = user.Id,
                Login = user.Login
            };
        }
    }
}
