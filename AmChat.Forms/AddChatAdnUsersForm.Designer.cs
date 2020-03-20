namespace AmChat.Forms
{
    partial class AddChatAdnUsersForm
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
            this.CreateChat_button = new System.Windows.Forms.Button();
            this.AddLogins_groupBox = new System.Windows.Forms.GroupBox();
            this.ListOfLoginsToAdd_panel = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AddMoreLogins_button = new System.Windows.Forms.Button();
            this.AddLogins_groupBox.SuspendLayout();
            this.ListOfLoginsToAdd_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CahtName_label
            // 
            this.CahtName_label.AutoSize = true;
            this.CahtName_label.Location = new System.Drawing.Point(23, 14);
            this.CahtName_label.Name = "CahtName_label";
            this.CahtName_label.Size = new System.Drawing.Size(85, 13);
            this.CahtName_label.TabIndex = 0;
            this.CahtName_label.Text = "Enter chat name";
            // 
            // ChatName_textBox
            // 
            this.ChatName_textBox.Location = new System.Drawing.Point(26, 30);
            this.ChatName_textBox.Name = "ChatName_textBox";
            this.ChatName_textBox.Size = new System.Drawing.Size(207, 20);
            this.ChatName_textBox.TabIndex = 1;
            this.ChatName_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddContact_textBox_KeyDown);
            // 
            // CreateChat_button
            // 
            this.CreateChat_button.Location = new System.Drawing.Point(167, 268);
            this.CreateChat_button.Name = "CreateChat_button";
            this.CreateChat_button.Size = new System.Drawing.Size(75, 23);
            this.CreateChat_button.TabIndex = 2;
            this.CreateChat_button.Text = "Create Chat";
            this.CreateChat_button.UseVisualStyleBackColor = true;
            this.CreateChat_button.Click += new System.EventHandler(this.CreateChat_button_Click);
            // 
            // AddLogins_groupBox
            // 
            this.AddLogins_groupBox.Controls.Add(this.ListOfLoginsToAdd_panel);
            this.AddLogins_groupBox.Controls.Add(this.AddMoreLogins_button);
            this.AddLogins_groupBox.Location = new System.Drawing.Point(16, 57);
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
            this.AddMoreLogins_button.Location = new System.Drawing.Point(10, 48);
            this.AddMoreLogins_button.Name = "AddMoreLogins_button";
            this.AddMoreLogins_button.Size = new System.Drawing.Size(23, 23);
            this.AddMoreLogins_button.TabIndex = 1;
            this.AddMoreLogins_button.Text = "+";
            this.AddMoreLogins_button.UseVisualStyleBackColor = true;
            this.AddMoreLogins_button.Click += new System.EventHandler(this.AddMoreLogins_button_Click);
            // 
            // AddChatAdnUsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 303);
            this.Controls.Add(this.AddLogins_groupBox);
            this.Controls.Add(this.CreateChat_button);
            this.Controls.Add(this.ChatName_textBox);
            this.Controls.Add(this.CahtName_label);
            this.Name = "AddChatAdnUsersForm";
            this.Text = "Add Chat";
            this.AddLogins_groupBox.ResumeLayout(false);
            this.ListOfLoginsToAdd_panel.ResumeLayout(false);
            this.ListOfLoginsToAdd_panel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateChat_button;
        private System.Windows.Forms.TextBox ChatName_textBox;
        private System.Windows.Forms.Label CahtName_label;
        private System.Windows.Forms.GroupBox AddLogins_groupBox;
        private System.Windows.Forms.Button AddMoreLogins_button;
        private System.Windows.Forms.Panel ListOfLoginsToAdd_panel;
        private System.Windows.Forms.TextBox textBox1;
    }
}