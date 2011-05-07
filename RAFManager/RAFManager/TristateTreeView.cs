using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAFManager
{
    public delegate void NodeClickedHandler(TristateTreeNode node, MouseEventArgs e);
    public delegate void NodeRightClickedHandler(TristateTreeNode node, MouseEventArgs e);
    public partial class TristateTreeView : Control
    {
        private VScrollBar vscrollbar           = new VScrollBar();
        private TristateTreeNodeCollection nodes = null;
        private int tabWidth                    = 40; //How much we shift to the right when we
                                                      //go down to a childnode for rendering
        private Graphics g                      = null;
        private Point startDrawOffset           = new Point(5, 5);
        private string emptyComment             = null;

        public event NodeClickedHandler NodeClicked = null;
        public event NodeRightClickedHandler NodeRightClicked = null;

        public TristateTreeView()
        {
            InitializeComponent();
            this.vscrollbar.Dock = DockStyle.Right;

            this.g = this.CreateGraphics();

            this.nodes = new TristateTreeNodeCollection(this);

            this.Paint += new PaintEventHandler(ProjectViewer_Paint);
            this.Resize += new EventHandler(TristateTreeView_Resize);
            this.MouseDown += new MouseEventHandler(TristateTreeView_MouseDown);
            this.MouseMove += new MouseEventHandler(TristateTreeView_MouseMove);

            this.vscrollbar.ValueChanged += new EventHandler(vscrollbar_ValueChanged);

            this.Controls.Add(vscrollbar);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
        }

        void vscrollbar_ValueChanged(object sender, EventArgs e)
        {
            OnPaint(null);
        }

        void TristateTreeView_Resize(object sender, EventArgs e)
        {
            SizeVScrollbar();
        }
        private void SizeVScrollbar()
        {
            //Console.WriteLine("TDH: " + this.TreeDrawingHeight);
            //Console.WriteLine("H: " + this.Height);
            if (this.TreeDrawingHeight < this.Height)
            {
                this.vscrollbar.Enabled = false;
                this.vscrollbar.Minimum = 0;
                this.vscrollbar.Maximum = 10;
                this.vscrollbar.Value = 2;
            }
            else
            {
                this.vscrollbar.Minimum = 0;
                this.vscrollbar.Maximum = this.TreeDrawingHeight - this.Height + 20; //HACK: 20 added

                this.vscrollbar.Enabled = true;
            }
        }

        private bool mousedown = false;
        private TristateTreeNode selectedNode = null;
        void TristateTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
            TristateTreeNode node = GetNodeAtLocation(new Point(e.X, e.Y + vscrollbar.Value));
            selectedNode = node;
            Console.WriteLine("Clicked: " + (node == null?"nothing":node.Text));
            if (node != null)
            {
                Point nodeLocation = node.GetLocation();
                Console.WriteLine("nLoc: " + nodeLocation);
                node.ProcessClick(
                     new Point(
                         e.X - nodeLocation.X,
                         e.Y - nodeLocation.Y + vscrollbar.Value
                     ), e
                );
                SizeVScrollbar();
                Invalidate();
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                if (NodeClicked != null) NodeClicked(selectedNode, e);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                if (NodeRightClicked != null) NodeRightClicked(selectedNode, e);

        }
        void TristateTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (mousedown && selectedNode != null) //Dragging a node
            {
                //TODO
            }
        }
        public TristateTreeNode SelectedNode
        {
            get
            {
                return this.selectedNode;
            }
            set
            {
                this.selectedNode = value;
            }
        }
        private TristateTreeNode GetNodeAtLocation(Point point)
        {
            Point offset = startDrawOffset;
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
                offset = node.GetDrawingOffset(g, offset);
                Console.WriteLine("OFF: " + offset);
            }
            return null;
        }

        public Graphics GetGraphics()
        {
            return this.g;
        }
        public Point StartDrawOffset
        {
            get
            {
                return startDrawOffset;
            }
        }
        public int TreeDrawingHeight
        {
            get
            {
                if (this.nodes.Count == 0) return 0;
                TristateTreeNode currentNode = this.nodes[this.nodes.Count - 1];
                while (currentNode.Nodes.Count != 0 && (currentNode.HasToggle ? currentNode.IsToggled : true))
                {
                    Console.WriteLine(currentNode.Nodes.Count);
                    currentNode = currentNode.Nodes[currentNode.Nodes.Count - 1];
                }
                return currentNode.GetLocation().Y + currentNode.BigSize.Height; //HACK
            }
        }
        Bitmap backBuffer = null;
        private void ProjectViewer_Paint(object sender, PaintEventArgs e)
        {
            if (backBuffer == null) backBuffer = new Bitmap(this.Width, this.Height);
            else if (backBuffer.Width != this.Width || backBuffer.Height != this.Height)
            {
                backBuffer.Dispose();
                backBuffer = new Bitmap(this.Width, this.Height);
            }
            Graphics bbg = Graphics.FromImage(backBuffer);
            bbg.Clear(SystemColors.ControlLightLight);
            RenderNodes(nodes, bbg, 
                new Point(
                    startDrawOffset.X,
                    startDrawOffset.Y - vscrollbar.Value
                )
            );

            Graphics g = (e == null ? this.CreateGraphics() : e.Graphics);
            g.Clear(SystemColors.ControlLightLight);
            g.DrawImage(backBuffer, new Point(0, 0));
            //Console.WriteLine(this.Width);

            if (this.nodes.Count == 0)
                g.DrawString(emptyComment, Font, Brushes.Black, new Point(0, 0));
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="g"></param>
        /// <param name="offset"></param>
        /// <returns>Height of image</returns>
        private int RenderNodes(TristateTreeNodeCollection nodes, Graphics g, Point offset)
        {   //The recursion shouldn't go so deep as to get a stackoverflow...
            for (int i = 0; i < nodes.Count; i++)
            {
                offset = nodes[i].Draw(g, offset);
            }
            return offset.Y;
        }

        public TristateTreeNodeCollection Nodes
        {
            get
            {
                return nodes;
            }
        }
        public string EmptyComment
        {
            get
            {
                return emptyComment;
            }
            set
            {
                emptyComment = value;
                Invalidate();
            }
        }
    }
}
