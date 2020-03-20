using AmChat.ClientServices;
using AmChat.Forms.MyControls;
using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AmChat.Forms
{
    public partial class MainForm : Form
    {   
        private ClientMessengerService MessengerService { get; set; }

        private ChatHistoryServise ChatHistoryServise { get; set; }

        List<ChatControl> ChatsControls { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ChatsControls = new List<ChatControl>();
        }

        private void AddChat(string chatName, List<string> userLoginsToAdd)
        {
            MessengerService.AddChat(chatName, userLoginsToAdd);
        }

        private void AddChatToContactPanel(UserChat chat)
        {
            var chatControl = new ChatControl(chat) { Dock = DockStyle.Top };

            chatControl.ChatChosen += ChangeChat;
            chatControl.NewChatLoginsEntered += AddUsersToChat;

            Chats_panel.Invoke(new Action(() => Chats_panel.Controls.Add(chatControl)));

            ChatsControls.Add(chatControl);
        }

        private void AddUsersToChat(UserChat chat, List<string> userLoginsToAdd)
        {
            MessengerService.AddUsersToChat(chat, userLoginsToAdd);
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

            MessengerService.ChosenChat = chatControl.Chat;

            UpdateChatHistory();
        }

        private void CreateMessenger()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            var tcpSettings = new TcpSettings(ip, port);

            MessengerService = new ClientMessengerService();
            MessengerService.ChatAdded += AddChatToContactPanel;
            MessengerService.ErrorIsGotten += ShowErrorToUser;
            MessengerService.MessageToCurrentChatIsGotten += ShowMessageFromOtherUser;
            MessengerService.MessageToOtherChatIsGotten += ShowUnreadMessages;
            MessengerService.MessageCorretlySend += ShowMessageToOtherUser;
            MessengerService.NewUnreadNotification += ShowUnreadNotification;

            var thread = new Thread(() => MessengerService.Process(tcpSettings));
            thread.Start();
        }

        private void GetLogin()
        {
            var loginForm = new LoginForm();

            loginForm.LoginIsEntered += Login;

            loginForm.ShowDialog();
        }

        private void Login(string userLogin)
        {
            MessengerService.User.Login = userLogin;
            MessengerService.Login();
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

        private void ShowUnreadMessages(MessageToChat messageToShow)
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

        private void TrySendMessage()
        {
            var userInput = InputMessage_textBox.Text;

            var isUserInputCorrect = ValidateUserInput(userInput);

            if (isUserInputCorrect)
            {
                MessengerService.SendMessageToChat(userInput);
            }
        }

        private void UpdateChatHistory()
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.Clear()));

            var messagesHistory = ChatHistoryServise.GetHistory(MessengerService.ChosenChat, MessengerService);

            foreach (var historyMessage in messagesHistory)
            {
                HorizontalAlignment alignment;

                if (historyMessage.IsMyMessage == true)
                {
                    alignment = HorizontalAlignment.Right;
                }
                else
                {
                    alignment = HorizontalAlignment.Left;
                }

                AddMessageToChat(historyMessage.Message, alignment);
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
            var addChatForm = new AddChatAdnUsersForm();

            addChatForm.NewChatInfoEntered += AddChat;

            addChatForm.ShowDialog();
        }

        private void AM_Chat_Load(object sender, EventArgs e)
        {
            ChatHistoryServise = new ChatHistoryServise();

            CreateMessenger();

            GetLogin();
        }

        private void InputMessage_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TrySendMessage();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MessengerService.CloseConnection();
            //TO DO: stop all threads
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            TrySendMessage();
        }
    }
}
