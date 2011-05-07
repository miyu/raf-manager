using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFManager
{
    public class TristateTreeNodeCollection
    {
        private List<TristateTreeNode> nodes;
        private Object owner;
        public TristateTreeNodeCollection(Object owner)
        {
            this.owner = owner;
            nodes = new List<TristateTreeNode>();
        }
        public void Add(TristateTreeNode node)
        {
            nodes.Add(node);
            node.Parent = owner;

            if (owner is TristateTreeView)
                ((TristateTreeView)owner).Invalidate();
            else
                if(owner != null && ((TristateTreeNode)owner).TreeView != null)
                    ((TristateTreeNode)owner).TreeView.Invalidate();
        }
        public void Remove(TristateTreeNode node)
        {
            nodes.Remove(node);
            node.Parent = null;

            if (owner is TristateTreeView)
                ((TristateTreeView)owner).Invalidate();
            else
                if (owner != null && ((TristateTreeNode)owner).TreeView != null)
                    ((TristateTreeNode)owner).TreeView.Invalidate();
        }
        public void Clear()
        {
            nodes.Clear();
            if (owner is TristateTreeView)
                ((TristateTreeView)owner).Invalidate();
            else
                if (owner != null && ((TristateTreeNode)owner).TreeView != null)
                    ((TristateTreeNode)owner).TreeView.Invalidate();
        }
        public int IndexOf(TristateTreeNode node)
        {
            return this.nodes.IndexOf(node);
        }
        public TristateTreeNode this[int index]
        {
            get
            {
                return nodes[index];
            }
            set
            {
                nodes[index] = value;
            }
        }
        public int Count
        {
            get
            {
                return this.nodes.Count;
            }
        }
        public bool Contains(TristateTreeNode node)
        {
            return this.nodes.Contains(node);
        }
    }
}
