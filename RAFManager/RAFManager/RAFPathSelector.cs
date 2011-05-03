using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAFManager
{
    public partial class RAFPathSelector : Form
    {
        private static Size lastSize = Size.Empty;
        private static Point lastLocation = Point.Empty;
        public RAFPathSelector(TreeNode[] nodes)
        {
            InitializeComponent();
            //Resizes layout and add handler for resizing
            ManageLayout();
            this.Resize += delegate(object s, EventArgs e){ ManageLayout(); };

            //Add event handler for form closing, to store last location
            this.FormClosing += new FormClosingEventHandler(TreeNodeSelector_FormClosing);

            //When a node is clicked, GUI is updated to show the path of the node
            this.treeView.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView_NodeMouseClick);

            //When we are loaded, we set our position
            this.Load += new EventHandler(TreeNodeSelector_Load);

            //Add the nodes we're supposed to view.
            treeView.Nodes.AddRange(nodes);
        }

        void TreeNodeSelector_Load(object sender, EventArgs e)
        {
            if (lastSize != Size.Empty) this.Size = lastSize;
            if (lastLocation != Point.Empty) this.Location = lastLocation;
        }

        void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            this.selectedItemLabel.Text = ((RAFInMemoryFileSystemObject)e.Node).GetRAFPath();
            this.selectedItemLabel.SelectionStart = this.selectedItemLabel.Text.Length;
        }

        void TreeNodeSelector_FormClosing(object sender, FormClosingEventArgs e)
        {
            lastSize = this.Size;
            lastLocation = this.Location;
        }
        private void ManageLayout()
        {
            bigContainer.SplitterDistance = this.ClientSize.Height - this.doneButton.Height;
            this.doneButton.Top = 0;
            this.doneButton.Left = this.bigContainer.Panel2.Width - this.doneButton.Width;
            this.selectedItemLabel.Left = 0;
            this.selectedItemLabel.Top = 0;
            this.selectedItemLabel.Width = this.ClientSize.Width - this.doneButton.Width;
            this.selectedItemLabel.ReadOnly = true;
        }
        public string SelectedNodePath
        {
            get
            {
                if (this.treeView.SelectedNode != null)
                    return ((RAFInMemoryFileSystemObject)this.treeView.SelectedNode).GetRAFPath(true);
                else return null;
            }
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
