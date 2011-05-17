namespace DDS_Viewer
{
    partial class TextureViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextureViewer));
            this.d3dPanel = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToDiskddsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDdsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // d3dPanel
            // 
            this.d3dPanel.BackColor = System.Drawing.Color.White;
            this.d3dPanel.Location = new System.Drawing.Point(12, 60);
            this.d3dPanel.Name = "d3dPanel";
            this.d3dPanel.Size = new System.Drawing.Size(189, 191);
            this.d3dPanel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(511, 28);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.saveToDiskddsToolStripMenuItem,
            this.openDdsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(44, 24);
            this.toolStripMenuItem1.Text = "File";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.fileToolStripMenuItem.Text = "Save to Disk (bmp)";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // saveToDiskddsToolStripMenuItem
            // 
            this.saveToDiskddsToolStripMenuItem.Name = "saveToDiskddsToolStripMenuItem";
            this.saveToDiskddsToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.saveToDiskddsToolStripMenuItem.Text = "Save to Disk (dds)";
            this.saveToDiskddsToolStripMenuItem.Click += new System.EventHandler(this.saveToDiskddsToolStripMenuItem_Click);
            // 
            // openDdsToolStripMenuItem
            // 
            this.openDdsToolStripMenuItem.Name = "openDdsToolStripMenuItem";
            this.openDdsToolStripMenuItem.Size = new System.Drawing.Size(204, 24);
            this.openDdsToolStripMenuItem.Text = "Open dds";
            this.openDdsToolStripMenuItem.Click += new System.EventHandler(this.openDdsToolStripMenuItem_Click);
            // 
            // TextureViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(511, 462);
            this.Controls.Add(this.d3dPanel);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "TextureViewer";
            this.Text = "RAF Manager Texture Viewer";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel d3dPanel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToDiskddsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDdsToolStripMenuItem;
    }
}

