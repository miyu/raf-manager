using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

namespace RAFManager
{
    public class TreeViewStylizer
    {
        public static void Stylize(TreeView treeView)
        {
            treeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
            treeView.DrawNode += myTreeView_DrawNode;
        }
        // Create a Font object for the node tags.
        //TODO: Have code that isn't copied from MSDN...
        private static void myTreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            // Draw the background and node text for a selected node.
            if ((e.State & TreeNodeStates.Selected) != 0)
            {
                Rectangle rect = NodeBounds(e.Node);
                rect.Width += 5;
                e.Graphics.FillRectangle(Brushes.Gray, rect);

                Font nodeFont = e.Node.TreeView.Font;
                //if (nodeFont == null) nodeFont = ((TreeView)sender).Font;

                Rectangle bounds = NodeBounds(e.Node);
                bounds.Width += 10;
                e.Graphics.DrawString(e.Node.Text, nodeFont, Brushes.White, bounds);

                e.DrawDefault = false;
            }

            // Use the default background and node text.
            else
            {
                e.DrawDefault = true;
            }

            /*
            // If a node tag is present, draw its string representation 
            // to the right of the label text.
            if (e.Node.Tag != null)
            {
                e.Graphics.DrawString(e.Node.Tag.ToString(), e.Node.TreeView.Font,
                    Brushes.Yellow, e.Bounds.Right + 2, e.Bounds.Top);
            }

            // If the node has focus, draw the focus rectangle large, making
            // it large enough to include the text of the node tag, if present.
            if ((e.State & TreeNodeStates.Focused) != 0)
            {
                using (Pen focusPen = new Pen(Color.Black))
                {
                    focusPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;
                    Rectangle focusBounds = NodeBounds(e.Node);
                    focusBounds.Size = new Size(focusBounds.Width - 1,
                    focusBounds.Height - 1);
                    e.Graphics.DrawRectangle(focusPen, focusBounds);
                }
            }*/
        }

        // Returns the bounds of the specified node, including the region 
        // occupied by the node label and any node tag displayed.
        private static Rectangle NodeBounds(TreeNode node)
        {
            // Set the return value to the normal node bounds.
            Rectangle bounds = node.Bounds;
            if (node.Tag != null)
            {
                // Retrieve a Graphics object from the TreeView handle
                // and use it to calculate the display width of the tag.
                Graphics g = node.TreeView.CreateGraphics();
                int tagWidth = (int)g.MeasureString
                    (node.Tag.ToString(), node.TreeView.Font).Width + 6;

                // Adjust the node bounds using the calculated value.
                bounds.Offset(tagWidth / 2, 0);
                bounds = Rectangle.Inflate(bounds, tagWidth / 2, 0);
                g.Dispose();
            }

            return bounds;

        }
    }
}
