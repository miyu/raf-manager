#define TaskBar
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

            this.Load += new EventHandler(MainForm_Load);
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
            //this.Show();
            //Application.DoEvents();
            MainWindowLoading loader = new MainWindowLoading();
            loader.Show();

            loader.Log("Begin Check For Updates");
            CheckForUpdates();

            loader.Log("Find Archives Folder");
            SetArchivesRoot();

            int bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
            int smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
            this.ResizeBegin += delegate(object sender2, EventArgs e2)
            {
                try
                {
                    bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
                    smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
                    verboseLoggingCB.Left = bigContainer.Width - verboseLoggingCB.Width;
                }
                catch { }
            };
            this.Resize += delegate(object sender2, EventArgs e2)
            {
                try
                {
                    bigSplitterDistanceFromBottom = bigContainer.Height - bigContainer.SplitterDistance;
                    smallSplitterDistanceFromLeft = smallContainer.SplitterDistance;
                    verboseLoggingCB.Left = bigContainer.Width - verboseLoggingCB.Width;
                }
                catch { }
            };

            loader.Log("Init Changes View");
            InitializeChangesView();

            loader.Log("Init Util");
            InitializeUtil();

            loader.Log("Init Project Manager");
            InitializeProject();
#if TaskBar
            loader.Log("Init Win7 Taskbar Features");
            Windows7.DesktopIntegration.Windows7Taskbar.AllowTaskbarWindowMessagesThroughUIPI();
            Windows7.DesktopIntegration.Windows7Taskbar.SetWindowAppId(this.Handle, "RAFManager");
#endif


            loader.Log("Begin Loading RAF Archives");        
            log.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker "+ApplicationInformation.BuildTime;

            byte[] a;

            rafContentView.TreeViewNodeSorter = new RAFFSOTreeNodeSorter();
            //Enumerate RAF files
            string[] archivePaths = Directory.GetDirectories(archivesRoot);
            #region load_raf_archives
            for (int i = 0; i < archivePaths.Length; i++)
            {
                string archiveName = archivePaths[i].Replace(archivesRoot, "");

                loader.Log("Load Archive - " + archiveName);
                //Title("Loading RAF File - " + archiveName);
                //Log("Loading RAF Archive Folder: " + archiveName);

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
                        //if (j % 1000 == 0)
                        //    loader.Log(" - " + archiveName + " - " + j + "/" + entries.Count);
                            //Title("Loading RAF Files - " + archiveName +" - " + j+"/"+entries.Count);

                        RAFInMemoryFileSystemObject node = archiveRoot.AddToTree(RAFFSOType.FILE, entries[j].FileName);
                    }
                    //Log(entries.Count.ToString() + " Files");
                }
                catch (Exception exception) { Log("FAILED:\r\n" + exception.Message + "\r\n"); }

                //Add to our tree displayer
                loader.Log("Preparing Environment...");
                //Title("Sorting nodes... this might take a while");
                if (archiveRoot.Nodes.Count == 0)
                {
                    MessageBox.Show("Another instance of RAF Manager is likely already open.\r\n" +
                                    "If not, then another application has not released control over the \r\n" +
                                    "RAF Archives.  RAF Manager will continue to run, but some features \r\n" +
                                    "may not work properly.  Usually a restart of the application will \r\n" +
                                    "fix this.  If you have issues, post a reply on the forum thread, \r\n" +
                                    "whose link can be found under the 'About' menu header.");
                }
                rafContentView.Nodes.Add(archiveRoot);
            }
            #endregion

            loader.Hide();
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

        private void projectNameTb_TextChanged_1(object sender, EventArgs e)
        {
            HasProjectChanged = true;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PromptSaveToClose();
        }
        private void PromptSaveToClose()
        {
            if (HasProjectChanged)
            {
                if (DialogResult.Yes == MessageBox.Show("You haven't saved your project/changes!  Would you like to do so?", "U haz unsave changed!", MessageBoxButtons.YesNo))
                {
                    saveToolStripMenuItem_Click(null, null);
                }
            }
        }
    }
}
