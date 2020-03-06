namespace AmChat.Forms
{
    partial class AddContactForm
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
            this.AddContact_label = new System.Windows.Forms.Label();
            this.AddContact_textBox = new System.Windows.Forms.TextBox();
            this.AddContact_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // AddContact_label
            // 
            this.AddContact_label.AutoSize = true;
            this.AddContact_label.Location = new System.Drawing.Point(13, 13);
            this.AddContact_label.Name = "AddContact_label";
            this.AddContact_label.Size = new System.Drawing.Size(133, 13);
            this.AddContact_label.TabIndex = 0;
            this.AddContact_label.Text = "Enter contact name to add";
            // 
            // AddContact_textBox
            // 
            this.AddContact_textBox.Location = new System.Drawing.Point(13, 30);
            this.AddContact_textBox.Name = "AddContact_textBox";
            this.AddContact_textBox.Size = new System.Drawing.Size(279, 20);
            this.AddContact_textBox.TabIndex = 1;
            this.AddContact_textBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddContact_textBox_KeyDown);
            // 
            // AddContact_button
            // 
            this.AddContact_button.Location = new System.Drawing.Point(298, 28);
            this.AddContact_button.Name = "AddContact_button";
            this.AddContact_button.Size = new System.Drawing.Size(75, 23);
            this.AddContact_button.TabIndex = 2;
            this.AddContact_button.Text = "Add";
            this.AddContact_button.UseVisualStyleBackColor = true;
            this.AddContact_button.Click += new System.EventHandler(this.AddContact_button_Click);
            // 
            // AddContactForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 61);
            this.Controls.Add(this.AddContact_button);
            this.Controls.Add(this.AddContact_textBox);
            this.Controls.Add(this.AddContact_label);
            this.Name = "AddContactForm";
            this.Text = "Add Contact";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button AddContact_button;
        private System.Windows.Forms.TextBox AddContact_textBox;
        private System.Windows.Forms.Label AddContact_label;
    }
}