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
    public static class wGUI
    {
        static Int32 ticks = 0;
        public static void InitializeButton(PictureBox pb, Bitmap normal, Bitmap highlight, Bitmap down, bool willChangeImages)
        {
            Int32 myTick = ticks++;
            pb.Image = normal;

            if (willChangeImages)
                pb.Tag = myTick;
            pb.MouseHover += delegate(object sender, EventArgs e)
            {
                if (pb.Tag is Int32 && (int)pb.Tag != myTick) return;
                pb.Image = highlight;
            };
            pb.MouseLeave += delegate(object sender, EventArgs e)
            {
                if (pb.Tag is Int32 && (int)pb.Tag != myTick) return;
                pb.Image = normal;
            };
            pb.MouseDown += delegate(object sender, MouseEventArgs e)
            {
                if (pb.Tag is Int32 && (int)pb.Tag != myTick) return;
                pb.Image = down;
            };
            pb.MouseUp += delegate(object sender, MouseEventArgs e)
            {
                if (pb.Tag is Int32 && (int)pb.Tag != myTick) return;
                pb.Image = highlight;
            };
        }
        public static PointF GetContentPosition(ContentAlignment alignment, SizeF size, SizeF containerSize)
        {
            return new PointF(
                GetContentPositionX(alignment, size, containerSize),
                GetContentPositionY(alignment, size, containerSize)
            );
        }
        public static float GetContentPositionY(ContentAlignment alignment, SizeF contentSize, SizeF containerSize)
        {
            switch (alignment)
            {
                case ContentAlignment.TopLeft:
                case ContentAlignment.TopCenter:
                case ContentAlignment.TopRight:
                    return 2;
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.MiddleRight:
                    return (containerSize.Height - contentSize.Height) / 2;
                case ContentAlignment.BottomLeft:
                case ContentAlignment.BottomCenter:
                case ContentAlignment.BottomRight:
                    return containerSize.Height - contentSize.Height - 2;
                default:
                    return 2; //wat
            }
        }
        public static float GetContentPositionX(ContentAlignment alignment, SizeF contentSize, SizeF containerSize)
        {
            switch (alignment)
            {
                case ContentAlignment.BottomLeft:
                case ContentAlignment.MiddleLeft:
                case ContentAlignment.TopLeft:
                    return 2;
                case ContentAlignment.BottomCenter:
                case ContentAlignment.MiddleCenter:
                case ContentAlignment.TopCenter:
                    return (containerSize.Width - contentSize.Width) / 2;
                case ContentAlignment.BottomRight:
                case ContentAlignment.MiddleRight:
                case ContentAlignment.TopRight:
                    return containerSize.Width - contentSize.Width - 2;
                default:
                    return 2; //wat
            }
        }
    }
}
