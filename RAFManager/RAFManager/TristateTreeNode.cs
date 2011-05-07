using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

using ItzWarty;

namespace RAFManager
{
    public enum TristateTreeNodeState:byte
    {
        Empty     = 0x00,
        Checked   = 0x01,
        Unchecked = 0x02,
        Partial   = 0x04,
        Disabled  = 0x08

    }
    public class TristateTreeNode
    {
        private string text = "";
        private TristateTreeNodeCollection nodes;
        private TristateTreeNodeState checkboxState = TristateTreeNodeState.Unchecked;
        private int tabWidth = 40; //How much we shift to the right when we
                                   //go down to a childnode for rendering
        private TristateTreeNode parent = null;

        public TristateTreeNode()
        {
        }
        public TristateTreeNode(string text)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public Object Parent
        {
            get
            {
                return this.parent;
            }
            set
            {
                if (!parent.nodes.Contains(this))
                    parent.nodes.Add(this);
                this.parent = value;
            }
        }

        private TristateTreeView TreeView
        {
            get
            {
                Object currentNode = this;
                int maxDepth = 100; //Maximum depth before we just throw an exception
                for (int i = 0; i < maxDepth; i++)
                {
                    if (currentNode is TristateTreeView) return (TristateTreeView)currentNode;
                    else //it's a tristatetreenode
                        currentNode = ((TristateTreeNode)currentNode).Parent;
                }
                throw new Exception("TreeNode couldn't find parent after depth of {0} search".F(maxDepth));
            }
        }

        private Font GetFont()
        {
            Object currentNode = this;
            int maxDepth = 100; //Maximum depth before we just throw an exception
            for (int i = 0; i < maxDepth; i++)
            {
                if(currentNode is TristateTreeView)
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="location"></param>
        /// <returns>New drawing offset (bottom left)</returns>
        public Point Draw(Graphics g, Point location)
        {
            g.DrawImage(GetCheckboxBitmap(), new Rectangle(location, new Size(20, 20)));
            g.DrawString(this.text, 
                         this.font, 
                         Brushes.Black, 
                         new Point(
                             location.X + 20,
                             location.Y + (20 - (int)g.MeasureString(text, font).Height)/2
                         )
            );
            return GetDrawingOffset(g, location);
        }

        private Bitmap GetCheckboxBitmap()
        {
            if ((this.checkboxState & TristateTreeNodeState.Disabled) > 0)
            {
                return null; //TODO: Graphics
            }
            else
            {
                switch (this.checkboxState)
                {
                    case TristateTreeNodeState.Checked:
                        return Properties.Resources.Checkbox_Checked;
                    case TristateTreeNodeState.Partial:
                        return Properties.Resources.Checkbox_Partial;
                    case TristateTreeNodeState.Unchecked:
                        return Properties.Resources.Checkbox_Unchecked;
                    default:
                        return null; //?
                }
            }
        }

        /// <summary>
        /// Applys the drawing offset to the location, then returns it
        /// </summary>
        /// <param name="?"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public Point GetDrawingOffset(Graphics g, Point location)
        {
            return new Point(
                location.X,
                location.Y + Math.Max(g.MeasureString(text, font).ToSize().Height, 
                                      20)
            );
        }
        public Point GetBottomRightDrawingOffset(Graphics g, Point location)
        {
            Size size = Size; //lol
            return new Point(
                location.X + size.Width,
                location.Y + size.Height
            );
        }
        public Size Size
        {
            get
            {
                Size textSize = g.MeasureString(text, font).ToSize();
                return new Size(
                    20 + 2 + textSize.Width,
                    Math.Max(textSize.Height, 20)
                );
            }
        }

        /// <summary>
        /// Gets the size of this node, including its children
        /// </summary>
        public Size BigSize
        {
            get
            {
                Size textSize = g.MeasureString(text, font).ToSize();
                int width = 20 + 2 + textSize.Width;
                int height = 20;
                for (int i = 0; i < nodes.Count; i++)
                {
                    Size bigSize = nodes[i].BigSize;
                    height += bigSize.Height;
                    width = Math.Max(tabWidth + bigSize.Width, width);
                }
                return new Size(width, height);
            }
        }
        
        public List<TristateTreeNode> Nodes
        {
            get
            {
                return nodes;
            }
        }

        public TristateTreeNode GetNodeAtLocation(Point point)
        {
            if (new Rectangle(new Point(0, 0), Size).Contains(point)) return this;

            Point offset = this.GetDrawingOffset(g, new Point(tabWidth, 0));
            for (int i = 0; i < nodes.Count; i++)
            {
                TristateTreeNode node = nodes[i];
                if (new Rectangle(offset, node.Size).Contains(point))
                {
                    return node.GetNodeAtLocation(
                        new Point(
                            point.X - offset.X,
                            point.Y - offset.Y
                        )
                    );
                }
                offset = node.GetDrawingOffset(g, offset);
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Whether or not we are now selected</returns>
        public bool ProcessClick(Point point)
        {
            if (new Rectangle(0, 0, 20, 20).Contains(point))
            {
                //Checkbox click
                ToggleChecked();
                return true;
            }
            return false;
        }
        public void ToggleChecked()
        {
            if (this.checkboxState.HasFlag(TristateTreeNodeState.Checked))
                this.checkboxState = (this.checkboxState & ~TristateTreeNodeState.Checked) | TristateTreeNodeState.Unchecked;
            else if (this.checkboxState.HasFlag(TristateTreeNodeState.Unchecked))
                this.checkboxState = (this.checkboxState & ~TristateTreeNodeState.Unchecked) | TristateTreeNodeState.Checked;
            else if (this.checkboxState.HasFlag(TristateTreeNodeState.Partial))
                this.checkboxState = (this.checkboxState & ~TristateTreeNodeState.Partial) | TristateTreeNodeState.;
        }
        public void SetCheckState(TristateTreeNodeState state)
        {
            bool disabled = (this.checkboxState & TristateTreeNodeState.Disabled) > 0;
            TristateTreeNodeState flags = disabled ? TristateTreeNodeState.Disabled : TristateTreeNodeState.Empty;
            flags &= state;
        }
    }
}
