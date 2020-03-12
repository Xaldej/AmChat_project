﻿using AmChat.ClientServices;
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

        List<ContactControl> ContactsControls { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ContactsControls = new List<ContactControl>();
        }

        private void AM_Chat_Load(object sender, EventArgs e)
        {
            ChatHistoryServise = new ChatHistoryServise();

            CreateMessenger();

            GetLogin();
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

        private void CreateMessenger()
        {
            var ip = ConfigurationManager.AppSettings["ServerIP"];
            var port = Int32.Parse(ConfigurationManager.AppSettings["ServerPort"]);

            var tcpSettings = new TcpSettings(ip, port);

            MessengerService = new ClientMessengerService(tcpSettings);
            MessengerService.ContactsReceived += UpdateContacts;
            MessengerService.ContactAdded += AddContactToContactPanel;
            MessengerService.ErrorIsGotten += ShowErrorToUser;
            MessengerService.MessageForCurrentContactIsGotten += ShowMessageFromUser;
            MessengerService.MessageForOtherContactIsGotten += ShowUnreadMessages;
            MessengerService.MessageFromNewContactIsGotten += AddNewContactWithNewMessage;

            var thread = new Thread(MessengerService.Process);
            thread.Start();
        }

        private void AddNewContactWithNewMessage(MessageToUser messageToShow)
        {
            AddContactToContactPanel(messageToShow.FromUser);
            ShowUnreadMessages(messageToShow);
        }

        private void ShowUnreadMessages(MessageToUser messageToShow)
        {
            var fromUser = messageToShow.FromUser;

            var contactControl = ContactsControls.Where(c => c.User.Equals(fromUser)).FirstOrDefault();

            ChatHistoryServise.SaveHistory(fromUser, messageToShow.Text, false);

            contactControl.ShowUnreadMessagesNotification();
        }

        private void ShowErrorToUser(string errorText, bool exitApp)
        {
            MessageBox.Show(errorText, "Error");
            if(exitApp == true)
            {
                Application.Exit();
            }
        }

        private void UpdateContacts(List<UserInfo> contacts)
        {
            Contacts_panel.Invoke(new Action(() => Contacts_panel.Controls.Clear()));

            foreach (var contact in contacts)
            {
                AddContactToContactPanel(contact);
            }
        }

        private void AddContactToContactPanel(UserInfo contact)
        {
            var contactControl = new ContactControl(contact) { Dock = DockStyle.Top };

            contactControl.ContactChosen += ChangeContact;

            Contacts_panel.Invoke(new Action(() => Contacts_panel.Controls.Add(contactControl)));

            ContactsControls.Add(contactControl);
        }

        private void ChangeContact(ContactControl contactControl)
        {
            var previousChosenControls = Contacts_panel.Controls.OfType<ContactControl>().Where(c => c.BackColor == Color.Silver);

            foreach (var control in previousChosenControls)
            {
                control.BackColor = Color.Gainsboro;
            }

            Chat_panel.Enabled = true;

            MessengerService.ChosenUser = contactControl.User;

            UpdateChatHistory();
        }

        private void UpdateChatHistory()
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.Clear()));

            var messagesHistory = ChatHistoryServise.GetHistory(MessengerService.ChosenUser);

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

        private void InputMessage_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TrySendMessage();
            }
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            TrySendMessage();
        }        

        private void TrySendMessage()
        {
            var userInput = InputMessage_textBox.Text;

            var isUserInputCorrect = ValidateUserInput(userInput);

            if (isUserInputCorrect)
            {
                ShowMessageToUser(userInput);
                try
                {
                    MessengerService.SendMessageToUser(userInput);
                }
                catch
                {

                    Chat_richTextBox.SelectionAlignment = HorizontalAlignment.Center;
                    Chat_richTextBox.AppendText("------NO CONNECTION TO SERVER------\n" +
                                                       "message is not sent\n" +
                                                       "try to reconnect\n");
                }
            }
        }

        private void AddMessageToChat(string message, HorizontalAlignment alignment)
        {
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.SelectionAlignment = alignment));
            Chat_richTextBox.Invoke(new Action(() => Chat_richTextBox.AppendText(message + "\n")));
        }

        private void ShowMessageFromUser(string message)
        {
            AddMessageToChat(message, HorizontalAlignment.Left);
            ChatHistoryServise.SaveHistory(MessengerService.ChosenUser, message, false);
        }

        private void ShowMessageToUser(string message)
        {
            AddMessageToChat(message, HorizontalAlignment.Right);
            InputMessage_textBox.Clear();
            ChatHistoryServise.SaveHistory(MessengerService.ChosenUser, message, true);
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

        private void AddContact_button_Click(object sender, EventArgs e)
        {
            var addContactForm = new AddContactForm();

            addContactForm.LoginToAddIsEntered += AddContact;

            addContactForm.ShowDialog();
        }

        private void AddContact(string userName)
        {
            MessengerService.AddContact(userName);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //TO DO: stop all threads
        }
    }
}
