﻿namespace AmChat.Forms.MyControls
{
    partial class ChatControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ContactLogin_label = new System.Windows.Forms.Label();
            this.ChatInfo_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ContactLogin_label
            // 
            this.ContactLogin_label.AutoSize = true;
            this.ContactLogin_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ContactLogin_label.Location = new System.Drawing.Point(3, 14);
            this.ContactLogin_label.Name = "ContactLogin_label";
            this.ContactLogin_label.Size = new System.Drawing.Size(51, 20);
            this.ContactLogin_label.TabIndex = 0;
            this.ContactLogin_label.Text = "label1";
            this.ContactLogin_label.Click += new System.EventHandler(this.ContactControl_Click);
            // 
            // ChatInfo_button
            // 
            this.ChatInfo_button.BackColor = System.Drawing.Color.White;
            this.ChatInfo_button.Location = new System.Drawing.Point(113, 14);
            this.ChatInfo_button.Name = "ChatInfo_button";
            this.ChatInfo_button.Size = new System.Drawing.Size(37, 23);
            this.ChatInfo_button.TabIndex = 1;
            this.ChatInfo_button.Text = " info";
            this.ChatInfo_button.UseVisualStyleBackColor = false;
            this.ChatInfo_button.Click += new System.EventHandler(this.ChatInfo_button_Click);
            // 
            // ChatControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ChatInfo_button);
            this.Controls.Add(this.ContactLogin_label);
            this.Name = "ChatControl";
            this.Size = new System.Drawing.Size(153, 48);
            this.Click += new System.EventHandler(this.ContactControl_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ContactLogin_label;
        private System.Windows.Forms.Button ChatInfo_button;
    }
}
