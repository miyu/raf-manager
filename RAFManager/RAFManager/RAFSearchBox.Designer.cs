namespace RAFManager
{
    partial class RAFSearchBox
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
            this.stringSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.findNextButton = new System.Windows.Forms.Button();
            this.findPreviousButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // stringSearch
            // 
            this.stringSearch.Location = new System.Drawing.Point(12, 29);
            this.stringSearch.Name = "stringSearch";
            this.stringSearch.Size = new System.Drawing.Size(415, 22);
            this.stringSearch.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Find (case insensitive search):";
            // 
            // findNextButton
            // 
            this.findNextButton.Location = new System.Drawing.Point(296, 54);
            this.findNextButton.Name = "findNextButton";
            this.findNextButton.Size = new System.Drawing.Size(131, 34);
            this.findNextButton.TabIndex = 2;
            this.findNextButton.Text = "Find Next";
            this.findNextButton.UseVisualStyleBackColor = true;
            this.findNextButton.Click += new System.EventHandler(this.findNextButton_Click);
            // 
            // findPreviousButton
            // 
            this.findPreviousButton.Location = new System.Drawing.Point(159, 54);
            this.findPreviousButton.Name = "findPreviousButton";
            this.findPreviousButton.Size = new System.Drawing.Size(131, 34);
            this.findPreviousButton.TabIndex = 3;
            this.findPreviousButton.Text = "Find Previous";
            this.findPreviousButton.UseVisualStyleBackColor = true;
            this.findPreviousButton.Click += new System.EventHandler(this.findPreviousButton_Click);
            // 
            // RAFSearchBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 100);
            this.Controls.Add(this.findPreviousButton);
            this.Controls.Add(this.findNextButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stringSearch);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "RAFSearchBox";
            this.Text = "RAF Search Box";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox stringSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button findNextButton;
        private System.Windows.Forms.Button findPreviousButton;
    }
}