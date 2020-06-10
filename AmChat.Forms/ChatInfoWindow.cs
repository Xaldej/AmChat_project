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
        public ChatInfo Chat { get; set; }

        public Action<List<string>> NewChatLoginsEntered;


        private Point oldPos;

        private bool isDragging = false;


        public ChatInfoWindow(ChatInfo chat)
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
            var addUsersWindow = new AddChatAndUsersForm()
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

        private void CloseWindow_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MinimizeWindow_button_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void NavigationPanel_MouseDown(object sender, MouseEventArgs e)
        {
            this.isDragging = true;
            this.oldPos = new Point();
            this.oldPos.X = e.X;
            this.oldPos.Y = e.Y;
        }

        private void NavigationPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging)
            {
                Point tmp = new Point(this.Location.X, this.Location.Y);
                tmp.X += e.X - this.oldPos.X;
                tmp.Y += e.Y - this.oldPos.Y;
                this.Location = tmp;
            }
        }

        private void NavigationPanel_MouseUp(object sender, MouseEventArgs e)
        {
            this.isDragging = false;
        }
    }
}
