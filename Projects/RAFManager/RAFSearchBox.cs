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
    public partial class RAFSearchBox : Form
    {
        RAFInMemoryFileSystemObject[] allNodes = null;
        RAFInMemoryFileSystemObject currentNode = null;
        List<RAFInMemoryFileSystemObject> ignore = new List<RAFInMemoryFileSystemObject>();
        public RAFSearchBox(RAFInMemoryFileSystemObject[] nodes)
        {
            InitializeComponent();

            allNodes = nodes;
            currentNode = nodes[0];
        }
        public RAFSearchBox(RAFInMemoryFileSystemObject node)
        {
            InitializeComponent();

            Text = Text + " (searching within '" + node.Text + "')";
            allNodes = new RAFInMemoryFileSystemObject[] { node };
            currentNode = node;
        }

        private void findNextButton_Click(object sender, EventArgs e)
        {
            List<RAFInMemoryFileSystemObject> result = null;
            List<RAFInMemoryFileSystemObject> pastNodes = new List<RAFInMemoryFileSystemObject>();
            while (result == null || result.Count == 0)
            {
                pastNodes.Add(currentNode);

                result = Search(stringSearch.Text, currentNode, ignore);
                if (result.Count == 0)
                {
                    if (currentNode.Parent == null) //End of raf archive
                    {
                        currentNode = allNodes[(1 + Array.IndexOf(allNodes, currentNode)) % allNodes.Length]; //Select next node
                        if (pastNodes.Contains(currentNode)) //Done
                        {
                            SearchOutOfOptions();
                            ignore.Clear();
                            return;
                        }
                    }
                    else
                    {
                        currentNode = (RAFInMemoryFileSystemObject)currentNode.Parent;
                    }
                }
            }
            //Find the first result and select it on the tree view (its the result), then ignore it in the future
            currentNode.TreeView.SelectedNode = result[0];
            currentNode.TreeView.Focus();
            ignore.Add(result[0]);
        }
        private List<RAFInMemoryFileSystemObject> Search(string phrase, RAFInMemoryFileSystemObject node, List<RAFInMemoryFileSystemObject> ignore)
        {
            phrase = phrase.ToLower();

            List<RAFInMemoryFileSystemObject> results = new List<RAFInMemoryFileSystemObject>();
            if (node.Text.ToLower().Contains(phrase))
                if (!ignore.Contains(node))
                    results.Add(node);

            for(int i = 0; i < node.Nodes.Count; i++)
                results.AddRange(Search(phrase, (RAFInMemoryFileSystemObject)node.Nodes[i], ignore));

            return results;
        }
        private void SearchOutOfOptions()
        {
            MessageBox.Show("The search has concluded, and another result couldn't be found.", "End of search");
        }

        private void findPreviousButton_Click(object sender, EventArgs e)
        {
            if (ignore.Count > 1)
            {
                ignore.RemoveAt(ignore.Count - 1);
                ignore.RemoveAt(ignore.Count - 1);
                findNextButton_Click(sender, e);
            }
            else
            {
                ignore.Clear();
                currentNode = allNodes[(-1 + allNodes.Length + Array.IndexOf(allNodes, currentNode)) % allNodes.Length]; //Select previous node
                findNextButton_Click(sender, e);
            }
        }
    }
}
