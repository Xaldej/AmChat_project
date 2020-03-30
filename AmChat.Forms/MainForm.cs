using AmChat.ClientServices;
using AmChat.Forms.MyControls;
using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AmChat.Forms
{
    public partial class MainForm : Form
    {   
        CommandHandlerService CommandHandler { get; set; }
        
        ClientMessengerService Messenger { get; set; }

        List<ChatControl> ChatsControls { get; set; }

        LoginForm LoginForm { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ChatsControls = new List<ChatControl>();
        }

        private void AM_Chat_Load(object sender, EventArgs e)
        {
            CreateMessenger();

            CreateCommandHandler();

            GetLogin();
        }

        private void CreateCommandHandler()
        {
            CommandHandler = new CommandHandlerService(Messenger);
            Messenger.NewMessage += CommandHandler.ProcessMessage;

            CommandHandler.ChatAdded += AddChatToContactPanel;
            CommandHandler.ErrorIsGotten += ShowErrorToUser;
            CommandHandler.MessageToCurrentChatIsGotten += ShowMessageFromOtherUser;
            CommandHandler.MessageToOtherChatIsGotten += ShowUnreadMessages;
            CommandHandler.MessageCorretlySend += ShowMessageToOtherUser;
            CommandHandler.NewUnreadNotification += ShowUnreadNotification;
        }

        private TcpClient ConnectToServer()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            var tcpSettings = new TcpSettings(ip, port);

            var tcpConnectionService = new TcpConnectionService(tcpSettings);
            tcpConnectionService.ErrorIsGotten += ShowErrorToUser;

            return tcpConnectionService.Connect();
        }

        private void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            CommandHandler.AddChat(chatName, userLoginsToAdd);
        }

        private void AddChatToContactPanel(Chat chat)
        {
            var chatControl = new ChatControl(chat) { Dock = DockStyle.Top };

            chatControl.ChatChosen += ChangeChat;
            chatControl.NewChatLoginsEntered += AddUsersToChat;

            Chats_panel.Invoke(new Action(() => Chats_panel.Controls.Add(chatControl)));

            ChatsControls.Add(chatControl);
        }

        private void AddUsersToChat(Chat chat, List<string> userLoginsToAdd)
        {
            CommandHandler.AddUsersToChat(chat, userLoginsToAdd);
        }

        private void AddMessageToChat(string message, HorizontalAlignment alignment)
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.SelectionAlignment = alignment));
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.AppendText(message + "\n\n")));
        }

        private void ChangeChat(ChatControl chatControl)
        {
            var previousChosenControls = Chats_panel.Controls.OfType<ChatControl>().Where(c => c.BackColor == Color.Silver);

            foreach (var control in previousChosenControls)
            {
                control.BackColor = Color.Gainsboro;
            }

            Chat_panel.Enabled = true;

            CommandHandler.ChosenChat = chatControl.Chat;

            UpdateChatHistory();
        }

        private void CreateMessenger()
        {
            var tcpClient = ConnectToServer();

            Messenger = new ClientMessengerService(tcpClient);


            var thread = new Thread(() => Messenger.Process());
            thread.Start();
        }

        private void GetLogin()
        {
            LoginForm = new LoginForm();

            var loginService = new LoginService(Messenger, CommandHandler);
            loginService.CorrectLogin += OnCorrectLogin;
            loginService.IncorrectLogin += LoginForm.ShowIncorrectLoginMessage;

            LoginForm.LoginDataIsEntered += loginService.Login;

            LoginForm.ShowDialog();
        }

        private void OnCorrectLogin()
        {
            LoginForm.CloseForm();
            CommandHandler.GetChats();
        }

        private void ShowMessageFromOtherUser(string message)
        {
            AddMessageToChat(message, HorizontalAlignment.Left);
        }

        private void ShowMessageToOtherUser(string message)
        {
            AddMessageToChat(message, HorizontalAlignment.Right);
            InputMessage_textBox.Clear();
        }

        private void ShowUnreadMessages(ChatMessage messageToShow)
        {
            var chatControl = ChatsControls.Where(c => c.Chat.Id == messageToShow.ToChatId).FirstOrDefault();

            chatControl.ShowUnreadMessagesNotification();
        }

        private void ShowUnreadNotification(Guid chatId)
        {
            var chatControl = ChatsControls.Where(c => c.Chat.Id == chatId).FirstOrDefault();

            chatControl.ShowUnreadMessagesNotification();
        }

        private void ShowErrorToUser(string errorText, bool exitApp)
        {
            MessageBox.Show(errorText, "Error");
            if(exitApp == true)
            {
                Application.Exit();
            }
        }

        private void SendMessage()
        {
            var userInput = InputMessage_textBox.Text;

            var isUserInputCorrect = ValidateUserInput(userInput);

            if (isUserInputCorrect)
            {
                CommandHandler.SendMessageToChat(userInput);
            }
        }

        private void UpdateChatHistory()
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.Clear()));

            foreach (var message in CommandHandler.ChosenChat.ChatMessages)
            {
                string messageToShow;
                HorizontalAlignment alignment;

                if (message.FromUser.Equals(Messenger.User))
                {
                    alignment = HorizontalAlignment.Right;
                    messageToShow = message.Text;
                }
                else
                {
                    alignment = HorizontalAlignment.Left;
                    messageToShow = message.FromUser.Login + ":\n" + message.Text;
                }

                AddMessageToChat(messageToShow, alignment);
            }
        }

        private bool ValidateUserInput(string inputMessage)
        {
            var isMessageCorrect = false;

            if (inputMessage != string.Empty)
            {
                isMessageCorrect = true;
            }

            return isMessageCorrect;
        }


        private void AddChat_button_Click(object sender, EventArgs e)
        {
            var addChatForm = new AddChatAndUsersForm();

            addChatForm.NewChatInfoEntered += AddChat;

            addChatForm.ShowDialog();
        }



        private void InputMessage_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendMessage();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(Messenger!=null)
            {
                CommandHandler.CloseConnection();
            }
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
    }
}
