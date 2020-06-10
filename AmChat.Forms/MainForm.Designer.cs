namespace AmChat.Forms
{
    partial class MainForm
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
            this.Chat_panel = new System.Windows.Forms.Panel();
            this.InputMessage_textBox = new System.Windows.Forms.TextBox();
            this.Send_button = new System.Windows.Forms.Button();
            this.Chat_richTextBox = new System.Windows.Forms.RichTextBox();
            this.Chats_panel = new System.Windows.Forms.Panel();
            this.AddChat_button = new System.Windows.Forms.Button();
            this.NavigationPanel = new System.Windows.Forms.Panel();
            this.WindowsName_label = new System.Windows.Forms.Label();
            this.MinimizeWindow_button = new System.Windows.Forms.Button();
            this.CloseWindow_button = new System.Windows.Forms.Button();
            this.Chat_panel.SuspendLayout();
            this.NavigationPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Chat_panel
            // 
            this.Chat_panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Chat_panel.Controls.Add(this.InputMessage_textBox);
            this.Chat_panel.Controls.Add(this.Send_button);
            this.Chat_panel.Controls.Add(this.Chat_richTextBox);
            this.Chat_panel.Enabled = false;
            this.Chat_panel.Location = new System.Drawing.Point(2, 26);
            this.Chat_panel.Name = "Chat_panel";
            this.Chat_panel.Size = new System.Drawing.Size(341, 615);
            this.Chat_panel.TabIndex = 0;
            // 
            // InputMessage_textBox
            // 
            this.InputMessage_textBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputMessage_textBox.Location = new System.Drawing.Point(3, 589);
            this.InputMessage_textBox.Name = "InputMessage_textBox";
            this.InputMessage_textBox.Size = new System.Drawing.Size(249, 20);
            this.InputMessage_textBox.TabIndex = 1;
            this.InputMessage_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputMessage_textBox_KeyDown);
            // 
            // Send_button
            // 
            this.Send_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Send_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Send_button.Location = new System.Drawing.Point(259, 587);
            this.Send_button.Name = "Send_button";
            this.Send_button.Size = new System.Drawing.Size(75, 23);
            this.Send_button.TabIndex = 1;
            this.Send_button.Text = "Send";
            this.Send_button.UseVisualStyleBackColor = false;
            this.Send_button.Click += new System.EventHandler(this.Send_button_Click);
            // 
            // Chat_richTextBox
            // 
            this.Chat_richTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Chat_richTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Chat_richTextBox.Location = new System.Drawing.Point(3, 3);
            this.Chat_richTextBox.Name = "Chat_richTextBox";
            this.Chat_richTextBox.ReadOnly = true;
            this.Chat_richTextBox.Size = new System.Drawing.Size(331, 580);
            this.Chat_richTextBox.TabIndex = 2;
            this.Chat_richTextBox.Text = "";
            // 
            // Chats_panel
            // 
            this.Chats_panel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Chats_panel.Location = new System.Drawing.Point(342, 29);
            this.Chats_panel.Name = "Chats_panel";
            this.Chats_panel.Size = new System.Drawing.Size(155, 580);
            this.Chats_panel.TabIndex = 1;
            // 
            // AddChat_button
            // 
            this.AddChat_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.AddChat_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.AddChat_button.Location = new System.Drawing.Point(349, 613);
            this.AddChat_button.Name = "AddChat_button";
            this.AddChat_button.Size = new System.Drawing.Size(148, 23);
            this.AddChat_button.TabIndex = 0;
            this.AddChat_button.Text = "Add Chat";
            this.AddChat_button.UseVisualStyleBackColor = false;
            this.AddChat_button.Click += new System.EventHandler(this.AddChat_button_Click);
            // 
            // NavigationPanel
            // 
            this.NavigationPanel.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.NavigationPanel.Controls.Add(this.WindowsName_label);
            this.NavigationPanel.Controls.Add(this.MinimizeWindow_button);
            this.NavigationPanel.Controls.Add(this.CloseWindow_button);
            this.NavigationPanel.Location = new System.Drawing.Point(0, 0);
            this.NavigationPanel.Name = "NavigationPanel";
            this.NavigationPanel.Size = new System.Drawing.Size(497, 22);
            this.NavigationPanel.TabIndex = 2;
            this.NavigationPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseDown);
            this.NavigationPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseMove);
            this.NavigationPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.NavigationPanel_MouseUp);
            // 
            // WindowsName_label
            // 
            this.WindowsName_label.AutoSize = true;
            this.WindowsName_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.WindowsName_label.Location = new System.Drawing.Point(3, 5);
            this.WindowsName_label.Name = "WindowsName_label";
            this.WindowsName_label.Size = new System.Drawing.Size(50, 13);
            this.WindowsName_label.TabIndex = 4;
            this.WindowsName_label.Text = "AmChat";
            // 
            // MinimizeWindow_button
            // 
            this.MinimizeWindow_button.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.MinimizeWindow_button.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MinimizeWindow_button.Location = new System.Drawing.Point(447, 0);
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
            this.CloseWindow_button.Location = new System.Drawing.Point(474, 0);
            this.CloseWindow_button.Name = "CloseWindow_button";
            this.CloseWindow_button.Size = new System.Drawing.Size(23, 23);
            this.CloseWindow_button.TabIndex = 2;
            this.CloseWindow_button.Text = "X";
            this.CloseWindow_button.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseWindow_button.UseVisualStyleBackColor = false;
            this.CloseWindow_button.Click += new System.EventHandler(this.CloseWindow_button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(500, 640);
            this.Controls.Add(this.NavigationPanel);
            this.Controls.Add(this.AddChat_button);
            this.Controls.Add(this.Chats_panel);
            this.Controls.Add(this.Chat_panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MinimumSize = new System.Drawing.Size(500, 600);
            this.Name = "MainForm";
            this.Text = "AmChat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.AM_Chat_Load);
            this.Chat_panel.ResumeLayout(false);
            this.Chat_panel.PerformLayout();
            this.NavigationPanel.ResumeLayout(false);
            this.NavigationPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Chats_panel;
        private System.Windows.Forms.Panel Chat_panel;
        private System.Windows.Forms.TextBox InputMessage_textBox;
        private System.Windows.Forms.Button Send_button;
        private System.Windows.Forms.RichTextBox Chat_richTextBox;
        private System.Windows.Forms.Button AddChat_button;
        private System.Windows.Forms.Panel NavigationPanel;
        private System.Windows.Forms.Button MinimizeWindow_button;
        private System.Windows.Forms.Button CloseWindow_button;
        private System.Windows.Forms.Label WindowsName_label;
    }
}