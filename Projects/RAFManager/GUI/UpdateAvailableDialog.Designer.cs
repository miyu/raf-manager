namespace RAFManager
{
    partial class UpdateAvailableDialog
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
            this.messageLabel = new System.Windows.Forms.Label();
            this.updateButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.updateNotesTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // messageLabel
            // 
            this.messageLabel.AutoSize = true;
            this.messageLabel.ForeColor = System.Drawing.Color.Silver;
            this.messageLabel.Location = new System.Drawing.Point(12, 9);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(518, 17);
            this.messageLabel.TabIndex = 1;
            this.messageLabel.Text = "A newer version of RAF Manager (v{0}) is available.  Do you want to download it?";
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(484, 237);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 3;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(565, 237);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Skip";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // updateNotesTextbox
            // 
            this.updateNotesTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.updateNotesTextbox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.updateNotesTextbox.ForeColor = System.Drawing.Color.Silver;
            this.updateNotesTextbox.Location = new System.Drawing.Point(15, 30);
            this.updateNotesTextbox.Multiline = true;
            this.updateNotesTextbox.Name = "updateNotesTextbox";
            this.updateNotesTextbox.ReadOnly = true;
            this.updateNotesTextbox.Size = new System.Drawing.Size(625, 201);
            this.updateNotesTextbox.TabIndex = 5;
            // 
            // UpdateAvailableDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(652, 266);
            this.Controls.Add(this.updateNotesTextbox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.messageLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "UpdateAvailableDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "An Update is Available";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox updateNotesTextbox;
    }
}