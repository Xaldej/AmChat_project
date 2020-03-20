namespace AmChat.Forms
{
    partial class ChatInfoWindow
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
            this.ChatInfo_groupBox = new System.Windows.Forms.GroupBox();
            this.UsersInChat_groupBox = new System.Windows.Forms.GroupBox();
            this.UsersInChat_label = new System.Windows.Forms.Label();
            this.AddUsersToChat_button = new System.Windows.Forms.Button();
            this.UsersInChat_groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatInfo_groupBox
            // 
            this.ChatInfo_groupBox.Location = new System.Drawing.Point(15, 12);
            this.ChatInfo_groupBox.Name = "ChatInfo_groupBox";
            this.ChatInfo_groupBox.Size = new System.Drawing.Size(160, 100);
            this.ChatInfo_groupBox.TabIndex = 1;
            this.ChatInfo_groupBox.TabStop = false;
            this.ChatInfo_groupBox.Text = "ChatInfo";
            // 
            // UsersInChat_groupBox
            // 
            this.UsersInChat_groupBox.Controls.Add(this.UsersInChat_label);
            this.UsersInChat_groupBox.Controls.Add(this.AddUsersToChat_button);
            this.UsersInChat_groupBox.Location = new System.Drawing.Point(15, 119);
            this.UsersInChat_groupBox.Name = "UsersInChat_groupBox";
            this.UsersInChat_groupBox.Size = new System.Drawing.Size(160, 174);
            this.UsersInChat_groupBox.TabIndex = 2;
            this.UsersInChat_groupBox.TabStop = false;
            this.UsersInChat_groupBox.Text = "Users In Chat";
            // 
            // UsersInChat_label
            // 
            this.UsersInChat_label.AutoSize = true;
            this.UsersInChat_label.Location = new System.Drawing.Point(7, 20);
            this.UsersInChat_label.Name = "UsersInChat_label";
            this.UsersInChat_label.Size = new System.Drawing.Size(35, 13);
            this.UsersInChat_label.TabIndex = 1;
            this.UsersInChat_label.Text = "label1";
            // 
            // AddUsersToChat_button
            // 
            this.AddUsersToChat_button.Location = new System.Drawing.Point(79, 145);
            this.AddUsersToChat_button.Name = "AddUsersToChat_button";
            this.AddUsersToChat_button.Size = new System.Drawing.Size(75, 23);
            this.AddUsersToChat_button.TabIndex = 0;
            this.AddUsersToChat_button.Text = "Add Users";
            this.AddUsersToChat_button.UseVisualStyleBackColor = true;
            this.AddUsersToChat_button.Click += new System.EventHandler(this.AddUsersToChat_button_Click);
            // 
            // ChatInfoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(187, 305);
            this.Controls.Add(this.UsersInChat_groupBox);
            this.Controls.Add(this.ChatInfo_groupBox);
            this.Name = "ChatInfoWindow";
            this.Text = "Chat Info";
            this.UsersInChat_groupBox.ResumeLayout(false);
            this.UsersInChat_groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ChatInfo_groupBox;
        private System.Windows.Forms.GroupBox UsersInChat_groupBox;
        private System.Windows.Forms.Button AddUsersToChat_button;
        private System.Windows.Forms.Label UsersInChat_label;
    }
}