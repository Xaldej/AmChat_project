using AmChat.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmChat.Forms.MyControls
{
    public partial class ContactControl : UserControl
    {
        public ChatInfoWindow ChatInfoWindow { get; set; }

        public UserChat Chat { get; set; }

        public Action<ContactControl> ContactChosen;

        public ContactControl(UserChat chat)
        {
            Chat = chat;

            InitializeComponent();

            ChatInfoWindow = new ChatInfoWindow(Chat);

            ContactLogin_label.Text = chat.Name;
        }

        private void ContactControl_Click(object sender, EventArgs e)
        {
            ContactChosen(this);
            BackColor = Color.Silver;
            ContactLogin_label.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Regular);
        }

        public void ShowUnreadMessagesNotification()
        {
            ContactLogin_label.Invoke(new Action(() => ContactLogin_label.Font = new Font(FontFamily.GenericSansSerif, 12, FontStyle.Bold)));
        }

        private void ChatInfo_button_Click(object sender, EventArgs e)
        {
            ChatInfoWindow.UpdateInfo();
            ChatInfoWindow.ShowDialog();
        }
    }
}
