using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmChat.ClientServices
{
    public class LoginService
    {
        public ClientMessengerService Messenger { get; set; }

        public Action CorrectLogin;

        public Action IncorrectLogin;

        public LoginService(ClientMessengerService messenger)
        {
            Messenger = messenger;

            Messenger.CorrectLogin += OnCorrectLogin;

            Messenger.IncorrectLogin += OnIncorrectLogin;
        }


        public void Login(LoginData loginData)
        {
            var loginDataJson = JsonParser<LoginData>.OneObjectToJson(loginData);
            var command = CommandConverter.CreateJsonMessageCommand("/login", loginDataJson);
            Messenger.SendMessage(command);
        }


        private void OnIncorrectLogin()
        {
            IncorrectLogin();
        }

        private void OnCorrectLogin()
        {
            CorrectLogin();
        }
    }
}
