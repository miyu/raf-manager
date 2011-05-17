using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SlimDX;
using SlimDX.Windows;

using SlimDX.Direct3D9;

using System.Drawing;

using System.Runtime.InteropServices;

using System.IO;

namespace DDS_Viewer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length == 0)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "DirectDraw Surface|*.dds";
                ofd.ShowDialog();
                if (ofd.FileName != "")
                    Application.Run(new TextureViewer(ofd.FileName));
            }else
                if(File.Exists(args[0]) && args[0].ToLower().EndsWith("dds"))
                    Application.Run(new TextureViewer(args[0]));
        }
    }
}
