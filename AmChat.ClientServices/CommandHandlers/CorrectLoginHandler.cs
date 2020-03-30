using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices.CommandHandlers
{
    public class CorrectLoginHandler : ICommandHandler
    {
        public Action UserIsLoggedIn;

        public void Execute(IMessengerService messenger, string data)
        {
            var user = JsonParser<UserInfo>.JsonToOneObject(data);
            messenger.User.Id = user.Id;
            messenger.User.Login = user.Login;

            UserIsLoggedIn();
        }
    }
}
