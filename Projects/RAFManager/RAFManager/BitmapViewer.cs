using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace RAFManager
{
    public partial class BitmapViewer : Form
    {
        public BitmapViewer(string fileName, byte[] content)
        {
            InitializeComponent();

            this.Text = "RAF Manager - Viewing Bitmap: " + fileName;

            MemoryStream ms = new MemoryStream(content);
            pictureBox1.Image = Bitmap.FromStream(ms);
            this.Size = pictureBox1.Image.Size;
        }
    }
}
