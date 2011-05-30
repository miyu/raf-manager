using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Drawing;

namespace RAFManager
{
    public class BlackCustomRenderer : ToolStripProfessionalRenderer
    {
        public BlackCustomRenderer(ProfessionalColorTable colorTable):base(colorTable)
        {
        }
        //private SolidBrush normalBrush = new SolidBrush(Color.FromArgb(80, 80, 80));
        //private SolidBrush hoverBrush  = new SolidBrush(Color.FromArgb(96, 96, 96));
        private SolidBrush normalBrush = new SolidBrush(Color.FromArgb(80 - 32, 80 - 32, 80 - 32));
        private SolidBrush hoverBrush = new SolidBrush(Color.FromArgb(96 - 32, 96 - 32, 96 - 32));
        private SolidBrush textBrush = new SolidBrush(Color.Silver);
        private Pen borderPen = new Pen(Color.FromArgb(32, 32, 32));
        private Pen highlightPen = new Pen(Color.FromArgb(96, 96, 96));
        protected override void OnRenderItemBackground(ToolStripItemRenderEventArgs e)
        {
            Console.WriteLine("TODO ITEMBG");
        }
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            //background render
            e.Graphics.FillRectangle(normalBrush, new Rectangle(new Point(0, 0), e.AffectedBounds.Size));
        }
        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(hoverBrush, new Rectangle(new Point(0, 0), e.Item.Size));
                e.Graphics.DrawRectangle(highlightPen, new Rectangle(0, 0, e.Item.Width - 1, e.Item.Height - 1));
            }
            else
            {
                e.Graphics.FillRectangle(normalBrush, new Rectangle(new Point(0, 0), e.Item.Size));
                e.Graphics.DrawLine(highlightPen, 2, e.Item.Height - 4, e.Item.Width - 4, e.Item.Height - 4);
                e.Graphics.DrawLine(highlightPen, 2, e.Item.Height - 3, e.Item.Width - 4, e.Item.Height - 3);
            }
        }
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(hoverBrush, new Rectangle(new Point(0, 0), e.Item.Size));
            }
            else
                e.Graphics.FillRectangle(normalBrush, new Rectangle(new Point(0, 0), e.Item.Size));
        }
        protected override void OnRenderToolStripPanelBackground(ToolStripPanelRenderEventArgs e)
        {
            //Console.WriteLine("OnRenderToolStripPanelBackground");
            e.Graphics.FillRectangle(hoverBrush, new Rectangle(new Point(0, 0), e.ToolStripPanel.Size));            
        }
        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                e.Graphics.FillRectangle(hoverBrush, new Rectangle(new Point(0, 0), e.Item.Size));
                e.Graphics.DrawRectangle(highlightPen, new Rectangle(0, 0, e.Item.Width-1, e.Item.Height-1));
            }
            else
                e.Graphics.FillRectangle(normalBrush, new Rectangle(new Point(0, 0), e.Item.Size));
        }
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.Graphics.DrawString(
                e.Text,
                e.TextFont,
                textBrush,
                e.TextRectangle.Location
            );
        }
        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = textBrush.Color;
            base.OnRenderArrow(e);
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            int pad = 4;
            int offset = 2;
            e.Graphics.DrawLine(highlightPen, 0, pad + offset, 0, e.Item.Height - pad * 2 + offset);
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            //Console.WriteLine("OnRenderToolStripBorder");
            e.Graphics.DrawRectangle(
                borderPen, new Rectangle(0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1)
            );
        }
    }
}
