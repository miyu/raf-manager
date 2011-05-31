namespace RAFManager
{
    partial class SimpleGuide
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleGuide));
            this.doDragDropLabel = new System.Windows.Forms.Label();
            this.clickCheckBoxLabel = new System.Windows.Forms.Label();
            this.doPackLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // doDragDropLabel
            // 
            this.doDragDropLabel.AutoSize = true;
            this.doDragDropLabel.ForeColor = System.Drawing.Color.Silver;
            this.doDragDropLabel.Location = new System.Drawing.Point(13, 13);
            this.doDragDropLabel.Name = "doDragDropLabel";
            this.doDragDropLabel.Size = new System.Drawing.Size(551, 136);
            this.doDragDropLabel.TabIndex = 0;
            this.doDragDropLabel.Text = resources.GetString("doDragDropLabel.Text");
            // 
            // clickCheckBoxLabel
            // 
            this.clickCheckBoxLabel.AutoSize = true;
            this.clickCheckBoxLabel.ForeColor = System.Drawing.Color.Silver;
            this.clickCheckBoxLabel.Location = new System.Drawing.Point(12, 165);
            this.clickCheckBoxLabel.Name = "clickCheckBoxLabel";
            this.clickCheckBoxLabel.Size = new System.Drawing.Size(560, 68);
            this.clickCheckBoxLabel.TabIndex = 1;
            this.clickCheckBoxLabel.Text = resources.GetString("clickCheckBoxLabel.Text");
            // 
            // doPackLabel
            // 
            this.doPackLabel.AutoSize = true;
            this.doPackLabel.ForeColor = System.Drawing.Color.Silver;
            this.doPackLabel.Location = new System.Drawing.Point(11, 247);
            this.doPackLabel.Name = "doPackLabel";
            this.doPackLabel.Size = new System.Drawing.Size(577, 153);
            this.doPackLabel.TabIndex = 2;
            this.doPackLabel.Text = resources.GetString("doPackLabel.Text");
            // 
            // SimpleGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(605, 411);
            this.Controls.Add(this.doPackLabel);
            this.Controls.Add(this.clickCheckBoxLabel);
            this.Controls.Add(this.doDragDropLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SimpleGuide";
            this.Text = "Simple (Temporary) Guide - Preparing a prettier one soon...";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label doDragDropLabel;
        private System.Windows.Forms.Label clickCheckBoxLabel;
        private System.Windows.Forms.Label doPackLabel;
    }
}