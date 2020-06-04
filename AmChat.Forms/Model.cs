using AmChat.ClientServices;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Commands;
using AmChat.Infrastructure.Commands.FromClienToServer;
using AmChat.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmChat.Forms
{
    public class Model
    {
        public IMessengerService Messenger { get; set; }


        public Action<string, bool> ErrorIsGotten;

        public Action<ChatMessage, ChatInfo> NewMessageInChat;

        public Action UserIsGottenFromServer;


        public Model()
        {
            Logger.InitLogger();
        }


        public void AddUsersToChat(ChatInfo chat, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chat.Id, userLoginsToAdd);

            var commandJson = CommandMaker.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            Messenger.SendMessage(commandJson);
        }

        public void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            var newChatInfo = new NewChatInfo(chatName, userLoginsToAdd);

            var commandJson = CommandMaker.GetCommandJson<AddOrUpdateChat, NewChatInfo>(newChatInfo);

            Messenger.SendMessage(commandJson);
        }

        public void CloseConnection(int attemptAmount = 0)
        {
            if (Messenger == null)
            {
                return;
            }

            try
            {
                var commandJson = CommandMaker.GetCommandJson<CloseConnection, string>(string.Empty, true);

                Messenger.SendMessage(commandJson);
            }
            catch
            {
                if (attemptAmount < 3)
                {
                    Task.Delay(1000);
                    CloseConnection(attemptAmount + 1);
                }
                else
                {
                    return;
                }
            }
        }

        public void InitializeComponents()
        {
            CreateMessenger();

            CreateCommandHandler();

            GetKeyFromServer();

            GetLogin();
        }

        public void SendMessageToChat(string message, ChatInfo chat)
        {
            var messageToChat = new ChatMessage()
            {
                FromUser = Messenger.User,
                ToChatId = chat.Id,
                DateAndTime = DateTime.Now,
                Text = message,
            };

            var commandJson = CommandMaker.GetCommandJson<SendMessageToChat, ChatMessage>(messageToChat);

            Messenger.SendMessage(commandJson);

            chat.ChatMessages.Add(messageToChat);
        }


        private TcpClient ConnectToServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            var tcpSettings = new TcpSettings(ip, port);

            var tcpConnectionService = new TcpConnectionService(tcpSettings);
            tcpConnectionService.ErrorIsGotten += OnErrorIsGotten;

            return tcpConnectionService.Connect();
        }

        private void CreateMessenger()
        {
            var tcpClient = ConnectToServer();

            Messenger = new ClientMessengerService(tcpClient);

            var thread = new Thread(() => Messenger.ListenMessages()) { IsBackground = true };
            thread.Start();
        }

        private void CreateCommandHandler()
        {
            var commandHandler = new ClientCommandHandlerService(Messenger);

            commandHandler.ErrorIsGotten += OnErrorIsGotten;
            commandHandler.CorrectLoginData += OnCorrectLogin;
            commandHandler.IncorrectLoginData += ShowIncorrectLoginMessage;
            commandHandler.NewMessageInChat += OnNewMessageInChat;

            Messenger.CommandHandler = commandHandler;
        }

        private void GetChats()
        {
            var commandJson = CommandMaker.GetCommandJson<GetChats, string>(string.Empty, true);

            Messenger.SendMessage(commandJson);
        }

        private void GetKeyFromServer()
        {
            var commandJson = CommandMaker.GetCommandJson<GetKey, string>(string.Empty, true);

            Messenger.SendMessage(commandJson);
        }

        private void GetLogin()
        {
            var loginForm = new LoginForm();

            loginForm.LoginDataIsEntered += Login;
            UserIsGottenFromServer += loginForm.CloseForm;

            loginForm.ShowDialog();
        }

        private void Login(LoginData loginData)
        {
            var commandJson = CommandMaker.GetCommandJson<Login, LoginData>(loginData);

            Messenger.SendMessage(commandJson);
        }

        private void OnCorrectLogin()
        {
            UserIsGottenFromServer();

            GetChats();
        }

        private void OnErrorIsGotten(string error, bool closeApp)
        {
            ErrorIsGotten(error, closeApp);
        }

        private void OnNewMessageInChat(ChatMessage message, ChatInfo chat)
        {
            NewMessageInChat(message, chat);
        }

        private void ShowIncorrectLoginMessage()
        {
            MessageBox.Show("Check login and password and try again", "Incorrect login");
        }
    }
}
