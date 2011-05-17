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
    public partial class AboutBox : Form
    {
        public AboutBox()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ItzWarty.com/RAF/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://sourceforge.net/projects/hexbox/");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bit.ly/kThoeF");
        }
        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://bit.ly/mKDFYs");
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/raf-manager/");
        }

    }
}
