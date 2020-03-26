using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.Commands
{
    public class CorrectLogin : Command
    {
        public override string Name => "CorrectLogin";

        public Action UserIsLoggedIn;

        public override void Execute(IMessengerService messenger, string data)
        {
            var user = JsonParser<UserInfo>.JsonToOneObject(data);
            messenger.User.Id = user.Id;
            messenger.User.Login = user.Login;

            UserIsLoggedIn();
        }
    }
}
