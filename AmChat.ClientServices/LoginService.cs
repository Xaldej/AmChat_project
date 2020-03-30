using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
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

        CommandHandlerService CommandHandler { get; set; }

        public Action CorrectLogin;

        public Action IncorrectLogin;

        public LoginService(ClientMessengerService messenger, CommandHandlerService commandHandler)
        {
            Messenger = messenger;

            CommandHandler = commandHandler;

            CommandHandler.CorrectLoginData += OnCorrectLogin;

            CommandHandler.IncorrectLoginData += OnIncorrectLogin;
        }


        public void Login(LoginData loginData)
        {
            var loginDataJson = JsonParser<LoginData>.OneObjectToJson(loginData);

            var command = new Login() { Data = loginDataJson };
            var commandJson = JsonParser<Login>.OneObjectToJson(command);
            
            Messenger.SendMessage(commandJson);
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
