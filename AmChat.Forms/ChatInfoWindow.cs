using AmChat.Infrastructure;
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
    public partial class ChatInfoWindow : Form
    {
        public Chat Chat { get; set; }

        public Action<List<string>> NewChatLoginsEntered;

        public ChatInfoWindow(Chat chat)
        {
            Chat = chat;

            InitializeComponent();

            Name = Chat.Name;
        }

        public void UpdateInfo()
        {
            string usersInChat = string.Empty;

            foreach(var user in Chat.UsersInChat)
            {
                usersInChat += user.Login + "\n";
            }

            UsersInChat_label.Text = usersInChat;
        }

        private void AddUsersToChat_button_Click(object sender, EventArgs e)
        {
            var addUsersWindow = new AddChatAdnUsersForm()
            {
                Text = "Add Users"
            };

            addUsersWindow.DisableChatName(Chat.Name);

            addUsersWindow.NewChatInfoEntered += AddUsersToChat;

            addUsersWindow.ShowDialog();
        }

        private void AddUsersToChat(string chatName, List<string> userLogins)
        {
            NewChatLoginsEntered(userLogins);
        }
    }
}
