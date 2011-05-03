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
        //TODO: Configuration file to set this stuff
        //And a pretty editor.
        private string archivesRoot = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";

        //Names of our archives
        private string[] archives = null;
        private Dictionary<string, RAFArchive> rafArchives = new Dictionary<string, RAFArchive>();
        private string baseTitle = null;

        public MainForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(MainForm_Load);

            int bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
            int smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
            this.ResizeBegin += delegate(object sender, EventArgs e)
            {
                bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
                smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
            };
            this.Resize += delegate(object sender, EventArgs e){
                bigContainer.SplitterDistance = bigContainer.Height - bigSplitterDistanceFromBottom;
                smallContainer.SplitterDistance = smallSplitterDistanceFromLeft;
                AdjustModificationsView();
            };
            InitializeChangesView();

            this.baseTitle = this.Text;
        }

        //TODO: This needs to be made better.  it's pretty annoying to work with on the user-viewpoint
        private string PickRafPath()
        {
            rafContentView.SelectedNode = null;

            string oldTitle = this.Text;
            while (rafContentView.SelectedNode == null)
            {
                Title("Double click a file from the raf browser.");
                Application.DoEvents();
            }
            this.Text = oldTitle;
            return ((RAFInMemoryFileSystemObject)rafContentView.SelectedNode).GetRAFPath();
        }
        private void AdjustModificationsView()
        {
            changesView.Columns[0].Width = 50;
            changesView.Columns[1].Width = (changesView.Width - 110 - 20) / 2;
            changesView.Columns[2].Width = 30;
            changesView.Columns[3].Width = (changesView.Width - 110 - 20) / 2;
            changesView.Columns[4].Width = 30;
            changesView.ScrollBars = ScrollBars.Vertical;
        }
        private void Title(string s) { this.Text = baseTitle + " - " + s; Application.DoEvents(); }
        void MainForm_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();

            Title("Loading RAF Files - ");            
            log.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker 30-April-2011 4:34pm build";

            //Enumerate RAF files
            archives = Directory.GetDirectories(archivesRoot);
            #region load_raf_archives
            for (int i = 0; i < archives.Length; i++)
            {
                string archiveName = archives[i].Replace(archivesRoot, "");

                Title("Loading RAF File - " + archiveName);
                Log("Loading RAF Archive Folder: " + archives[i]);

                RAFArchive raf = null;
                RAFInMemoryFileSystemObject archiveRoot = new RAFInMemoryFileSystemObject(null, RAFFSOType.ARCHIVE, archiveName);
                try
                {
                    //Load raf file table and add to our list of archives
                    rafArchives.Add(archiveName,
                        raf = new RAFArchive(
                            Directory.GetFiles(archives[i], "*.raf")[0]
                        )
                    );

                    //Enumerate entries and add to our tree... in the future this should become sorted
                    List<RAFFileListEntry> entries = raf.GetDirectoryFile().GetFileList().GetFileEntries();
                    for (int j = 0; j < entries.Count; j++)
                    {
                        if (j % 100 == 0)
                            Title("Loading RAF Files - " + archiveName +" - " + j+"/"+entries.Count);

                        RAFInMemoryFileSystemObject node = archiveRoot.AddToTree(RAFFSOType.FILE, entries[j].FileName);
                    }
                    Log(entries.Count.ToString() + " Files");
                }
                catch (Exception exception) { Log("FAILED:\r\n" + exception.Message + "\r\n"); }

                //Add to our tree displayer
                rafContentView.Nodes.Add(archiveRoot);
            }
            #endregion
            rafContentView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(rafContentView_NodeMouseDoubleClick);

            rafContentView.AllowDrop = true;
            rafContentView.DragOver += new DragEventHandler(rafContentView_DragOver);
        }

        void rafContentView_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
            {
                e.Effect = DragDropEffects.Copy;

                rafContentView.Select();
                TreeNode hoveredNode = rafContentView.GetNodeAt(rafContentView.PointToClient(new Point(e.X, e.Y)));
                rafContentView.SelectedNode = hoveredNode;
                Application.DoEvents();
            }
        }

        void rafContentView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RAFInMemoryFileSystemObject node = (RAFInMemoryFileSystemObject)e.Node;
            string nodeInternalPath = node.GetRAFPath();
            if (node.GetFSOType() == RAFFSOType.FILE)
            {
                //We have double clicked a file... find out what file it was
                List<RAFFileListEntry> entries = this.rafArchives[node.GetTopmostParent().Name]
                    .GetDirectoryFile().GetFileList().GetFileEntries();

                //Find the RAF File entry that corresponds to the clicked file...
                RAFFileListEntry entry = entries.Where(
                    (Func<RAFFileListEntry, bool>)delegate(RAFFileListEntry theEntry)
                    {
                        return theEntry.FileName == nodeInternalPath;
                    }
                ).First();

                //Now select a viewer to use for the file.
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
                    }
                }
            }
        }
        private void Log(string s)
        {
            log.Text += "\r\n" + s;
            log.SelectionLength = 0; log.SelectionStart = 0;
        }
    }
}
