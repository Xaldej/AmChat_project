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
        public UserChat Chat { get; set; }

        public ChatInfoWindow(UserChat chat)
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
    }
}
