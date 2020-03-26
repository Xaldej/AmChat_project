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
    public partial class LoginForm : Form
    {
        public Action<LoginData> LoginDataIsEntered;

        public bool isClosedByUser = true;

        public LoginForm()
        {
            InitializeComponent();
        }

        private void Login_button_Click(object sender, EventArgs e)
        {
            LoginAdnPasswordEntered();
        }

        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isClosedByUser)
            {
                Application.Exit();
            }
        }

        private void NewKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginAdnPasswordEntered();
            }
        }

        private void LoginAdnPasswordEntered()
        {
            var login = Login_textBox.Text;
            var password = Password_textBox.Text;

            

            if (login == string.Empty || password == string.Empty)
            {
                return;
            }

            var passwordHash = password.GetHashCode();

            var loginData = new LoginData(login, passwordHash);

            LoginDataIsEntered(loginData);
        }

        public void ShowIncorrectLoginMessage()
        {
            MessageBox.Show("Check login and password and try again", "Incorrect login");
        }

        public void CloseForm()
        {
            isClosedByUser = false;
            this.Invoke(new Action(() => this.Close()));
        }
    }
}
