using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace RAFManager
{
    partial class MainForm:Form
    {
        private string baseTitle = null;

        public void InitializeUtil()
        {
            this.baseTitle = this.Text;
        }
        /// <summary>
        /// Sets the title of our form
        /// </summary>
        /// <param name="s"></param>
        private void Title(string s) { this.Text = baseTitle + " - " + s; Application.DoEvents(); }

        /// <summary>
        /// Allows the user to pick a RAF file from the rafContentView control...
        /// 
        /// TODO: 5 second timer for stopping
        /// TODO: This needs to be made better.  it's pretty annoying to work with on the user-viewpoint
        /// </summary>
        /// <returns></returns>
        private string PickRafPath()
        {
            TreeNode[] nodes = new TreeNode[this.rafContentView.Nodes.Count];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = (TreeNode)this.rafContentView.Nodes[i].Clone();
            RAFPathSelector selectorDialog = new RAFPathSelector(nodes);
            selectorDialog.ShowDialog();
            return selectorDialog.SelectedNodePath;

            rafContentView.SelectedNode = null;

            string oldTitle = this.Text;
            while (rafContentView.SelectedNode == null)
            {
                Title("Double click a file from the raf browser.");
                Application.DoEvents();
            }
            this.Text = oldTitle;
            return ((RAFInMemoryFileSystemObject)rafContentView.SelectedNode).GetRAFPath();
        }
    }
}
