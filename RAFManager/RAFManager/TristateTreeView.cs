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
    public partial class TristateTreeView : Control
    {
        private VScrollBar vscrollbar           = new VScrollBar();
        private List<TristateTreeNode> nodes    = null;
        private int tabWidth                    = 40; //How much we shift to the right when we
                                                      //go down to a childnode for rendering
        private Graphics g                      = null;
        public TristateTreeView()
        {
            InitializeComponent();
            this.vscrollbar.Dock = DockStyle.Right;

            this.g = this.CreateGraphics();

            this.nodes = new List<TristateTreeNode>();

            this.Paint += new PaintEventHandler(ProjectViewer_Paint);
            this.MouseDown += new MouseEventHandler(TristateTreeView_MouseDown);
            this.Controls.Add(vscrollbar);
        }

        private bool mousedown = false;
        private TristateTreeNode selectedNode = null;
        void TristateTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            mousedown = true;
            TristateTreeNode node = GetNodeAtLocation(new Point(e.X, e.Y));
            Console.WriteLine("Clicked: " + (node == null?"nothing":node.Text));
            //if (node != null)
            //    node.ProcessClick(e.X - e.Y);
        }

        private TristateTreeNode GetNodeAtLocation(Point point)
        {
            Point offset = new Point(5, 5);
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
            }
            return null;
        }

        private TristateTreeNode GetNodeLocation(TristateTreeNode search)
        {
            Point offset = new Point(5, 5);
            for (int i = 0; i < nodes.Count; i++)
            {
                TristateTreeNode node = nodes[i];
            }
            return null;
        }
        private 

        void ProjectViewer_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Clear(SystemColors.ControlLightLight);
            
            RenderNodes(nodes, g, new Point(0, 0));
        }
        private void RenderNodes(List<TristateTreeNode> nodes, Graphics g, Point offset)
        {   //The recursion shouldn't go so deep as to get a stackoverflow...
            for (int i = 0; i < nodes.Count; i++)
            {
                offset = nodes[i].Draw(g, offset);
                offset.X += tabWidth;
                RenderNodes(nodes[i].Nodes, g, offset);
                offset.X -= tabWidth;
            }
        }

        public List<TristateTreeNode> Nodes
        {
            get
            {
                return nodes;
            }
        }
    }
}
