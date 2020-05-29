using AmChat.ClientServices;
using AmChat.Forms.MyControls;
using AmChat.Infrastructure;
using AmChat.Infrastructure.Interfaces;
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
        private List<ChatControl> ChatsControls { get; set; }

        private Chat ChosenChat { get; set; }

        private CommandSender CommandSender { get; set; }

        private IMessengerService Messenger { get; set; }


        public MainForm()
        {
            InitializeComponent();

            Logger.InitLogger();

            ChatsControls = new List<ChatControl>();
        }

        private void AM_Chat_Load(object sender, EventArgs e)
        {
            CreateMessenger();

            CreateCommandHandler();

            CommandSender = new CommandSender(Messenger);

            CommandSender.GetKeyFromServer();

            GetLogin();
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

            commandHandler.ChatAdded += AddChatToContactPanel;
            commandHandler.ErrorIsGotten += ShowErrorToUser;
            commandHandler.CorrectLoginData += OnCorrectLogin;
            commandHandler.IncorrectLoginData += ShowIncorrectLoginMessage;
            commandHandler.NewMessageInChat += ShowNewMessage;

            Messenger.CommandHandler = commandHandler;
        }


        private void AddChatToContactPanel(Chat chat)
        {
            var chatControl = new ChatControl(chat) { Dock = DockStyle.Top };

            chatControl.ChatChosen += ChangeChat;
            chatControl.NewChatLoginsEntered += CommandSender.AddUsersToChat;

            Chats_panel.Invoke(new Action(() => Chats_panel.Controls.Add(chatControl)));

            ChatsControls.Add(chatControl);
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

            ChosenChat = chatControl.Chat;

            UpdateChatHistory();
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

        private void GetLogin()
        {
            var loginForm = new LoginForm();
            loginForm.Owner = this;
            loginForm.LoginDataIsEntered += CommandSender.Login;

            loginForm.ShowDialog();
        }

        private void OnCorrectLogin()
        {
            
            foreach (var form in this.OwnedForms)
            {   
                if (!(form is LoginForm loginForm))
                {
                    return;
                }

                loginForm.CloseForm();
            }

            CommandSender.GetChats();
        }

        private void ShowIncorrectLoginMessage()
        {
            MessageBox.Show("Check login and password and try again", "Incorrect login");
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

        private void ShowNewMessage(ChatMessage message, Chat chat)
        {
            if (message.FromUser.Equals(Messenger.User))
            {
                ShowMessageToOtherUser(message.Text);
            }
            else
            {
                if (ChosenChat == null || chat.Id != ChosenChat.Id)
                {
                    var chatToShowMessage = Messenger.UserChats.Where(c => c.Id == message.ToChatId).FirstOrDefault();
                    ShowUnreadMessages(message);
                }
                else
                {
                    var messageToShow = message.FromUser.Login + ":\n" + message.Text;
                    ShowMessageFromOtherUser(messageToShow);
                }
            }
        }

        private void ShowUnreadMessages(ChatMessage messageToShow)
        {
            var chatControl = ChatsControls.Where(c => c.Chat.Id == messageToShow.ToChatId).FirstOrDefault();

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
                CommandSender.SendMessageToChat(userInput, ChosenChat);
            }
        }

        private void UpdateChatHistory()
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.Clear()));

            foreach (var message in ChosenChat.ChatMessages)
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

            addChatForm.NewChatInfoEntered += CommandSender.AddChat;

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
                CommandSender.CloseConnection();
            }
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
    }
}
