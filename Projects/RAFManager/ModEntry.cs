using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItzWarty;

namespace RAFManager
{
    public partial class ModEntry : UserControl
    {
        public bool IsChecked { 
            get { return isChecked; }
            set
            {
                if (isChecked != value)
                    entryEnableCheckboxPB_MouseUp(null, null);
            }
        }
        private bool isChecked = false;
        private RMPropInterpreter script = null;
        public RMPropInterpreter Script 
        {
            get { return script; }
            set
            {
                script = value;
                ManageLayout(true);
            }
        }
        public string ModName { get; set; }
        public string ModCreator { get; set; }
        public string ModURL { get; set; }
        public ModEntry(string modName, string modCreator, string modURL, TreeNode[] nodes)
        {
            InitializeComponent();

            ModName = modName;
            ModCreator = modCreator;
            ModURL = modURL;

            this.entryNameLabel.Text = modName;
            this.entryCreatorLabel.Text += " "+modCreator;
            this.entryWebsiteLabel.Text += " " + modURL;

            this.entryFileListTreeView.Nodes.Clear();
            for (int i = 0; i < nodes.Length; i++)
                this.entryFileListTreeView.Nodes.Add(nodes[i]);

            this.Resize += new EventHandler(ModEntry_Resize);
            TreeViewStylizer.Stylize(this.entryFileListTreeView);

            wGUI.InitializeButton(
                entryEnableCheckboxPB,
                Properties.Resources.ModDisabled,
                Properties.Resources.ModDisabledHover,
                Properties.Resources.ModDisabledDown,
                true
            );

            wGUI.InitializeButton(
                optionsButtonPB,
                Properties.Resources.OptionsButton_Normal,
                Properties.Resources.OptionsButton_Highlight,
                Properties.Resources.OptionsButton_Down,
                false
            );
            wGUI.InitializeButton(
                deleteButtonPB,
                Properties.Resources.DeleteButton_Normal,
                Properties.Resources.DeleteButton_Highlight,
                Properties.Resources.DeleteButton_Down,
                false
            );

            entryEnableCheckboxPB.MouseUp += new MouseEventHandler(entryEnableCheckboxPB_MouseUp);

            ManageLayout(true);
        }

        void entryEnableCheckboxPB_MouseUp(object sender, MouseEventArgs e)
        {
            isChecked = !isChecked;
            if (isChecked)
            {
                wGUI.InitializeButton(
                    entryEnableCheckboxPB,
                    Properties.Resources.ModEnabled,
                    Properties.Resources.ModEnabledHover,
                    Properties.Resources.ModEnabledDown,
                    true
                );
                entryEnableCheckboxPB.Image = Properties.Resources.ModEnabledHover;
            }
            else
            {
                wGUI.InitializeButton(
                    entryEnableCheckboxPB,
                    Properties.Resources.ModDisabled,
                    Properties.Resources.ModDisabledHover,
                    Properties.Resources.ModDisabledDown,
                    true
                );
                entryEnableCheckboxPB.Image = Properties.Resources.ModDisabledHover;
            }
        }

        int padding = 2;
        string rightTriangle    = "▸";
        string downTriangle     = "▾";
        string leftTriangle     = "◂";
        void ModEntry_Resize(object sender, EventArgs e)
        {
            ManageLayout(false);
        }
        void ManageLayout(bool allowSelfResize)
        {
            if (this.Height == 1) return; //we've deleted ourself

            this.entryIconPB.Left = padding;
            this.entryIconPB.Top = padding; //(this.Height - this.entryIconPB.Height) / 2;

            this.entryNameLabel.Left = this.entryIconPB.Right;
            this.entryNameLabel.Width = (int)this.CreateGraphics().MeasureString(this.entryNameLabel.Text, this.entryNameLabel.Font).Width;
            this.entryCreatorLabel.Left = this.entryIconPB.Right;
            this.entryWebsiteLabel.Left = this.entryIconPB.Right;
            this.entryFileListDropdownToggle.Left = this.entryIconPB.Right;

            this.entryEnableCheckboxPB.Top = (entryIconPB.Height - this.entryEnableCheckboxPB.Height) / 2;
            this.entryEnableCheckboxPB.Left = this.Width - this.entryEnableCheckboxPB.Width - this.entryEnableCheckboxPB.Top;

            this.optionsButtonPB.Left = this.entryEnableCheckboxPB.Left - this.optionsButtonPB.Width - padding;
            this.optionsButtonPB.Top = this.entryEnableCheckboxPB.Top + padding;

            this.deleteButtonPB.Left = this.entryEnableCheckboxPB.Left - this.deleteButtonPB.Width - padding;
            this.deleteButtonPB.Top = this.optionsButtonPB.Bottom + padding;

            if (treeViewVisible)
            {
                this.Height = 180;

                this.entryFileListTreeView.Left = padding;
                this.entryFileListTreeView.Top = this.entryIconPB.Bottom + padding;
                this.entryFileListTreeView.Width = this.Width - padding * 2;
                this.entryFileListTreeView.Height = this.Height - this.entryFileListTreeView.Top - padding;
            }
            else
            {
                this.Height = this.entryIconPB.Bottom + this.entryIconPB.Top;

                this.entryFileListTreeView.Left = this.Width + 100;
            }

            optionsButtonPB.Visible = Script != null;
        }
        bool treeViewVisible = false;
        private void entryFileListDropdownToggle_Click(object sender, EventArgs e)
        {
            //toggle visibility of tree view
            treeViewVisible = !treeViewVisible;
            if (treeViewVisible)
                entryFileListDropdownToggle.Text = "Files " + downTriangle;
            else
                entryFileListDropdownToggle.Text = "Files +";
            ManageLayout(true);

            this.OnResize(null);
        }

        public Image IconImage
        {
            get
            {
                return entryIconPB.Image;
            }
            set
            {
                entryIconPB.Image = value;
            }
        }

        private void deleteButtonPB_Click(object sender, EventArgs e)
        {
            //sort of hackish...
            this.Height = 1;
            this.OnResize(null);
        }

        private void optionsButtonPB_Click(object sender, EventArgs e)
        {
            if(this.script != null)
                new ModOptionsWindow(this.script).ShowDialog();
        }
        public ModEntryNodeTag[] GetModEntryTags()
        {
            List<ModEntryNodeTag> filePaths = new List<ModEntryNodeTag>();
            foreach (TreeNode node in this.entryFileListTreeView.Nodes)
            {
                filePaths.Add((ModEntryNodeTag)node.Tag);
            }
            return filePaths.ToArray();
        }
        public void AddFile(string path, string rafPath)
        {
            TreeNode node = new TreeNode(path.Replace("\\", "/").Split("/").Last());
            node.Nodes.Add("Local Path: " + path);
            node.Nodes.Add("RAF Path: " + rafPath);
            node.Tag = new ModEntryNodeTag()
            {
                localPath = path,
                rafPath = rafPath
            };
            this.entryFileListTreeView.Nodes.Add(node);
        }
        public void Delete()
        {
            this.deleteButtonPB_Click(null, null);
        }
    }
}
