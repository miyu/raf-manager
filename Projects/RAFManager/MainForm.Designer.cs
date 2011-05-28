namespace RAFManager
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.log = new System.Windows.Forms.TextBox();
            this.logContainer = new System.Windows.Forms.GroupBox();
            this.verboseLoggingCB = new System.Windows.Forms.CheckBox();
            this.rafContentView = new System.Windows.Forms.TreeView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.packToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToRAFManagerHomePageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.projectNameTb = new System.Windows.Forms.ToolStripTextBox();
            this.bigContainer = new System.Windows.Forms.SplitContainer();
            this.smallContainer = new System.Windows.Forms.SplitContainer();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.logContainer.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bigContainer)).BeginInit();
            this.bigContainer.Panel1.SuspendLayout();
            this.bigContainer.Panel2.SuspendLayout();
            this.bigContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.smallContainer)).BeginInit();
            this.smallContainer.Panel1.SuspendLayout();
            this.smallContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log.Location = new System.Drawing.Point(3, 18);
            this.log.Multiline = true;
            this.log.Name = "log";
            this.log.ReadOnly = true;
            this.log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.log.Size = new System.Drawing.Size(764, 151);
            this.log.TabIndex = 0;
            // 
            // logContainer
            // 
            this.logContainer.Controls.Add(this.verboseLoggingCB);
            this.logContainer.Controls.Add(this.log);
            this.logContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logContainer.Location = new System.Drawing.Point(0, 0);
            this.logContainer.Name = "logContainer";
            this.logContainer.Size = new System.Drawing.Size(770, 172);
            this.logContainer.TabIndex = 1;
            this.logContainer.TabStop = false;
            this.logContainer.Text = "Output";
            // 
            // verboseLoggingCB
            // 
            this.verboseLoggingCB.AutoSize = true;
            this.verboseLoggingCB.Location = new System.Drawing.Point(448, 0);
            this.verboseLoggingCB.Name = "verboseLoggingCB";
            this.verboseLoggingCB.Size = new System.Drawing.Size(320, 21);
            this.verboseLoggingCB.TabIndex = 1;
            this.verboseLoggingCB.Text = "Enable Pack-Time Logging (Slows Pack Time)";
            this.verboseLoggingCB.UseVisualStyleBackColor = true;
            // 
            // rafContentView
            // 
            this.rafContentView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rafContentView.Font = new System.Drawing.Font("Lucida Console", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rafContentView.Location = new System.Drawing.Point(0, 0);
            this.rafContentView.Name = "rafContentView";
            this.rafContentView.Size = new System.Drawing.Size(263, 354);
            this.rafContentView.TabIndex = 2;
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.projectNameTb});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(770, 27);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.packToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(68, 24);
            this.toolStripDropDownButton1.Tag = "";
            this.toolStripDropDownButton1.Text = "Project";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(361, 24);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(361, 24);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(361, 24);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // packToolStripMenuItem
            // 
            this.packToolStripMenuItem.Name = "packToolStripMenuItem";
            this.packToolStripMenuItem.Size = new System.Drawing.Size(361, 24);
            this.packToolStripMenuItem.Text = "Pack (Install Checked, Uninstall Unchecked)";
            this.packToolStripMenuItem.Click += new System.EventHandler(this.packToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem,
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem,
            this.goToRAFManagerHomePageToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(54, 24);
            this.toolStripDropDownButton2.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(558, 24);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem
            // 
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Image")));
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Name = "goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem";
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Size = new System.Drawing.Size(558, 24);
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Text = "Go to RAF Manager League of Legends Thread (North American Forums)";
            this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem.Click += new System.EventHandler(this.goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem_Click);
            // 
            // goToRAFPackerLeagueCraftTHreadToolStripMenuItem
            // 
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Image")));
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Name = "goToRAFPackerLeagueCraftTHreadToolStripMenuItem";
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Size = new System.Drawing.Size(558, 24);
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Text = "Go to RAF Manager LeagueCraft Thread";
            this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem.Click += new System.EventHandler(this.goToRAFPackerLeagueCraftTHreadToolStripMenuItem_Click);
            // 
            // goToRAFManagerHomePageToolStripMenuItem
            // 
            this.goToRAFManagerHomePageToolStripMenuItem.Image = global::RAFManager.Properties.Resources.RAFManagerIcon;
            this.goToRAFManagerHomePageToolStripMenuItem.Name = "goToRAFManagerHomePageToolStripMenuItem";
            this.goToRAFManagerHomePageToolStripMenuItem.Size = new System.Drawing.Size(558, 24);
            this.goToRAFManagerHomePageToolStripMenuItem.Text = "Go to RAF Manager Home Page";
            this.goToRAFManagerHomePageToolStripMenuItem.Click += new System.EventHandler(this.goToRAFManagerHomePageToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(106, 24);
            this.toolStripLabel2.Text = "Project Name: ";
            // 
            // projectNameTb
            // 
            this.projectNameTb.Name = "projectNameTb";
            this.projectNameTb.Size = new System.Drawing.Size(150, 27);
            this.projectNameTb.TextChanged += new System.EventHandler(this.projectNameTb_TextChanged_1);
            // 
            // bigContainer
            // 
            this.bigContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bigContainer.Location = new System.Drawing.Point(0, 27);
            this.bigContainer.Name = "bigContainer";
            this.bigContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // bigContainer.Panel1
            // 
            this.bigContainer.Panel1.Controls.Add(this.smallContainer);
            // 
            // bigContainer.Panel2
            // 
            this.bigContainer.Panel2.Controls.Add(this.logContainer);
            this.bigContainer.Size = new System.Drawing.Size(770, 530);
            this.bigContainer.SplitterDistance = 354;
            this.bigContainer.TabIndex = 4;
            // 
            // smallContainer
            // 
            this.smallContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.smallContainer.Location = new System.Drawing.Point(0, 0);
            this.smallContainer.Name = "smallContainer";
            // 
            // smallContainer.Panel1
            // 
            this.smallContainer.Panel1.Controls.Add(this.rafContentView);
            this.smallContainer.Size = new System.Drawing.Size(770, 354);
            this.smallContainer.SplitterDistance = 263;
            this.smallContainer.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 557);
            this.Controls.Add(this.bigContainer);
            this.Controls.Add(this.toolStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RAF Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.logContainer.ResumeLayout(false);
            this.logContainer.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.bigContainer.Panel1.ResumeLayout(false);
            this.bigContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bigContainer)).EndInit();
            this.bigContainer.ResumeLayout(false);
            this.smallContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.smallContainer)).EndInit();
            this.smallContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox log;
        private System.Windows.Forms.GroupBox logContainer;
        private System.Windows.Forms.TreeView rafContentView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem;
        private System.Windows.Forms.SplitContainer bigContainer;
        private System.Windows.Forms.SplitContainer smallContainer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox projectNameTb;
        private System.Windows.Forms.ToolStripMenuItem goToRAFPackerLeagueCraftTHreadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem packToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToRAFManagerHomePageToolStripMenuItem;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.CheckBox verboseLoggingCB;
    }
}

