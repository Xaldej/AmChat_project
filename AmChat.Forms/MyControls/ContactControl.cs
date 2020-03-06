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
        public UserInfo User { get; set; }

        public Action<ContactControl> ContactChosen;

        public ContactControl(UserInfo user)
        {
            User = user;

            InitializeComponent();

            ContactLogin_label.Text = User.Login;
        }

        private void ContactControl_Click(object sender, EventArgs e)
        {
            ContactChosen(this);
        }
    }
}
