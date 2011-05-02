using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItzWarty;

using System.IO;

using Be.Windows.Forms;

using RAFLib;

namespace RAF_Packer
{
    public partial class MainForm : Form
    {
        //TODO: Configuration file
        private string archivesRoot = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";
        private string[] archives = null;
        private Dictionary<string, RAFArchive> rafArchives = new Dictionary<string, RAFArchive>();
        private string baseTitle = null;
        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);
            this.Resize += new EventHandler(MainForm_Resize);
            this.baseTitle = this.Text;
        }

        void MainForm_Resize(object sender, EventArgs e)
        {
            this.logContainer.Top = this.ClientSize.Height - this.logContainer.Height;
            this.logContainer.Width = this.ClientSize.Width - this.logContainer.Left * 2;

            this.rafContentView.Width = this.logContainer.Width;
            this.rafContentView.Height = this.logContainer.Top - this.rafContentView.Top;
        }

        private void Title(string s) { this.Text = baseTitle + " - " + s; Application.DoEvents(); }
        void MainForm_Load(object sender, EventArgs e)
        {
            this.Show();
            Title("Loading RAF Files - ");
            

            this.Load += new EventHandler(MainForm_Load);
            log.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker 30-April-2011 4:34pm build";

            //Enumerate RAF files
            archives = Directory.GetDirectories(archivesRoot);
            for (int i = 0; i < archives.Length; i++)
            {
                string archiveName = archives[i].Replace(archivesRoot, "");
                Title("Loading RAF Files - " + archiveName);
                Log("Loading RAF Archive Folder: " + archives[i]);
                RAFArchive raf = null;
                RAFInMemoryFileSystemObject archiveRoot = new RAFInMemoryFileSystemObject(null, RAFFSOType.ARCHIVE, archiveName);
                try
                {
                    rafArchives.Add(archiveName,
                        raf = new RAFArchive(
                            Directory.GetFiles(archives[i], "*.raf")[0]
                        )
                    );

                    List<RAFFileListEntry> entries = raf.GetDirectoryFile().GetFileList().GetFileEntries();
                    for (int j = 0; j < entries.Count; j++)
                    {
                        if (j % 100 == 0)
                            Title("Loading RAF Files - " + archiveName +" - " + j+"/"+entries.Count);

                        RAFInMemoryFileSystemObject node = archiveRoot.AddToTree(RAFFSOType.FILE, entries[j].FileName);
                        //Log(entries[j].GetFileName);
                    }
                    //foreach (RAFFileListEntry entry in entries)
                    //    Log(entry.GetFileName);
                    Log(entries.Count.ToString() + " Files");
                }
                catch { Log("FAILED"); }

                rafContentView.Nodes.Add(archiveRoot);
            }
            rafContentView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(rafContentView_NodeMouseDoubleClick);

            //Open RAFs
        }

        void rafContentView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RAFInMemoryFileSystemObject node = (RAFInMemoryFileSystemObject)e.Node;
            string nodeInternalPath = node.GetRAFPath();
            if (node.GetFSOType() == RAFFSOType.FILE)
            {
                List<RAFFileListEntry> entries = this.rafArchives[node.GetTopmostParent().Name]
                    .GetDirectoryFile().GetFileList().GetFileEntries();

                RAFFileListEntry entry = entries.Where(
                    (Func<RAFFileListEntry, bool>)delegate(RAFFileListEntry theEntry)
                    {
                        return theEntry.FileName == nodeInternalPath;
                    }
                ).First();
                if (entry.FileName.ToLower().EndsWith("inibin") || entry.FileName.ToLower().EndsWith("troybin"))
                {
                    new TextViewer(this.baseTitle + " - inibin/troybin view - " + nodeInternalPath,
                        new InibinFile().main(entry.GetContent())
                    ).Show();
                }
                else if (entry.FileSize < 10000 || //If > 200, ask, then continue
                       MessageBox.Show("This file is quite large ({0} bytes).  Sure you want to read it?".F(entry.FileSize), "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (entry.GetContent().All(c => c >= ' ' && c <= '~')) //All content is ascii
                    {
                        new TextViewer(this.baseTitle + " - Text View - " + nodeInternalPath,
                            Encoding.ASCII.GetString(entry.GetContent())
                        ).Show();
                    }
                    else
                    {
                        new BinaryViewer(this.baseTitle + " - Binary View by Be.HexEditor http://sourceforge.net/projects/hexbox/- " + nodeInternalPath,
                            entry.GetContent()
                        ).Show();
                        MessageBox.Show(entry.FileSize.ToString());
                    }
                }
            }
        }
        private void Log(string s)
        {
            log.Text += "\r\n" + s;
            log.SelectionLength = 0; log.SelectionStart = 0;
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }
    }
}
