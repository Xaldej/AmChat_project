using AmChat.ClientServices;
using AmChat.Forms.MyControls;
using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmChat.Forms
{
    public partial class MainForm : Form
    {   
        private ClientMessengerService MessengerService { get; set; }

        List<ContactControl> ContactsControls { get; set; }

        public MainForm()
        {
            InitializeComponent();

            ContactsControls = new List<ContactControl>();
        }

        private void AM_Chat_Load(object sender, EventArgs e)
        {
            Login();
        }

        private void Login()
        {
            var loginForm = new LoginForm();

            loginForm.LoginIsEntered += CreateMessenger;

            loginForm.ShowDialog();
        }

        private void CreateMessenger(string userLogin)
        {
            MessengerService = new ClientMessengerService(userLogin);
            MessengerService.ContactsAreUpdated += UpdateContacts;
            MessengerService.ErrorIsGotten += ShowErrorToUser;
            MessengerService.MessageForCurrentContactIsGotten += ShowGottenMessage;
            MessengerService.MessageForOtherContactIsGotten += ShowUnreadMessages;
            MessengerService.MessageFromNewContactIsGotten += AddNewContactWithNewMessage;

            var thread = new Thread(MessengerService.Process);
            thread.Start();
        }

        private void AddNewContactWithNewMessage(MessageToUser obj)
        {
            throw new NotImplementedException();
        }

        private void ShowUnreadMessages(MessageToUser messageToShow)
        {
            var contact = ContactsControls.Where(c => c.User.Id == messageToShow.FromUserId).FirstOrDefault();

            contact.ShowUnreadMessagesNotification();
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
                var contactControl = new ContactControl(contact) { Dock = DockStyle.Top };

                contactControl.ContactChosen += ChangeContact;

                Contacts_panel.Invoke(new Action(() => Contacts_panel.Controls.Add(contactControl)));

                ContactsControls.Add(contactControl);
            }
        }

        private void ChangeContact(ContactControl contactControl)
        {
            //TO DO: update history
            var previousChosenControls = Contacts_panel.Controls.OfType<ContactControl>().Where(c => c.BackColor == Color.Silver);

            foreach (var control in previousChosenControls)
            {
                control.BackColor = Color.Gainsboro;
            }

            Chat_panel.Enabled = true;

            MessengerService.ChosenUser = contactControl.User;
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

        private void ShowGottenMessage(string message)
        {
            ChatHistory_richTextBox.Invoke(new Action(() => ChatHistory_richTextBox.SelectionAlignment = HorizontalAlignment.Left));
            ChatHistory_richTextBox.Invoke(new Action(() => ChatHistory_richTextBox.AppendText(message + "\n")));
        }

        private void TrySendMessage()
        {
            var userInput = InputMessage_textBox.Text;

            var isUserInputCorrect = ValidateUserInput(userInput);

            if (isUserInputCorrect)
            {
                ShowMessage(userInput);
                try
                {
                    MessengerService.SendMessage(userInput);
                }
                catch
                {

                    ChatHistory_richTextBox.SelectionAlignment = HorizontalAlignment.Center;
                    ChatHistory_richTextBox.AppendText("------NO CONNECTION TO SERVER------\n" +
                                                       "message is not sent\n" +
                                                       "try to reconnect\n");
                }
            }
        }

        private void ShowMessage(string userInput)
        {
            ChatHistory_richTextBox.SelectionAlignment = HorizontalAlignment.Right;
            ChatHistory_richTextBox.AppendText(userInput + "\n");
            InputMessage_textBox.Clear();
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
            var message = "/addcontact:" + userName;
            MessengerService.ExecuteCommands(message);
        }
    }
}
