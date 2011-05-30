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
    }
}
