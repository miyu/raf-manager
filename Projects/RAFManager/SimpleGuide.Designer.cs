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
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
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
            this.doPackLabel.Size = new System.Drawing.Size(577, 68);
            this.doPackLabel.TabIndex = 2;
            this.doPackLabel.Text = resources.GetString("doPackLabel.Text");
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(13, 325);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(573, 68);
            this.label4.TabIndex = 3;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(39, 395);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(353, 85);
            this.label5.TabIndex = 4;
            this.label5.Text = "Right click a node of the RAF Tree View:\r\n    Archive Dump\r\n    Archive Search\r\n " +
                "   View contents as Text/DDS/INIBIN/Bitmap\r\nDouble left click node - view conten" +
                "ts by default means";
            // 
            // SimpleGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(605, 495);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}