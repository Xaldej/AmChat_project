using AmChat.Infrastructure;
using System;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;

namespace AmChat.Forms.MyControls
{
    public partial class ChatControl : UserControl
    {
        public ChatInfoWindow ChatInfoWindow { get; set; }

        public ChatInfo Chat { get; set; }

        public Action<ChatControl> ChatChosen;

        public Action<ChatInfo, List<string>> NewChatLoginsEntered;

        public ChatControl(ChatInfo chat)
        {
            Chat = chat;

            InitializeComponent();

            ChatInfoWindow = new ChatInfoWindow(Chat);
            ChatInfoWindow.NewChatLoginsEntered += OnNewChatLoginsEntered;

            ContactLogin_label.Text = chat.Name;
        }

        private void OnNewChatLoginsEntered(List<string> loginsToAdd)
        {
            NewChatLoginsEntered(Chat, loginsToAdd);
        }

        private void ContactControl_Click(object sender, EventArgs e)
        {
            ChatChosen(this);
            BackColor = Color.White;
            ContactLogin_label.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
        }

        public void ShowUnreadMessagesNotification()
        {
            ContactLogin_label.Invoke(new Action(() => ContactLogin_label.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)));
        }

        private void ChatInfo_button_Click(object sender, EventArgs e)
        {
            ChatInfoWindow.UpdateInfo();
            ChatInfoWindow.Show();
        }
    }
}
