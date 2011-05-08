using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItzWarty;

namespace RAFManager
{
    public partial class MainWindowLoading : Form
    {
        public MainWindowLoading()
        {
            InitializeComponent();

            headerLabel.Text = headerLabel.Text.F(RAFManagerUpdater.Versioning.CurrentVersion.GetVersionString());
            statusLabel.Text = "Opened Loading Screen";

            this.ControlBox = false;
        }

        public void Log(string s)
        {
            List<string> lines = new List<string>(statusLabel.Text.Split("\r\n"));
            lines.Add(s);
            while (lines.Count > 6) lines.RemoveAt(0);
            statusLabel.Text = String.Join("\r\n", lines.ToArray());

            Application.DoEvents();
        }
    }
}
