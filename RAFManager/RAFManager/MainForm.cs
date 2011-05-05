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
            };

            InitializeChangesView();
            InitializeUtil();
            InitializeProject();

            Windows7.DesktopIntegration.Windows7Taskbar.AllowTaskbarWindowMessagesThroughUIPI();
            Windows7.DesktopIntegration.Windows7Taskbar.SetWindowAppId(this.Handle, "RAFManager");
        }
        /// <summary>
        /// Sets the progress value of our taskbar entry
        /// </summary>
        /// <param name="n">progress from 0-100</param>
        void SetTaskbarProgress(int n)
        {
            if(n <= 0)
                Windows7.DesktopIntegration.Windows7Taskbar.SetProgressState(this.Handle, Windows7.DesktopIntegration.Windows7Taskbar.ThumbnailProgressState.NoProgress);
            else
                Windows7.DesktopIntegration.Windows7Taskbar.SetProgressValue(this.Handle, (UInt32)n, 100);
        }
        void MainForm_Load(object sender, EventArgs e)
        {
            this.Show();
            Application.DoEvents();


            Title("Loading RAF Files - ");            
            log.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker 30-April-2011 4:34pm build";

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
        private void Log(string s)
        {
            log.Text += "\r\n" + s;
            log.SelectionStart = log.Text.Length;
            log.ScrollToCaret();
        }
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

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are all archives backed up?  This application has been tested quite a bit, but errors might one day pop up.  You should backup your archive files (run the backup menu item).  Do you want to continue?  This is your last warning.", "Confirm backup", MessageBoxButtons.YesNo);

            //Verify
            if (!VerifyPackPrecondition()) MessageBox.Show("Not all preconditions for packing were met.  Read the log, located at the bottom of the main window.");

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                //We shall do our stuffs.
                for (int i = 0; i < changesView.Rows.Count; i++)
                {
                    DataGridViewRow row = changesView.Rows[i];
                    string rafPath = (string)row.Cells[CN_RAFPATH].Value;
                    string localPath = (string)row.Cells[CN_LOCALPATH].Value;

                }
            }
        }
        private bool VerifyPackPrecondition()
        {
            for (int i = 0; i < changesView.Rows.Count; i++)
            {
                DataGridViewRow row = changesView.Rows[i];
                if (row.Cells[CN_RAFPATH].Value == "" || row.Cells[CN_LOCALPATH].Value == "")
                {
                    Log("Row of rafpath value '" + (string)row.Cells[CN_RAFPATH].Value + "' and localPath value '" + (string)row.Cells[CN_LOCALPATH].Value + "' is incomplete'");
                    return false;
                }else{
                    List<RAFArchive> archives = new List<RAFArchive>(rafArchives.Values);
                    bool foundEntry = false;
                    foreach(RAFArchive archive in archives)
                    {
                        if (archive.GetDirectoryFile().GetFileList().GetFileEntry((string)row.Cells[CN_RAFPATH].Value) != null) foundEntry = true;
                    }
                    if (!foundEntry)
                    {
                        Log("RAFPath '" + (string)row.Cells[CN_RAFPATH].Value + "' couldn't be found in RAF archives.  ");
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
