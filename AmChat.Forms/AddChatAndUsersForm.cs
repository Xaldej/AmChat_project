using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmChat.Forms
{
    public partial class AddChatAndUsersForm : Form
    {
        public Action<string, List<string>> NewChatInfoEntered;

        public AddChatAndUsersForm()
        {
            InitializeComponent();
        }

        private void AddContact_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                CreateNewChatInfo();
            }
        }

        private void CreateNewChatInfo()
        {
            var chatName = ChatName_textBox.Text;

            if (chatName == string.Empty)
            {
                return;
            }

            var userLoginsToAdd = GetLoginsToadd();

            NewChatInfoEntered(chatName, userLoginsToAdd);

            this.Close();
        }

        private List<string> GetLoginsToadd()
        {
            var logins = new List<string>();

            var textBoxes = ListOfLoginsToAdd_panel.Controls.OfType<TextBox>();
            foreach(var textBox in textBoxes)
            {
                if(textBox.Text!=String.Empty)
                {
                    logins.Add(textBox.Text);
                }
            }

            return logins;
        }

        internal void DisableChatName(string chatName)
        {
            ChatName_textBox.Text = chatName;
            ChatName_textBox.Enabled = false;
            CahtName_label.Visible = false;
        }

        private void CreateChat_button_Click(object sender, EventArgs e)
        {
            CreateNewChatInfo();
        }

        private void AddMoreLogins_button_Click(object sender, EventArgs e)
        {
            var loginToAdd = new TextBox()
            {
                Size = new Size(190, 20),
                Dock = DockStyle.Bottom,
            };

            var AddMoreLoginsBottom = AddMoreLogins_button.PointToScreen(new Point(AddMoreLogins_button.Width, AddMoreLogins_button.Height)).Y;
            var addLoginsGroupBoxBotom = AddLogins_groupBox.PointToScreen(new Point(AddLogins_groupBox.Width, AddLogins_groupBox.Height)).Y;

            if ((AddMoreLoginsBottom + 60) < addLoginsGroupBoxBotom + 39)
            {
                AddMoreLogins_button.Top += 20;
                ListOfLoginsToAdd_panel.Height += 20;
            }

            ListOfLoginsToAdd_panel.Controls.Add(loginToAdd);
        }
    }
}
