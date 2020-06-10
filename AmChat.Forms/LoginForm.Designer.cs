namespace AmChat.Forms
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Login_textBox = new System.Windows.Forms.TextBox();
            this.Login_button = new System.Windows.Forms.Button();
            this.Login_label = new System.Windows.Forms.Label();
            this.Password_label = new System.Windows.Forms.Label();
            this.Password_textBox = new System.Windows.Forms.TextBox();
            this.NavigationPanel = new System.Windows.Forms.Panel();
            this.WindowsName_label = new System.Windows.Forms.Label();
            this.MinimizeWindow_button = new System.Windows.Forms.Button();
            this.CloseWindow_button = new System.Windows.Forms.Button();
            this.NavigationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Login_textBox
            // 
            this.Login_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Login_textBox.Location = new System.Drawing.Point(12, 55);
            this.Login_textBox.Name = "Login_textBox";
            this.Login_textBox.Size = new System.Drawing.Size(170, 20);
            this.Login_textBox.TabIndex = 0;
            this.Login_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewKeyPressed);
            // 
            // Login_button
            // 
            this.Login_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Login_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Login_button.Location = new System.Drawing.Point(106, 145);
            this.Login_button.Name = "Login_button";
            this.Login_button.Size = new System.Drawing.Size(75, 23);
            this.Login_button.TabIndex = 1;
            this.Login_button.Text = "Login";
            this.Login_button.UseVisualStyleBackColor = false;
            this.Login_button.Click += new System.EventHandler(this.Login_button_Click);
            // 
            // Login_label
            // 
            this.Login_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Login_label.AutoSize = true;
            this.Login_label.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Login_label.Location = new System.Drawing.Point(13, 39);
            this.Login_label.Name = "Login_label";
            this.Login_label.Size = new System.Drawing.Size(80, 13);
            this.Login_label.TabIndex = 2;
            this.Login_label.Text = "Enter your login";
            // 
            // Password_label
            // 
            this.Password_label.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.Password_label.AutoSize = true;
            this.Password_label.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Password_label.Location = new System.Drawing.Point(13, 96);
            this.Password_label.Name = "Password_label";
            this.Password_label.Size = new System.Drawing.Size(103, 13);
            this.Password_label.TabIndex = 3;
            this.Password_label.Text = "Enter your password";
            // 
            // Password_textBox
            // 
            this.Password_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.Password_textBox.Location = new System.Drawing.Point(12, 112);
            this.Password_textBox.Name = "Password_textBox";
            this.Password_textBox.Size = new System.Drawing.Size(170, 20);
            this.Password_textBox.TabIndex = 4;
            this.Password_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewKeyPressed);
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NavigationPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.NavigationPanel.Controls.Add(this.WindowsName_label);
            this.NavigationPanel.Controls.Add(this.MinimizeWindow_button);
            this.NavigationPanel.Controls.Add(this.CloseWindow_button);
            this.NavigationPanel.Location = new System.Drawing.Point(0, 1);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(201, 22);
            this.NavigationPanel.TabIndex = 5;
            this.NavigationPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseDown);
            this.NavigationPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseMove);
            this.NavigationPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseUp);
            // 
            // WindowsName_label
            // 
            this.WindowsName_label.AutoSize = true;
            this.WindowsName_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WindowsName_label.Location = new System.Drawing.Point(4, 6);
            this.WindowsName_label.Name = "WindowsName_label";
            this.WindowsName_label.Size = new System.Drawing.Size(38, 13);
            this.WindowsName_label.TabIndex = 2;
            this.WindowsName_label.Text = "Login";
            // 
            // MinimizeWindow_button
            // 
            this.MinimizeWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MinimizeWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MinimizeWindow_button.Location = new System.Drawing.Point(146, -1);
            this.MinimizeWindow_button.Name = "MinimizeWindow_button";
            this.MinimizeWindow_button.Size = new System.Drawing.Size(23, 23);
            this.MinimizeWindow_button.TabIndex = 1;
            this.MinimizeWindow_button.Text = "_";
            this.MinimizeWindow_button.UseVisualStyleBackColor = false;
            this.MinimizeWindow_button.Click += new System.EventHandler(this.MinimizeWindow_button_Click);
            // 
            // CloseWindow_button
            // 
            this.CloseWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CloseWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseWindow_button.Location = new System.Drawing.Point(173, -1);
            this.CloseWindow_button.Name = "CloseWindow_button";
            this.CloseWindow_button.Size = new System.Drawing.Size(23, 23);
            this.CloseWindow_button.TabIndex = 0;
            this.CloseWindow_button.Text = "X";
            this.CloseWindow_button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseWindow_button.UseVisualStyleBackColor = false;
            this.CloseWindow_button.Click += new System.EventHandler(this.CloswWindow_button_Click);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(200, 180);
            this.Controls.Add(this.NavigationPanel);
            this.Controls.Add(this.Password_textBox);
            this.Controls.Add(this.Password_label);
            this.Controls.Add(this.Login_label);
            this.Controls.Add(this.Login_button);
            this.Controls.Add(this.Login_textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(200, 180);
            this.Name = "LoginForm";
            this.Text = "Login";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.LoginForm_FormClosed);
            this.NavigationPanel.ResumeLayout(false);
            this.NavigationPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Login_label;
        private System.Windows.Forms.Button Login_button;
        private System.Windows.Forms.TextBox Login_textBox;
        private System.Windows.Forms.Label Password_label;
        private System.Windows.Forms.TextBox Password_textBox;
        private System.Windows.Forms.Panel NavigationPanel;
        private System.Windows.Forms.Button CloseWindow_button;
        private System.Windows.Forms.Button MinimizeWindow_button;
        private System.Windows.Forms.Label WindowsName_label;
    }
}