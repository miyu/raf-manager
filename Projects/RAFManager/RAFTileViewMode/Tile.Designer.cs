namespace RAFManager.RAFTileViewMode
{
    partial class Tile
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
            this.fileIconPB = new System.Windows.Forms.PictureBox();
            this.fileNameLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fileIconPB)).BeginInit();
            this.SuspendLayout();
            // 
            // fileIconPB
            // 
            this.fileIconPB.Location = new System.Drawing.Point(-12, -32);
            this.fileIconPB.Name = "fileIconPB";
            this.fileIconPB.Size = new System.Drawing.Size(162, 100);
            this.fileIconPB.TabIndex = 0;
            this.fileIconPB.TabStop = false;
            // 
            // fileNameLabel
            // 
            this.fileNameLabel.AutoSize = true;
            this.fileNameLabel.Location = new System.Drawing.Point(56, 80);
            this.fileNameLabel.Name = "fileNameLabel";
            this.fileNameLabel.Size = new System.Drawing.Size(46, 17);
            this.fileNameLabel.TabIndex = 1;
            this.fileNameLabel.Text = "label1";
            // 
            // Tile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fileNameLabel);
            this.Controls.Add(this.fileIconPB);
            this.Name = "Tile";
            ((System.ComponentModel.ISupportInitialize)(this.fileIconPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox fileIconPB;
        private System.Windows.Forms.Label fileNameLabel;
    }
}
