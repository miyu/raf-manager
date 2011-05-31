namespace RAFManager
{
    partial class ModEntry
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
            System.Windows.Forms.TreeNode treeNode16 = new System.Windows.Forms.TreeNode("Node1");
            System.Windows.Forms.TreeNode treeNode17 = new System.Windows.Forms.TreeNode("Node3");
            System.Windows.Forms.TreeNode treeNode18 = new System.Windows.Forms.TreeNode("Node4");
            System.Windows.Forms.TreeNode treeNode19 = new System.Windows.Forms.TreeNode("Node2", new System.Windows.Forms.TreeNode[] {
            treeNode17,
            treeNode18});
            System.Windows.Forms.TreeNode treeNode20 = new System.Windows.Forms.TreeNode("Node0", new System.Windows.Forms.TreeNode[] {
            treeNode16,
            treeNode19});
            this.entryWebsiteLabel = new System.Windows.Forms.Label();
            this.entryCreatorLabel = new System.Windows.Forms.Label();
            this.entryNameLabel = new System.Windows.Forms.Label();
            this.entryFileListDropdownToggle = new System.Windows.Forms.Label();
            this.entryFileListTreeView = new System.Windows.Forms.TreeView();
            this.optionsButtonPB = new System.Windows.Forms.PictureBox();
            this.entryEnableCheckboxPB = new System.Windows.Forms.PictureBox();
            this.entryIconPB = new System.Windows.Forms.PictureBox();
            this.deleteButtonPB = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.optionsButtonPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryEnableCheckboxPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryIconPB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteButtonPB)).BeginInit();
            this.SuspendLayout();
            // 
            // entryWebsiteLabel
            // 
            this.entryWebsiteLabel.AutoSize = true;
            this.entryWebsiteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entryWebsiteLabel.ForeColor = System.Drawing.Color.Silver;
            this.entryWebsiteLabel.Location = new System.Drawing.Point(66, 35);
            this.entryWebsiteLabel.Name = "entryWebsiteLabel";
            this.entryWebsiteLabel.Size = new System.Drawing.Size(61, 16);
            this.entryWebsiteLabel.TabIndex = 2;
            this.entryWebsiteLabel.Text = "Website:";
            this.entryWebsiteLabel.Click += new System.EventHandler(this.entryWebsiteLabel_Click);
            // 
            // entryCreatorLabel
            // 
            this.entryCreatorLabel.AutoSize = true;
            this.entryCreatorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F);
            this.entryCreatorLabel.ForeColor = System.Drawing.Color.Silver;
            this.entryCreatorLabel.Location = new System.Drawing.Point(66, 20);
            this.entryCreatorLabel.Name = "entryCreatorLabel";
            this.entryCreatorLabel.Size = new System.Drawing.Size(50, 15);
            this.entryCreatorLabel.TabIndex = 1;
            this.entryCreatorLabel.Text = "Creator:";
            // 
            // entryNameLabel
            // 
            this.entryNameLabel.AutoSize = true;
            this.entryNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entryNameLabel.ForeColor = System.Drawing.Color.Silver;
            this.entryNameLabel.Location = new System.Drawing.Point(63, 1);
            this.entryNameLabel.Name = "entryNameLabel";
            this.entryNameLabel.Size = new System.Drawing.Size(90, 20);
            this.entryNameLabel.TabIndex = 0;
            this.entryNameLabel.Text = "Mod Name";
            this.entryNameLabel.Click += new System.EventHandler(this.entryNameLabel_Click);
            // 
            // entryFileListDropdownToggle
            // 
            this.entryFileListDropdownToggle.AutoSize = true;
            this.entryFileListDropdownToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.2F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.entryFileListDropdownToggle.ForeColor = System.Drawing.Color.Silver;
            this.entryFileListDropdownToggle.Location = new System.Drawing.Point(66, 52);
            this.entryFileListDropdownToggle.Name = "entryFileListDropdownToggle";
            this.entryFileListDropdownToggle.Size = new System.Drawing.Size(43, 15);
            this.entryFileListDropdownToggle.TabIndex = 5;
            this.entryFileListDropdownToggle.Text = "Files +";
            this.entryFileListDropdownToggle.Click += new System.EventHandler(this.entryFileListDropdownToggle_Click);
            // 
            // entryFileListTreeView
            // 
            this.entryFileListTreeView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.entryFileListTreeView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.entryFileListTreeView.ForeColor = System.Drawing.Color.Silver;
            this.entryFileListTreeView.Location = new System.Drawing.Point(3, 72);
            this.entryFileListTreeView.Name = "entryFileListTreeView";
            treeNode16.Name = "Node1";
            treeNode16.Text = "Node1";
            treeNode17.Name = "Node3";
            treeNode17.Text = "Node3";
            treeNode18.Name = "Node4";
            treeNode18.Text = "Node4";
            treeNode19.Name = "Node2";
            treeNode19.Text = "Node2";
            treeNode20.Name = "Node0";
            treeNode20.Text = "Node0";
            this.entryFileListTreeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode20});
            this.entryFileListTreeView.Size = new System.Drawing.Size(553, 102);
            this.entryFileListTreeView.TabIndex = 6;
            // 
            // optionsButtonPB
            // 
            this.optionsButtonPB.Image = global::RAFManager.Properties.Resources.OptionsButton_Normal;
            this.optionsButtonPB.Location = new System.Drawing.Point(436, 9);
            this.optionsButtonPB.Name = "optionsButtonPB";
            this.optionsButtonPB.Size = new System.Drawing.Size(60, 19);
            this.optionsButtonPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.optionsButtonPB.TabIndex = 8;
            this.optionsButtonPB.TabStop = false;
            this.optionsButtonPB.Click += new System.EventHandler(this.optionsButtonPB_Click);
            // 
            // entryEnableCheckboxPB
            // 
            this.entryEnableCheckboxPB.Image = global::RAFManager.Properties.Resources.ModDisabled;
            this.entryEnableCheckboxPB.Location = new System.Drawing.Point(502, 4);
            this.entryEnableCheckboxPB.Name = "entryEnableCheckboxPB";
            this.entryEnableCheckboxPB.Size = new System.Drawing.Size(48, 48);
            this.entryEnableCheckboxPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.entryEnableCheckboxPB.TabIndex = 4;
            this.entryEnableCheckboxPB.TabStop = false;
            // 
            // entryIconPB
            // 
            this.entryIconPB.Image = global::RAFManager.Properties.Resources.NoIcon;
            this.entryIconPB.Location = new System.Drawing.Point(2, 2);
            this.entryIconPB.Name = "entryIconPB";
            this.entryIconPB.Size = new System.Drawing.Size(64, 64);
            this.entryIconPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.entryIconPB.TabIndex = 3;
            this.entryIconPB.TabStop = false;
            // 
            // deleteButtonPB
            // 
            this.deleteButtonPB.Image = global::RAFManager.Properties.Resources.DeleteButton_Normal;
            this.deleteButtonPB.Location = new System.Drawing.Point(436, 31);
            this.deleteButtonPB.Name = "deleteButtonPB";
            this.deleteButtonPB.Size = new System.Drawing.Size(60, 19);
            this.deleteButtonPB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.deleteButtonPB.TabIndex = 9;
            this.deleteButtonPB.TabStop = false;
            this.deleteButtonPB.Click += new System.EventHandler(this.deleteButtonPB_Click);
            // 
            // ModEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.deleteButtonPB);
            this.Controls.Add(this.optionsButtonPB);
            this.Controls.Add(this.entryFileListTreeView);
            this.Controls.Add(this.entryFileListDropdownToggle);
            this.Controls.Add(this.entryEnableCheckboxPB);
            this.Controls.Add(this.entryIconPB);
            this.Controls.Add(this.entryWebsiteLabel);
            this.Controls.Add(this.entryNameLabel);
            this.Controls.Add(this.entryCreatorLabel);
            this.Name = "ModEntry";
            this.Size = new System.Drawing.Size(559, 180);
            ((System.ComponentModel.ISupportInitialize)(this.optionsButtonPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryEnableCheckboxPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.entryIconPB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.deleteButtonPB)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox entryEnableCheckboxPB;
        private System.Windows.Forms.PictureBox entryIconPB;
        private System.Windows.Forms.Label entryWebsiteLabel;
        private System.Windows.Forms.Label entryCreatorLabel;
        private System.Windows.Forms.Label entryNameLabel;
        private System.Windows.Forms.Label entryFileListDropdownToggle;
        private System.Windows.Forms.TreeView entryFileListTreeView;
        private System.Windows.Forms.PictureBox optionsButtonPB;
        private System.Windows.Forms.PictureBox deleteButtonPB;
    }
}
