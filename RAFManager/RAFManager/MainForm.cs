//#define TaskBar
//TODO: Import folder to raf manager

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
using RAFManager.Project;

namespace RAFManager
{
    public partial class MainForm : Form
    {
        //TODO: Configuration file to set this stuff
        //And a pretty editor.
        private string archivesRoot = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";

        //Names of our archives
        private Dictionary<string, RAFArchive> rafArchives = new Dictionary<string, RAFArchive>();

        private TristateTreeView changesView = null;

        public MainForm()
        {
            InitializeComponent();
            changesView = new TristateTreeView();
            changesView.Dock = DockStyle.Fill;
            changesView.EmptyComment = "No items have been added yet!  Drag skin files in!";
            this.smallContainer.Panel2.Controls.Add(changesView);

            /*
             * TristateTreeNode test
            for (int i = 0; i < 5; i++)
            {
                changesView.Nodes.Add(new TristateTreeNode("...."));
                for (int j = 0; j < 5; j++)
                {
                    changesView.Nodes[i].Nodes.Add(new TristateTreeNode("...."));
                }
            }
             */
            CheckForUpdates();

            SetArchivesRoot();

            this.Load += new EventHandler(MainForm_Load);

            int bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
            int smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
            this.ResizeBegin += delegate(object sender, EventArgs e)
            {
                try
                {
                    bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
                    smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
                }
                catch { }
            };
            this.Resize += delegate(object sender, EventArgs e)
            {
                try
                {
                    bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
                    smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
                }
                catch { }
            };

            InitializeChangesView();
            InitializeUtil();
            InitializeProject();
#if TaskBar
            Windows7.DesktopIntegration.Windows7Taskbar.AllowTaskbarWindowMessagesThroughUIPI();
            Windows7.DesktopIntegration.Windows7Taskbar.SetWindowAppId(this.Handle, "RAFManager");
#endif
        }
        /// <summary>
        /// Sets the progress value of our taskbar entry
        /// </summary>
        /// <param name="n">progress from 0-100</param>
        void SetTaskbarProgress(int n)
        {
#if TaskBar
            if(n <= 0)
                Windows7.DesktopIntegration.Windows7Taskbar.SetProgressState(this.Handle, Windows7.DesktopIntegration.Windows7Taskbar.ThumbnailProgressState.NoProgress);
            else
                Windows7.DesktopIntegration.Windows7Taskbar.SetProgressValue(this.Handle, (UInt32)n, 100);
#endif
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();


            Title("Loading RAF Files - ");            
            log.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker "+ApplicationInformation.BuildTime;

            byte[] a;
            
            //Enumerate RAF files
            string[] archivePaths = Directory.GetDirectories(archivesRoot);
            #region load_raf_archives
            for (int i = 0; i < archivePaths.Length; i++)
            {
                string archiveName = archivePaths[i].Replace(archivesRoot, "");

                Title("Loading RAF File - " + archiveName);
                Log("Loading RAF Archive Folder: " + archiveName[i]);

                RAFArchive raf = null;
                RAFInMemoryFileSystemObject archiveRoot = new RAFInMemoryFileSystemObject(null, RAFFSOType.ARCHIVE, archiveName);
                try
                {
                    //Load raf file table and add to our list of archives
                    rafArchives.Add(archiveName,
                        raf = new RAFArchive(
                            Directory.GetFiles(archivePaths[i], "*.raf")[0]
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

            Title("Done loading RAF Files");

            LogInstructions();
            UpdateProjectGUI();
        }

        /// <summary>
        /// Logs a message to our console
        /// </summary>
        private void Log(string s)
        {
            try
            {
                log.Text += "\r\n" + s;
                log.SelectionStart = log.Text.Length;
                log.ScrollToCaret();
            }
            catch { }
        }

        /// <summary>
        /// Writes the instructions/welcome of this app to our logger
        /// </summary>
        private void LogInstructions()
        {
            Log("");
            Log("ItzWarty's RAFManager Instructions: (build time: " + ApplicationInformation.BuildTime + ")");
            Log("Check www.ItzWarty.com/RAF/ for updates!");
            Log("This program will _not_ automatically update as of yet.");
            Log("");
            Log("Double click a node on the file tree to the top left to view its contents.");
            Log("Ask questions @ (LoL Forums)http://bit.ly/jnT4p8 or (LeagueCraft)http://bit.ly/mLitsi");
            Log("");
            Log("INIBIN/TROYBIN Reader by Engberg @ http://bit.ly/kThoeF");
            Log("Be.HexEditor by http://sourceforge.net/projects/hexbox/");
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void goToRAFPackerLeagueOfLegendsThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.leagueoflegends.com/board/showthread.php?t=722943");
        }

        private void goToRAFPackerLeagueCraftTHreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.leaguecraft.com/index.php?/topic/31543-rafmanager-release/");
        }

        private void goToRAFManagerHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ItzWarty.com/RAF/");
        }
    }
}
