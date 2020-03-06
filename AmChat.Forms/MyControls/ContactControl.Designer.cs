namespace AmChat.Forms.MyControls
{
    partial class ContactControl
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
            // 
            // ContactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.ContactLogin_label);
            this.Name = "ContactControl";
            this.Size = new System.Drawing.Size(153, 48);
            this.Click += new System.EventHandler(this.ContactControl_Click);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ContactLogin_label;
    }
}
