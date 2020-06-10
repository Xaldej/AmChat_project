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

        private bool isDragging = false;

        private Point oldPos;

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

        public void CloseForm()
        {
            isClosedByUser = false;
            this.Invoke(new Action(() => this.Close()));
        }

        private void CloswWindow_button_Click(object sender, EventArgs e)
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
