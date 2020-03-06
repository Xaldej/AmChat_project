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
    public partial class AddContactForm : Form
    {
        public Action<string> LoginToAddIsEntered;

        public AddContactForm()
        {
            InitializeComponent();
        }

        private void AddContact_textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginEntered();
            }
        }

        private void LoginEntered()
        {
            string userLogin = string.Empty;

            userLogin = AddContact_textBox.Text;

            if (userLogin == string.Empty)
            {
                return;
            }

            LoginToAddIsEntered(userLogin);

            this.Close();
        }

        private void AddContact_button_Click(object sender, EventArgs e)
        {
            LoginEntered();
        }
    }
}
