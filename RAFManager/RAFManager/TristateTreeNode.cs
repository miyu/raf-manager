using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;

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
    public enum TristateTreeNodeType : byte
    {
        Radio       = 0x01,
        Checkboxes  = 0x02
    }
    /// <summary>
    /// Lots of optimization has to be done here...
    /// </summary>
    public class TristateTreeNode
    {
        private string text = "";
        private TristateTreeNodeCollection nodes;
        private TristateTreeNodeState checkboxState = TristateTreeNodeState.Unchecked;

        /// <summary>
        /// Note that we look at ourselves first, then our parent to see what type we are...
        /// </summary>
        private TristateTreeNodeType nodeType = TristateTreeNodeType.Checkboxes;
        private int tabWidth = 40; //How much we shift to the right when we
                                   //go down to a childnode for rendering
        private Object parent = null;

        public TristateTreeNode()
        {
            nodes = new TristateTreeNodeCollection(this);
        }
        public TristateTreeNode(string text)
        {
            this.text = text;
            nodes = new TristateTreeNodeCollection(this);
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
                if (value is TristateTreeNode)
                {
                    TristateTreeNode p = (TristateTreeNode)value;
                    if (!p.nodes.Contains(this))
                        p.nodes.Add(this);
                    this.parent = value;
                }
                else if(value is TristateTreeView)
                {
                    TristateTreeView p = (TristateTreeView)value;
                    if (!p.Nodes.Contains(this))
                        p.Nodes.Add(this);
                    this.parent = value;
                }
            }
        }

        public TristateTreeView TreeView
        {
            get
            {
                Object currentNode = this;
                int maxDepth = 100; //Maximum depth before we just throw an exception
                for (int i = 0; i < maxDepth && currentNode != null; i++)
                {
                    if (currentNode is TristateTreeView) return (TristateTreeView)currentNode;
                    else //it's a tristatetreenode
                        currentNode = ((TristateTreeNode)currentNode).Parent;
                }
                return null;
                //throw new Exception("TreeNode couldn't find parent after depth of {0} search".F(maxDepth));
            }
        }

        private Font GetFont()
        {
            return this.TreeView.Font;
        }
        private Graphics GetGraphics()
        {
            return this.TreeView.GetGraphics();
        }
        public Point GetLocation()
        {
            Object currentNode = this.parent;
            int offsetX = 0;
            int offsetY = 0;
            int depth   = 0; //How many levels/tabs in are we? 
            while (currentNode is TristateTreeNode)
            {
                TristateTreeNode cNode = (TristateTreeNode)currentNode;
                offsetY += cNode.Size.Height;
                //Console.WriteLine(offsetY);
                for (int i = 0; i < cNode.nodes.Count; i++)
                {
                    if (!cNode.nodes[i].Contains(this) && cNode.nodes[i] != this) //Not our owner, just add its height
                    {
                        //Console.WriteLine(cNode.text + " " + cNode.nodes[i].BigSize.Height);
                        //Console.WriteLine("add height: " + cNode.nodes[i].BigSize.Height);
                        offsetY += cNode.nodes[i].BigSize.Height;
                    }
                    else //We found our owner, don't add offset, since we've already done so earlier
                    {
                        i = cNode.nodes.Count; //Done.
                    }
                }
                //Console.WriteLine(offsetY);
                depth++;
                currentNode = cNode.parent;
            }
            //curentNode is now a tristatetreeview
            TristateTreeView view = (TristateTreeView)currentNode;
            bool done = false;
            for (int i = 0; i < view.Nodes.Count && !done; i++)
                if (view.Nodes[i].Contains(this) || view.Nodes[i] == this)
                    done = true;
                else
                    offsetY += view.Nodes[i].BigSize.Height;
            return new Point(tabWidth * depth + ((TristateTreeView)currentNode).StartDrawOffset.X, offsetY);
        }
        public bool Contains(TristateTreeNode node)
        {
            bool result = nodes.Contains(node);
            for (int i = 0; i < nodes.Count; i++)
            {
                result |= nodes[i].Contains(node);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="g"></param>
        /// <param name="location"></param>
        /// <returns>New drawing offset (bottom left)</returns>
        public Point Draw(Graphics g, Point location)
        {
            if (HasToggle)
            {
                //Draw lines linking to parent
                if (this.parent is TristateTreeNode)
                {
                    Pen pen = new Pen(Brushes.LightGray);
                    pen.DashStyle = DashStyle.Dash;
                    TristateTreeNodeCollection parentCNs = null;
                    if (this.parent is TristateTreeNode) parentCNs = ((TristateTreeNode)parent).nodes;
                    else parentCNs = ((TristateTreeView)parent).Nodes;

                    int offset = 20;
                    for (int i = 0; i < parentCNs.IndexOf(this); i++)
                        offset += parentCNs[i].BigSize.Height;
                    g.DrawLine(pen, location.X + 10, location.Y + 10, location.X - tabWidth + 10, location.Y + 10);
                    //Vline up
                    g.DrawLine(pen, location.X - tabWidth + 10, location.Y + 10, location.X - tabWidth + 10, location.Y - offset + 10);
                }

                //Draw actual element
                if (location.Y > -20 && location.Y < TreeView.Height + 20)
                {
                    if (HasCheckBox)
                    {
                        Point stringLocation = new Point(
                            location.X + 20 + 19,
                            location.Y + (20 - (int)g.MeasureString(text, this.GetFont()).Height) / 2
                        );
                        g.DrawImage(GetToggleBitmap(), new Rectangle(location, new Size(19, 20)));
                        g.DrawImage(GetCheckboxBitmap(), new Rectangle(new Point(location.X + 19, location.Y), new Size(20, 20)));

                        if (this.TreeView.SelectedNodes.Contains(this))
                        {
                            g.FillRectangle(Brushes.LightGray, new Rectangle(stringLocation, g.MeasureString(this.text, this.GetFont()).ToSize()));
                        }
                        g.DrawString(this.text,
                                     this.GetFont(),
                                     Brushes.Black,
                                     stringLocation
                        );
                    }
                    else
                    {
                        Point stringLocation = new Point(
                            location.X + 19,
                            location.Y + (20 - (int)g.MeasureString(text, this.GetFont()).Height) / 2
                        );
                        g.DrawImage(GetToggleBitmap(), new Rectangle(location, new Size(19, 20)));

                        if (this.TreeView.SelectedNode == this)
                        {
                            g.FillRectangle(Brushes.LightGray, new Rectangle(stringLocation, g.MeasureString(this.text, this.GetFont()).ToSize()));
                        }
                        g.DrawString(this.text,
                                     this.GetFont(),
                                     Brushes.Black,
                                     stringLocation
                        );
                    }
                }
                //Draw toggle
                if (toggle)
                {
                    Point offset = new Point(
                        location.X + tabWidth,
                        location.Y + this.Size.Height
                    );
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        if(offset.Y < TreeView.Height)
                            offset = nodes[i].Draw(g, offset);
                    }
                    //RenderNodes(nodes[i].Nodes, g, offset);
                    //offset.X -= tabWidth;
                }
                return GetDrawingOffset(g, location);
            }
            else
            {
                //Draw lines linking to parent
                if (this.parent is TristateTreeNode)
                {
                    Pen pen = new Pen(Brushes.LightGray);
                    pen.DashStyle = DashStyle.Dash;
                    TristateTreeNodeCollection parentCNs = null;
                    if (this.parent is TristateTreeNode) parentCNs = ((TristateTreeNode)parent).nodes;
                    else parentCNs = ((TristateTreeView)parent).Nodes;

                    int offset = 20;
                    for (int i = 0; i < parentCNs.IndexOf(this); i++)
                        offset += parentCNs[i].BigSize.Height;
                    g.DrawLine(pen, location.X + 10, location.Y + 10, location.X - tabWidth + 10, location.Y + 10);
                    //Vline up
                    g.DrawLine(pen, location.X - tabWidth + 10, location.Y + 10, location.X - tabWidth + 10, location.Y - offset + 10);
                }


                if (HasCheckBox)
                {
                    Point stringLocation = new Point(
                        location.X + 19 + 20,
                        location.Y + (20 - (int)g.MeasureString(text, this.GetFont()).Height) / 2
                    );
                    g.DrawImage(GetCheckboxBitmap(), new Rectangle(
                        new Point(location.X + 19, location.Y), new Size(20, 20)));
                    if (this.TreeView.SelectedNode == this)
                    {
                        g.FillRectangle(Brushes.LightGray, new Rectangle(stringLocation, g.MeasureString(this.text, this.GetFont()).ToSize()));
                    }
                    g.DrawString(this.text,
                                 this.GetFont(),
                                 Brushes.Black,
                                 stringLocation
                    );
                }
                else
                {
                    Point stringLocation = new Point(
                        location.X + 19 + 20,
                        location.Y + (20 - (int)g.MeasureString(text, this.GetFont()).Height) / 2
                    );
                    if (this.TreeView.SelectedNode == this)
                    {
                        g.FillRectangle(Brushes.LightGray, new Rectangle(stringLocation, g.MeasureString(this.text, this.GetFont()).ToSize()));
                    }
                    g.DrawString(this.text,
                                 this.GetFont(),
                                 Brushes.Black,
                                 stringLocation
                    );
                }
                return GetDrawingOffset(g, location);
            }
        }

        private Bitmap GetToggleBitmap()
        {
            if (toggle)
                return Properties.Resources.CheckboxToggleEnable;
            else
                return Properties.Resources.CheckboxToggleDisable;
        }
        private Bitmap GetCheckboxBitmap()
        {
            if ((this.checkboxState & TristateTreeNodeState.Disabled) > 0)
            {
                return null; //TODO: Graphics
            }
            else
            {
                TristateTreeNodeType nType = this.nodeType;
                if (this.parent is TristateTreeNode)
                    nType = ((TristateTreeNode)parent).nodeType;
                if(nType == TristateTreeNodeType.Checkboxes)
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
                else // if (nType == TristateTreeNodeType.Radio)
                    switch (this.checkboxState)
                    {
                        case TristateTreeNodeState.Checked:
                            return Properties.Resources.Radio_Checked;
                        case TristateTreeNodeState.Partial:
                            return null; //lol?
                        case TristateTreeNodeState.Unchecked:
                            return Properties.Resources.Radio_Unchecked;
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
            Point result = new Point(
                location.X,
                location.Y + Math.Max(g.MeasureString(text, GetFont()).ToSize().Height, 
                                      20)
            );
            if(HasToggle && toggle)
                for(int i = 0; i < this.nodes.Count; i++)
                {
                    result = this.nodes[i].GetDrawingOffset(g, result);
                }
            return result;
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
                Size textSize = GetGraphics().MeasureString(text, GetFont()).ToSize();
                return new Size(
                    20 + 19 + 2 + textSize.Width,
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
                Size textSize = GetGraphics().MeasureString(text, GetFont()).ToSize();
                int width = 20 + 19 + 2 + textSize.Width;
                int height = 20;
                if(HasToggle && toggle)
                    for (int i = 0; i < nodes.Count; i++)
                    {
                        Size bigSize = nodes[i].BigSize;
                        height += bigSize.Height;
                        width = Math.Max(tabWidth + bigSize.Width, width);
                    }
                return new Size(width, height);
            }
        }

        public TristateTreeNodeCollection Nodes
        {
            get
            {
                return this.nodes;
            }
        }

        public TristateTreeNode GetNodeAtLocation(Point point)
        {
            if (new Rectangle(new Point(0, 0), Size).Contains(point)) return this;

            Point offset = new Point(tabWidth, this.Size.Height);
            Console.WriteLine("co: " + offset);
            for (int i = 0; i < nodes.Count; i++)
            {
                TristateTreeNode node = nodes[i];
                if (new Rectangle(offset, node.BigSize).Contains(point))
                {
                    return node.GetNodeAtLocation(
                        new Point(
                            point.X - offset.X,
                            point.Y - offset.Y
                        )
                    );
                }
                offset = node.GetDrawingOffset(GetGraphics(), offset);
                Console.WriteLine("co: " + offset);
            }
            return null;
        }
        public bool HasToggle
        {
            get
            {
                return this.nodes.Count > 0;
            }
        }
        public bool IsToggled
        {
            get
            {
                return toggle;
            }
        }
        private bool toggle = false;
        public bool HasCheckBox
        {
            get
            {
                return this.hasCheckBox;
            }
            set
            {
                this.hasCheckBox = value;
            }
        }
        private bool hasCheckBox = false;

        /// <summary>
        /// 
        /// </summary>
        public void ProcessClick(Point point, MouseEventArgs e)
        {
            Console.WriteLine("PC: " + point);
            if (e.Button == MouseButtons.Left)
            {
                if (HasToggle)
                {
                    if (new Rectangle(2, 2, 15, 16).Contains(point))
                    {   //Toggle click
                        Toggle();
                    }
                    if (HasCheckBox && new Rectangle(19, 0, 20, 20).Contains(point))
                    {
                        //Checkbox click
                        ToggleChecked();
                    }
                }
                else
                {
                    if (HasCheckBox && new Rectangle(19, 0, 20, 20).Contains(point))
                    {
                        //Checkbox click
                        ToggleChecked();
                    }
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
            }
        }
        public void Toggle()
        {
            toggle = !toggle;
        }
        public void ToggleChecked()
        {
            if (this.checkboxState.HasFlag(TristateTreeNodeState.Checked))
                SetCheckState(TristateTreeNodeState.Unchecked, true, true);
            else if (this.checkboxState.HasFlag(TristateTreeNodeState.Unchecked))
                SetCheckState(TristateTreeNodeState.Checked, true, true);
            else if (this.checkboxState.HasFlag(TristateTreeNodeState.Partial))
                SetCheckState(TristateTreeNodeState.Checked, true, true);
        }
        private void ModifyChildState()
        {
            if ((this.checkboxState & TristateTreeNodeState.Checked) > 0) //we be checked
            {
                for (int i = 0; i < this.nodes.Count; i++)
                    this.nodes[i].SetCheckState(TristateTreeNodeState.Checked, true, false);
            }
            else if ((this.checkboxState & TristateTreeNodeState.Unchecked) > 0) //we be checked
                for (int i = 0; i < this.nodes.Count; i++)
                    this.nodes[i].SetCheckState(TristateTreeNodeState.Unchecked, true, false);
        }
        public void UpdateCheckState(bool bubbleToChildren, bool bubbleToParent)
        {
            if (this.nodes.Count == 0) return;
            bool allChecked = true;
            bool allEmpty = true;
            for (int i = 0; i < this.nodes.Count; i++)
            {
                TristateTreeNodeState state = this.nodes[i].CheckState;
                if (state == TristateTreeNodeState.Checked)
                    allEmpty = false;
                else if (state == TristateTreeNodeState.Unchecked)
                    allChecked = false;
                else //partial
                {
                    allEmpty = false;
                    allChecked = false;
                }
            }
            if (allChecked) SetCheckState(TristateTreeNodeState.Checked, bubbleToChildren, bubbleToParent);
            else if (allEmpty) SetCheckState(TristateTreeNodeState.Unchecked, bubbleToChildren, bubbleToParent);
            else SetCheckState(TristateTreeNodeState.Partial, bubbleToChildren, bubbleToParent);
        }
        public void SetCheckState(TristateTreeNodeState state, bool bubbleToChildren, bool bubbleToParent)
        {
            bool disabled = (this.checkboxState & TristateTreeNodeState.Disabled) > 0;
            TristateTreeNodeState flags = disabled ? TristateTreeNodeState.Disabled : TristateTreeNodeState.Empty;
            flags |= state;
            this.checkboxState = flags;

            if(bubbleToChildren)
                ModifyChildState();

            //Update parent
            if (this.parent is TristateTreeNode && bubbleToParent)
                ((TristateTreeNode)this.parent).UpdateCheckState(false, true);
        }
        public TristateTreeNodeState CheckState
        {
            get
            {
                UpdateCheckState(false, false);
                return this.checkboxState & ~TristateTreeNodeState.Disabled;
            }
            set
            {
                SetCheckState(value & ~TristateTreeNodeState.Disabled, true, true);
            }
        }
        private object tag;
        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }
    }
}
