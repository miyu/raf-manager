using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RAFLib;
namespace RAFManager
{
    public class ChangesViewEntry
    {
        private string localPath;
        private RAFLib.RAFFileListEntry entry;
        TristateTreeNode node;
        public ChangesViewEntry(string localPath, RAFFileListEntry entry, TristateTreeNode node)
        {
            this.localPath = localPath;
            this.entry = entry;
            this.node = node;
        }
        public RAFFileListEntry Entry { get { return entry; } set { entry = value; } }
        public string LocalPath { get { return localPath; } set { localPath = value; } }
        public bool Checked
        {
            get
            {
                return this.node.CheckState == TristateTreeNodeState.Checked;
            }
        }
    }
}
