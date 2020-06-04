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

        private ChatInfo ChosenChat { get; set; }

        private ViewModel ViewModel { get; set; }



        public MainForm()
        {
            InitializeComponent();

            ChatsControls = new List<ChatControl>();
        }


        private void AM_Chat_Load(object sender, EventArgs e)
        {
            ViewModel = new ViewModel();

            ViewModel.NewChatIsAdded += AddChatToContactPanel;
            ViewModel.ErrorIsGotten += ShowError;
            ViewModel.NewMessageInChat += ShowNewMessage;
            ViewModel.UserLoginIsUpdated += UpdateWindowText;

            ViewModel.Initialize();
        }


        private void AddChatToContactPanel(ChatInfo chat)
        {
            var chatControl = new ChatControl(chat) { Dock = DockStyle.Top };

            chatControl.ChatChosen += ChangeChat;
            chatControl.NewChatLoginsEntered += ViewModel.OnNewChatLoginsEntered;

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

        private void ShowNewMessage(ChatMessage message, ChatInfo chat)
        {
            if (message.FromUser.Equals(ViewModel.GetUser()))
            {
                AddMessageToChat(message.Text, HorizontalAlignment.Right);
                InputMessage_textBox.Clear();
            }
            else
            {
                if (ChosenChat == null || chat.Id != ChosenChat.Id)
                {
                    ShowUnreadMessages(message);
                }
                else
                {
                    var messageToShow = message.FromUser.Login + ":\n" + message.Text;
                    AddMessageToChat(messageToShow, HorizontalAlignment.Left);
                }
            }
        }

        private void ShowUnreadMessages(ChatMessage messageToShow)
        {
            var chatControl = ChatsControls.Where(c => c.Chat.Id == messageToShow.ToChatId).FirstOrDefault();

            chatControl.ShowUnreadMessagesNotification();
        }

        private void ShowError(string errorText, bool closeApp)
        {
            MessageBox.Show(errorText, "Error");

            if (closeApp == true)
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
                ViewModel.SendMessageToChat(userInput, ChosenChat);
            }
        }

        private void UpdateChatHistory()
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.Clear()));

            foreach (var message in ChosenChat.ChatMessages)
            {
                string messageToShow;
                HorizontalAlignment alignment;

                if (message.FromUser.Equals(ViewModel.GetUser()))
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

        private void UpdateWindowText(string userLogin)
        {
            Invoke(new Action(() => Text = $"AmChat: {userLogin}"));
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

            addChatForm.NewChatInfoEntered += ViewModel.AddChat;

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
            ViewModel.CloseConnection();
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            SendMessage();
        }
    }
}
