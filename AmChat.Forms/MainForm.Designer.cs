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
            this.Chat_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Chat_panel
            // 
            this.Chat_panel.Controls.Add(this.InputMessage_textBox);
            this.Chat_panel.Controls.Add(this.Send_button);
            this.Chat_panel.Controls.Add(this.Chat_richTextBox);
            this.Chat_panel.Enabled = false;
            this.Chat_panel.Location = new System.Drawing.Point(2, 1);
            this.Chat_panel.Name = "Chat_panel";
            this.Chat_panel.Size = new System.Drawing.Size(325, 561);
            this.Chat_panel.TabIndex = 0;
            // 
            // InputMessage_textBox
            // 
            this.InputMessage_textBox.Location = new System.Drawing.Point(4, 535);
            this.InputMessage_textBox.Name = "InputMessage_textBox";
            this.InputMessage_textBox.Size = new System.Drawing.Size(233, 20);
            this.InputMessage_textBox.TabIndex = 1;
            this.InputMessage_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InputMessage_textBox_KeyDown);
            // 
            // Send_button
            // 
            this.Send_button.Location = new System.Drawing.Point(243, 533);
            this.Send_button.Name = "Send_button";
            this.Send_button.Size = new System.Drawing.Size(75, 23);
            this.Send_button.TabIndex = 1;
            this.Send_button.Text = "Send";
            this.Send_button.UseVisualStyleBackColor = true;
            this.Send_button.Click += new System.EventHandler(this.Send_button_Click);
            // 
            // Chat_richTextBox
            // 
            this.Chat_richTextBox.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Chat_richTextBox.Location = new System.Drawing.Point(3, 3);
            this.Chat_richTextBox.Name = "Chat_richTextBox";
            this.Chat_richTextBox.ReadOnly = true;
            this.Chat_richTextBox.Size = new System.Drawing.Size(315, 526);
            this.Chat_richTextBox.TabIndex = 2;
            this.Chat_richTextBox.Text = "";
            // 
            // Chats_panel
            // 
            this.Chats_panel.Location = new System.Drawing.Point(326, 1);
            this.Chats_panel.Name = "Chats_panel";
            this.Chats_panel.Size = new System.Drawing.Size(155, 529);
            this.Chats_panel.TabIndex = 1;
            // 
            // AddChat_button
            // 
            this.AddChat_button.Location = new System.Drawing.Point(333, 534);
            this.AddChat_button.Name = "AddChat_button";
            this.AddChat_button.Size = new System.Drawing.Size(148, 23);
            this.AddChat_button.TabIndex = 0;
            this.AddChat_button.Text = "Add Chat";
            this.AddChat_button.UseVisualStyleBackColor = true;
            this.AddChat_button.Click += new System.EventHandler(this.AddChat_button_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 561);
            this.Controls.Add(this.AddChat_button);
            this.Controls.Add(this.Chats_panel);
            this.Controls.Add(this.Chat_panel);
            this.Name = "MainForm";
            this.Text = "AmChat";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.AM_Chat_Load);
            this.Chat_panel.ResumeLayout(false);
            this.Chat_panel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Chats_panel;
        private System.Windows.Forms.Panel Chat_panel;
        private System.Windows.Forms.TextBox InputMessage_textBox;
        private System.Windows.Forms.Button Send_button;
        private System.Windows.Forms.RichTextBox Chat_richTextBox;
        private System.Windows.Forms.Button AddChat_button;
    }
}