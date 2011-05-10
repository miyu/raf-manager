using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItzWarty;

using RAFLib;

using System.IO;

namespace RAFManager
{
    public partial class TextViewer : Form
    {
        private RAFFileListEntry fileEntry = null;
        private string defaultTitle = "";

        private static Point lastLocation = Point.Empty;
        private static Size lastSize = Size.Empty;

        /// <summary>
        /// Instantiates a TextViewer form which displays a string
        /// </summary>
        /// <param name="title">Title of the window</param>
        /// <param name="content">Content displayed</param>
        public TextViewer(RAFFileListEntry fileEntry)
        {
            InitializeComponent();
            this.fileEntry = fileEntry;
            string content = Encoding.ASCII.GetString(fileEntry.GetContent());

            this.Text = defaultTitle = "Text Viewer - " + fileEntry.FileName;
            this.contentTB.Text = content;

            this.Load += delegate(object sender, EventArgs e)
            {
                if (lastSize != Size.Empty) Size = lastSize;
                if (lastLocation != Point.Empty) Location = lastLocation;
            };

            this.FormClosing += new FormClosingEventHandler(TextViewer_FormClosing);
        }


        void TextViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            lastLocation = Location;
            lastSize = Size;
        }

        public TextViewer(string fileName, string content)
        {
            InitializeComponent();

            this.Text = defaultTitle = "(read-only) "+fileName;
            this.contentTB.Text = content;
            this.toolStrip1.Hide();
            this.contentTB.ReadOnly = true;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Text = "Inserting modified file...";
            fileEntry.FileSize = (UInt32)this.contentTB.Text.Length;
            fileEntry.RAFArchive.InsertFile(
                fileEntry.FileName,
                this.contentTB.Text.ToBytes(),
                Console.Out
            );
            fileEntry.RAFArchive.SaveDirectoryFile();
            this.Text = defaultTitle;
        }

        private void saveToDiskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            string fileName = fileEntry.FileName.Replace("\\", "/").Split("/").Last();
            string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
            sfd.Filter = "File|."+fileExt;
            sfd.FileName = fileName;
            sfd.ShowDialog();

            if (sfd.FileName != "")
            {
                File.WriteAllText(sfd.FileName, this.contentTB.Text);
            }
        }
    }
}
