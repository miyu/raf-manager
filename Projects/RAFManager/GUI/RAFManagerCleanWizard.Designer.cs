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
            this.introductionTextbox = new System.Windows.Forms.TextBox();
            this.Panel1_Introduction = new System.Windows.Forms.Panel();
            this.Panel1_Next = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.Panel3_Analyzing = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Panel3_EstimatedSpaceFreedLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Panel4_Minifying = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Panel2_BackupWarning = new System.Windows.Forms.Panel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.wProgressBar3 = new RAFManager.GUI.WProgressBar();
            this.wProgressBar4 = new RAFManager.GUI.WProgressBar();
            this.Panel3_EntryProgressBar = new RAFManager.GUI.WProgressBar();
            this.Panel3_ArchiveProgressBar = new RAFManager.GUI.WProgressBar();
            this.Panel1_Introduction.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel1_Next)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.Panel3_Analyzing.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.Panel4_Minifying.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.Panel2_BackupWarning.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // introductionTextbox
            // 
            this.introductionTextbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.introductionTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.introductionTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.introductionTextbox.ForeColor = System.Drawing.Color.Silver;
            this.introductionTextbox.Location = new System.Drawing.Point(7, 6);
            this.introductionTextbox.Multiline = true;
            this.introductionTextbox.Name = "introductionTextbox";
            this.introductionTextbox.ReadOnly = true;
            this.introductionTextbox.Size = new System.Drawing.Size(432, 159);
            this.introductionTextbox.TabIndex = 1;
            this.introductionTextbox.Text = resources.GetString("introductionTextbox.Text");
            // 
            // Panel1_Introduction
            // 
            this.Panel1_Introduction.Controls.Add(this.Panel1_Next);
            this.Panel1_Introduction.Controls.Add(this.introductionTextbox);
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
            this.Panel1_Next.Click += new System.EventHandler(this.NextButton_Clicked);
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
            this.pictureBox3.Click += new System.EventHandler(this.BackButton_Clicked);
            // 
            // Panel3_Analyzing
            // 
            this.Panel3_Analyzing.Controls.Add(this.Panel3_EntryProgressBar);
            this.Panel3_Analyzing.Controls.Add(this.Panel3_ArchiveProgressBar);
            this.Panel3_Analyzing.Controls.Add(this.label4);
            this.Panel3_Analyzing.Controls.Add(this.label3);
            this.Panel3_Analyzing.Controls.Add(this.Panel3_EstimatedSpaceFreedLabel);
            this.Panel3_Analyzing.Controls.Add(this.pictureBox1);
            this.Panel3_Analyzing.Controls.Add(this.pictureBox3);
            this.Panel3_Analyzing.Controls.Add(this.label1);
            this.Panel3_Analyzing.Location = new System.Drawing.Point(232, 309);
            this.Panel3_Analyzing.Name = "Panel3_Analyzing";
            this.Panel3_Analyzing.Size = new System.Drawing.Size(437, 130);
            this.Panel3_Analyzing.TabIndex = 5;
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
            // Panel3_EstimatedSpaceFreedLabel
            // 
            this.Panel3_EstimatedSpaceFreedLabel.AutoSize = true;
            this.Panel3_EstimatedSpaceFreedLabel.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Panel3_EstimatedSpaceFreedLabel.ForeColor = System.Drawing.Color.Silver;
            this.Panel3_EstimatedSpaceFreedLabel.Location = new System.Drawing.Point(8, 95);
            this.Panel3_EstimatedSpaceFreedLabel.Name = "Panel3_EstimatedSpaceFreedLabel";
            this.Panel3_EstimatedSpaceFreedLabel.Size = new System.Drawing.Size(179, 10);
            this.Panel3_EstimatedSpaceFreedLabel.TabIndex = 1;
            this.Panel3_EstimatedSpaceFreedLabel.Text = "Estimated Space Freed: 1000MB";
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
            this.pictureBox1.Click += new System.EventHandler(this.NextButton_Clicked);
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
            // Panel4_Minifying
            // 
            this.Panel4_Minifying.Controls.Add(this.label9);
            this.Panel4_Minifying.Controls.Add(this.wProgressBar3);
            this.Panel4_Minifying.Controls.Add(this.wProgressBar4);
            this.Panel4_Minifying.Controls.Add(this.label5);
            this.Panel4_Minifying.Controls.Add(this.label6);
            this.Panel4_Minifying.Controls.Add(this.label7);
            this.Panel4_Minifying.Controls.Add(this.pictureBox2);
            this.Panel4_Minifying.Controls.Add(this.label8);
            this.Panel4_Minifying.Location = new System.Drawing.Point(313, 464);
            this.Panel4_Minifying.Name = "Panel4_Minifying";
            this.Panel4_Minifying.Size = new System.Drawing.Size(437, 130);
            this.Panel4_Minifying.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(26, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(161, 10);
            this.label5.TabIndex = 3;
            this.label5.Text = "Estimated End Size: 234 MB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label6.ForeColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(62, 85);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(125, 10);
            this.label6.TabIndex = 2;
            this.label6.Text = "Current Size: 1234MB";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label7.ForeColor = System.Drawing.Color.Silver;
            this.label7.Location = new System.Drawing.Point(8, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(179, 10);
            this.label7.TabIndex = 1;
            this.label7.Text = "Estimated Space Freed: 1000MB";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::RAFManager.Properties.Resources.CloseButtonMed_Normal;
            this.pictureBox2.Location = new System.Drawing.Point(318, 86);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(111, 35);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label8.ForeColor = System.Drawing.Color.Silver;
            this.label8.Location = new System.Drawing.Point(8, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(255, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Minifying Archives - DO NOT CLOSE RAF MANAGER";
            // 
            // Panel2_BackupWarning
            // 
            this.Panel2_BackupWarning.Controls.Add(this.pictureBox6);
            this.Panel2_BackupWarning.Controls.Add(this.pictureBox5);
            this.Panel2_BackupWarning.Controls.Add(this.textBox2);
            this.Panel2_BackupWarning.Location = new System.Drawing.Point(500, 59);
            this.Panel2_BackupWarning.Name = "Panel2_BackupWarning";
            this.Panel2_BackupWarning.Size = new System.Drawing.Size(444, 213);
            this.Panel2_BackupWarning.TabIndex = 3;
            // 
            // pictureBox6
            // 
            this.pictureBox6.Image = global::RAFManager.Properties.Resources.Back_Normal;
            this.pictureBox6.Location = new System.Drawing.Point(211, 171);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(111, 35);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox6.TabIndex = 8;
            this.pictureBox6.TabStop = false;
            this.pictureBox6.Click += new System.EventHandler(this.BackButton_Clicked);
            // 
            // pictureBox5
            // 
            this.pictureBox5.Image = global::RAFManager.Properties.Resources.Next_Normal;
            this.pictureBox5.Location = new System.Drawing.Point(328, 171);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(111, 35);
            this.pictureBox5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox5.TabIndex = 2;
            this.pictureBox5.TabStop = false;
            this.pictureBox5.Click += new System.EventHandler(this.NextButton_Clicked);
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.textBox2.ForeColor = System.Drawing.Color.Silver;
            this.textBox2.Location = new System.Drawing.Point(7, 6);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(432, 159);
            this.textBox2.TabIndex = 1;
            this.textBox2.Text = "Please take the time to go backup your RAF Archives.  This process is long and ca" +
                "nnot be reverted by RAF Manager!\r\n\r\nWhile it has been tested a bit, it\'s still v" +
                "ery experimental.";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Lucida Console", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.label9.ForeColor = System.Drawing.Color.Silver;
            this.label9.Location = new System.Drawing.Point(225, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 10);
            this.label9.TabIndex = 8;
            this.label9.Text = "Working";
            // 
            // wProgressBar3
            // 
            this.wProgressBar3.AText = "ARCHIVE";
            this.wProgressBar3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.wProgressBar3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wProgressBar3.BorderThickness = 1;
            this.wProgressBar3.FontBorderColor = System.Drawing.Color.Black;
            this.wProgressBar3.FontColor = System.Drawing.Color.White;
            this.wProgressBar3.ForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.wProgressBar3.Location = new System.Drawing.Point(11, 56);
            this.wProgressBar3.Name = "wProgressBar3";
            this.wProgressBar3.Size = new System.Drawing.Size(418, 23);
            this.wProgressBar3.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.wProgressBar3.TabIndex = 7;
            this.wProgressBar3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.wProgressBar3.Value = 50;
            // 
            // wProgressBar4
            // 
            this.wProgressBar4.AText = "ARCHIVE";
            this.wProgressBar4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.wProgressBar4.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.wProgressBar4.BorderThickness = 1;
            this.wProgressBar4.FontBorderColor = System.Drawing.Color.Black;
            this.wProgressBar4.FontColor = System.Drawing.Color.White;
            this.wProgressBar4.ForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.wProgressBar4.Location = new System.Drawing.Point(11, 27);
            this.wProgressBar4.Name = "wProgressBar4";
            this.wProgressBar4.Size = new System.Drawing.Size(418, 23);
            this.wProgressBar4.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.wProgressBar4.TabIndex = 6;
            this.wProgressBar4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.wProgressBar4.Value = 50;
            // 
            // Panel3_EntryProgressBar
            // 
            this.Panel3_EntryProgressBar.AText = "ARCHIVE";
            this.Panel3_EntryProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.Panel3_EntryProgressBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Panel3_EntryProgressBar.BorderThickness = 1;
            this.Panel3_EntryProgressBar.FontBorderColor = System.Drawing.Color.Black;
            this.Panel3_EntryProgressBar.FontColor = System.Drawing.Color.White;
            this.Panel3_EntryProgressBar.ForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.Panel3_EntryProgressBar.Location = new System.Drawing.Point(11, 56);
            this.Panel3_EntryProgressBar.Name = "Panel3_EntryProgressBar";
            this.Panel3_EntryProgressBar.Size = new System.Drawing.Size(418, 23);
            this.Panel3_EntryProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Panel3_EntryProgressBar.TabIndex = 7;
            this.Panel3_EntryProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Panel3_EntryProgressBar.Value = 50;
            // 
            // Panel3_ArchiveProgressBar
            // 
            this.Panel3_ArchiveProgressBar.AText = "ARCHIVE";
            this.Panel3_ArchiveProgressBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.Panel3_ArchiveProgressBar.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.Panel3_ArchiveProgressBar.BorderThickness = 1;
            this.Panel3_ArchiveProgressBar.FontBorderColor = System.Drawing.Color.Black;
            this.Panel3_ArchiveProgressBar.FontColor = System.Drawing.Color.White;
            this.Panel3_ArchiveProgressBar.ForegroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(112)))), ((int)(((byte)(112)))), ((int)(((byte)(112)))));
            this.Panel3_ArchiveProgressBar.Location = new System.Drawing.Point(11, 27);
            this.Panel3_ArchiveProgressBar.Name = "Panel3_ArchiveProgressBar";
            this.Panel3_ArchiveProgressBar.Size = new System.Drawing.Size(418, 23);
            this.Panel3_ArchiveProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.Panel3_ArchiveProgressBar.TabIndex = 6;
            this.Panel3_ArchiveProgressBar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Panel3_ArchiveProgressBar.Value = 50;
            // 
            // RAFManagerCleanWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.ClientSize = new System.Drawing.Size(1049, 681);
            this.Controls.Add(this.Panel2_BackupWarning);
            this.Controls.Add(this.Panel4_Minifying);
            this.Controls.Add(this.Panel3_Analyzing);
            this.Controls.Add(this.Panel1_Introduction);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "RAFManagerCleanWizard";
            this.Text = "RAF Manager - Clean/Minify Wizard";
            this.Panel1_Introduction.ResumeLayout(false);
            this.Panel1_Introduction.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Panel1_Next)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.Panel3_Analyzing.ResumeLayout(false);
            this.Panel3_Analyzing.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.Panel4_Minifying.ResumeLayout(false);
            this.Panel4_Minifying.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.Panel2_BackupWarning.ResumeLayout(false);
            this.Panel2_BackupWarning.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox introductionTextbox;
        private System.Windows.Forms.Panel Panel1_Introduction;
        private System.Windows.Forms.PictureBox Panel1_Next;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel Panel3_Analyzing;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label Panel3_EstimatedSpaceFreedLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private GUI.WProgressBar Panel3_ArchiveProgressBar;
        private GUI.WProgressBar Panel3_EntryProgressBar;
        private System.Windows.Forms.Panel Panel4_Minifying;
        private GUI.WProgressBar wProgressBar3;
        private GUI.WProgressBar wProgressBar4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel Panel2_BackupWarning;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.Label label9;
    }
}