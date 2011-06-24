namespace RAFManager
{
    partial class RAFManagerCleanWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RAFManagerCleanWizard));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.Panel1_Introduction = new System.Windows.Forms.Panel();
            this.Panel1_Next = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.wProgressBar1 = new RAFManager.GUI.WProgressBar();
            this.wProgressBar2 = new RAFManager.GUI.WProgressBar();
            this.Panel1_Introduction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel1_Next)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox1.ForeColor = System.Drawing.Color.Silver;
            this.textBox1.Location = new System.Drawing.Point(7, 6);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(432, 159);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // Panel1_Introduction
            // 
            this.Panel1_Introduction.Controls.Add(this.Panel1_Next);
            this.Panel1_Introduction.Controls.Add(this.textBox1);
            this.Panel1_Introduction.Location = new System.Drawing.Point(12, 90);
            this.Panel1_Introduction.Name = "Panel1_Introduction";
            this.Panel1_Introduction.Size = new System.Drawing.Size(444, 213);
            this.Panel1_Introduction.TabIndex = 2;
            // 
            // Panel1_Next
            // 
            this.Panel1_Next.Image = global::RAFManager.Properties.Resources.Next_Normal;
            this.Panel1_Next.Location = new System.Drawing.Point(328, 171);
            this.Panel1_Next.Name = "Panel1_Next";
            this.Panel1_Next.Size = new System.Drawing.Size(111, 35);
            this.Panel1_Next.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.Panel1_Next.TabIndex = 2;
            this.Panel1_Next.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::RAFManager.Properties.Resources.Back_Normal;
            this.pictureBox3.Location = new System.Drawing.Point(201, 86);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(111, 35);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.wProgressBar2);
            this.panel1.Controls.Add(this.wProgressBar1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(232, 309);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(437, 130);
            this.panel1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(26, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(161, 10);
            this.label4.TabIndex = 3;
            this.label4.Text = "Estimated End Size: 234 MB";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(62, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 10);
            this.label3.TabIndex = 2;
            this.label3.Text = "Current Size: 1234MB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(8, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 10);
            this.label2.TabIndex = 1;
            this.label2.Text = "Estimated Space Freed: 1000MB";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::RAFManager.Properties.Resources.Next_Normal;
            this.pictureBox1.Location = new System.Drawing.Point(318, 86);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(111, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Quickly Analyzing the RAF Archives...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(471, 469);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(418, 184);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Value = 100;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox3.ForeColor = System.Drawing.Color.Silver;
            this.textBox3.Location = new System.Drawing.Point(498, 122);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(418, 133);
            this.textBox3.TabIndex = 4;
            // 
            // wProgressBar1
            // 
            this.wProgressBar1.AText = "ARCHIVE";
            this.wProgressBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.wProgressBar1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wProgressBar1.BorderThickness = 1;
            this.wProgressBar1.FontBorderColor = System.Drawing.Color.Black;
            this.wProgressBar1.FontColor = System.Drawing.Color.White;
            this.wProgressBar1.ForegroundColor = System.Drawing.Color.Black;
            this.wProgressBar1.Location = new System.Drawing.Point(11, 27);
            this.wProgressBar1.Name = "wProgressBar1";
            this.wProgressBar1.Size = new System.Drawing.Size(418, 23);
            this.wProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.wProgressBar1.TabIndex = 6;
            this.wProgressBar1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.wProgressBar1.Value = 50;
            // 
            // wProgressBar2
            // 
            this.wProgressBar2.AText = "dsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsdsd" +
                "sdsdsdsdsdsdsdsdsdsdsdsdsdsds";
            this.wProgressBar2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.wProgressBar2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wProgressBar2.BorderThickness = 1;
            this.wProgressBar2.FontBorderColor = System.Drawing.Color.Black;
            this.wProgressBar2.FontColor = System.Drawing.Color.White;
            this.wProgressBar2.ForegroundColor = System.Drawing.Color.Black;
            this.wProgressBar2.Location = new System.Drawing.Point(11, 56);
            this.wProgressBar2.Name = "wProgressBar2";
            this.wProgressBar2.Size = new System.Drawing.Size(418, 23);
            this.wProgressBar2.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.wProgressBar2.TabIndex = 7;
            this.wProgressBar2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.wProgressBar2.Value = 50;
            // 
            // RAFManagerCleanWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1049, 681);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.Panel1_Introduction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RAFManagerCleanWizard";
            this.Text = "RAF Manager - Clean/Minify Wizard";
            this.Panel1_Introduction.ResumeLayout(false);
            this.Panel1_Introduction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel1_Next)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel Panel1_Introduction;
        private System.Windows.Forms.PictureBox Panel1_Next;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TextBox textBox3;
        private GUI.WProgressBar wProgressBar1;
        private GUI.WProgressBar wProgressBar2;
    }
}