namespace AmChat.Forms
{
    partial class AddChatAndUsersForm
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
            this.CahtName_label = new System.Windows.Forms.Label();
            this.ChatName_textBox = new System.Windows.Forms.TextBox();
            this.AddNewChatInfo_button = new System.Windows.Forms.Button();
            this.AddLogins_groupBox = new System.Windows.Forms.GroupBox();
            this.ListOfLoginsToAdd_panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AddMoreLogins_button = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.MinimizeWindow_button = new System.Windows.Forms.Button();
            this.CloseWindow_button = new System.Windows.Forms.Button();
            this.WindowName_label = new System.Windows.Forms.Label();
            this.AddLogins_groupBox.SuspendLayout();
            this.ListOfLoginsToAdd_panel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CahtName_label
            // 
            this.CahtName_label.AutoSize = true;
            this.CahtName_label.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.CahtName_label.Location = new System.Drawing.Point(22, 32);
            this.CahtName_label.Name = "CahtName_label";
            this.CahtName_label.Size = new System.Drawing.Size(85, 13);
            this.CahtName_label.TabIndex = 0;
            this.CahtName_label.Text = "Enter chat name";
            // 
            // ChatName_textBox
            // 
            this.ChatName_textBox.Location = new System.Drawing.Point(25, 48);
            this.ChatName_textBox.Name = "ChatName_textBox";
            this.ChatName_textBox.Size = new System.Drawing.Size(207, 20);
            this.ChatName_textBox.TabIndex = 1;
            this.ChatName_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddContact_textBox_KeyDown);
            // 
            // AddNewChatInfo_button
            // 
            this.AddNewChatInfo_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddNewChatInfo_button.Location = new System.Drawing.Point(166, 286);
            this.AddNewChatInfo_button.Name = "AddNewChatInfo_button";
            this.AddNewChatInfo_button.Size = new System.Drawing.Size(75, 23);
            this.AddNewChatInfo_button.TabIndex = 2;
            this.AddNewChatInfo_button.Text = "Add";
            this.AddNewChatInfo_button.UseVisualStyleBackColor = false;
            this.AddNewChatInfo_button.Click += new System.EventHandler(this.CreateChat_button_Click);
            // 
            // AddLogins_groupBox
            // 
            this.AddLogins_groupBox.Controls.Add(this.ListOfLoginsToAdd_panel);
            this.AddLogins_groupBox.Controls.Add(this.AddMoreLogins_button);
            this.AddLogins_groupBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddLogins_groupBox.Location = new System.Drawing.Point(15, 75);
            this.AddLogins_groupBox.Name = "AddLogins_groupBox";
            this.AddLogins_groupBox.Size = new System.Drawing.Size(226, 205);
            this.AddLogins_groupBox.TabIndex = 3;
            this.AddLogins_groupBox.TabStop = false;
            this.AddLogins_groupBox.Text = "Enter logins to add in chat";
            // 
            // ListOfLoginsToAdd_panel
            // 
            this.ListOfLoginsToAdd_panel.AutoScroll = true;
            this.ListOfLoginsToAdd_panel.AutoScrollMinSize = new System.Drawing.Size(0, 20);
            this.ListOfLoginsToAdd_panel.Controls.Add(this.textBox1);
            this.ListOfLoginsToAdd_panel.Location = new System.Drawing.Point(7, 19);
            this.ListOfLoginsToAdd_panel.Name = "ListOfLoginsToAdd_panel";
            this.ListOfLoginsToAdd_panel.Size = new System.Drawing.Size(216, 20);
            this.ListOfLoginsToAdd_panel.TabIndex = 2;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(216, 20);
            this.textBox1.TabIndex = 0;
            // 
            // AddMoreLogins_button
            // 
            this.AddMoreLogins_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddMoreLogins_button.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AddMoreLogins_button.Location = new System.Drawing.Point(10, 48);
            this.AddMoreLogins_button.Name = "AddMoreLogins_button";
            this.AddMoreLogins_button.Size = new System.Drawing.Size(23, 23);
            this.AddMoreLogins_button.TabIndex = 1;
            this.AddMoreLogins_button.Text = "+";
            this.AddMoreLogins_button.UseVisualStyleBackColor = false;
            this.AddMoreLogins_button.Click += new System.EventHandler(this.AddMoreLogins_button_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.Controls.Add(this.WindowName_label);
            this.panel1.Controls.Add(this.MinimizeWindow_button);
            this.panel1.Controls.Add(this.CloseWindow_button);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 22);
            this.panel1.TabIndex = 4;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseDown);
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseMove);
            this.panel1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseUp);
            // 
            // MinimizeWindow_button
            // 
            this.MinimizeWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MinimizeWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MinimizeWindow_button.Location = new System.Drawing.Point(201, 0);
            this.MinimizeWindow_button.Name = "MinimizeWindow_button";
            this.MinimizeWindow_button.Size = new System.Drawing.Size(23, 23);
            this.MinimizeWindow_button.TabIndex = 3;
            this.MinimizeWindow_button.Text = "_";
            this.MinimizeWindow_button.UseVisualStyleBackColor = false;
            this.MinimizeWindow_button.Click += new System.EventHandler(this.MinimizeWindow_button_Click);
            // 
            // CloseWindow_button
            // 
            this.CloseWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CloseWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.CloseWindow_button.Location = new System.Drawing.Point(228, 0);
            this.CloseWindow_button.Name = "CloseWindow_button";
            this.CloseWindow_button.Size = new System.Drawing.Size(23, 23);
            this.CloseWindow_button.TabIndex = 2;
            this.CloseWindow_button.Text = "X";
            this.CloseWindow_button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseWindow_button.UseVisualStyleBackColor = false;
            this.CloseWindow_button.Click += new System.EventHandler(this.CloseWindow_button_Click);
            // 
            // WindowName_label
            // 
            this.WindowName_label.AutoSize = true;
            this.WindowName_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WindowName_label.Location = new System.Drawing.Point(4, 6);
            this.WindowName_label.Name = "WindowName_label";
            this.WindowName_label.Size = new System.Drawing.Size(59, 13);
            this.WindowName_label.TabIndex = 4;
            this.WindowName_label.Text = "Add Chat";
            // 
            // AddChatAndUsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(254, 323);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.AddLogins_groupBox);
            this.Controls.Add(this.AddNewChatInfo_button);
            this.Controls.Add(this.ChatName_textBox);
            this.Controls.Add(this.CahtName_label);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddChatAndUsersForm";
            this.Text = "Add Chat";
            this.AddLogins_groupBox.ResumeLayout(false);
            this.ListOfLoginsToAdd_panel.ResumeLayout(false);
            this.ListOfLoginsToAdd_panel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddNewChatInfo_button;
        private System.Windows.Forms.TextBox ChatName_textBox;
        private System.Windows.Forms.Label CahtName_label;
        private System.Windows.Forms.GroupBox AddLogins_groupBox;
        private System.Windows.Forms.Button AddMoreLogins_button;
        private System.Windows.Forms.Panel ListOfLoginsToAdd_panel;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button MinimizeWindow_button;
        private System.Windows.Forms.Button CloseWindow_button;
        private System.Windows.Forms.Label WindowName_label;
    }
}