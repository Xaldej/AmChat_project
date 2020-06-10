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
            this.NavigationPanel = new System.Windows.Forms.Panel();
            this.MinimizeWindow_button = new System.Windows.Forms.Button();
            this.CloseWindow_button = new System.Windows.Forms.Button();
            this.WindowName_label = new System.Windows.Forms.Label();
            this.UsersInChat_groupBox.SuspendLayout();
            this.NavigationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // ChatInfo_groupBox
            // 
            this.ChatInfo_groupBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ChatInfo_groupBox.Location = new System.Drawing.Point(15, 38);
            this.ChatInfo_groupBox.Name = "ChatInfo_groupBox";
            this.ChatInfo_groupBox.Size = new System.Drawing.Size(160, 74);
            this.ChatInfo_groupBox.TabIndex = 1;
            this.ChatInfo_groupBox.TabStop = false;
            this.ChatInfo_groupBox.Text = "ChatInfo";
            // 
            // UsersInChat_groupBox
            // 
            this.UsersInChat_groupBox.Controls.Add(this.UsersInChat_label);
            this.UsersInChat_groupBox.Controls.Add(this.AddUsersToChat_button);
            this.UsersInChat_groupBox.ForeColor = System.Drawing.SystemColors.ControlLightLight;
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
            this.AddUsersToChat_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddUsersToChat_button.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.AddUsersToChat_button.Location = new System.Drawing.Point(79, 145);
            this.AddUsersToChat_button.Name = "AddUsersToChat_button";
            this.AddUsersToChat_button.Size = new System.Drawing.Size(75, 23);
            this.AddUsersToChat_button.TabIndex = 0;
            this.AddUsersToChat_button.Text = "Add Users";
            this.AddUsersToChat_button.UseVisualStyleBackColor = false;
            this.AddUsersToChat_button.Click += new System.EventHandler(this.AddUsersToChat_button_Click);
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.NavigationPanel.Controls.Add(this.WindowName_label);
            this.NavigationPanel.Controls.Add(this.MinimizeWindow_button);
            this.NavigationPanel.Controls.Add(this.CloseWindow_button);
            this.NavigationPanel.Location = new System.Drawing.Point(0, 0);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(188, 22);
            this.NavigationPanel.TabIndex = 3;
            // 
            // MinimizeWindow_button
            // 
            this.MinimizeWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MinimizeWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MinimizeWindow_button.Location = new System.Drawing.Point(135, 0);
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
            this.CloseWindow_button.Location = new System.Drawing.Point(162, 0);
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
            this.WindowName_label.Text = "Chat Info";
            // 
            // ChatInfoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(187, 305);
            this.Controls.Add(this.NavigationPanel);
            this.Controls.Add(this.UsersInChat_groupBox);
            this.Controls.Add(this.ChatInfo_groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatInfoWindow";
            this.Text = "Chat Info";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseUp);
            this.UsersInChat_groupBox.ResumeLayout(false);
            this.UsersInChat_groupBox.PerformLayout();
            this.NavigationPanel.ResumeLayout(false);
            this.NavigationPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox ChatInfo_groupBox;
        private System.Windows.Forms.GroupBox UsersInChat_groupBox;
        private System.Windows.Forms.Button AddUsersToChat_button;
        private System.Windows.Forms.Label UsersInChat_label;
        private System.Windows.Forms.Panel NavigationPanel;
        private System.Windows.Forms.Button MinimizeWindow_button;
        private System.Windows.Forms.Button CloseWindow_button;
        private System.Windows.Forms.Label WindowName_label;
    }
}