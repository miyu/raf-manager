using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using ItzWarty;
using RAFLib;

namespace RAFManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            InitializeLayoutManagement();
            InitializeRAFContentViewManagement();
            InitializeModEntriesManagement();
            this.Load += new EventHandler(MainForm_Load);
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            //HACK!  Fixes scrollbar overlapping content...
            Timer t = new Timer();
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        /// <summary>
        /// When the form closes, we save our last state so we can load it when we restart
        /// </summary>
        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveProject(".laststate.rmproj");
        }

        //The title we use in our wysiwyg editor.  We append to this when setting the title
        string baseTitle = null;
        public void Title(string s)
        {
            if (baseTitle == null) baseTitle = this.Text;

            this.Text = baseTitle + " - " + s;
        }
        /// <summary>
        /// Log text, redrawing the gui and handling the message pump
        /// </summary>
        private void Log(string s)
        {
            Log(s, true);
        }

        /// <summary>
        /// Delayed Log - handles the message pump every 10 ms
        /// </summary>
        private void DLog(string s)
        {
            if (updateDuringLongOperationsCB.Checked)
                Log(s, true);
            else
            {
                if ((DateTime.Now.Millisecond % 10) == 0)
                    Log(s, true);
                else
                    Log(s, false);
            }
        }
        /// <summary>
        /// Logs - takes a param telling it whether or not to redraw the gui & handle message pump
        /// </summary>
        private void Log(string s, bool immediate)
        {
            lock (consoleLogTB)
            {
                try
                {
                    //probs not necessary
                    if (!immediate)
                        consoleLogTB.SuspendLayout();
                    else
                    {
                        consoleLogTB.ResumeLayout(true);
                    }
                    consoleLogTB.Text += "\r\n" + s;
                    consoleLogTB.SelectionStart = consoleLogTB.Text.Length - 2;
                    consoleLogTB.SelectionLength = 1;
                    consoleLogTB.ScrollToCaret();
                    if (immediate)
                        Application.DoEvents();
                }
                catch { }//technically not thread safe... and in this case, it really isn't
            }
        }
        /// <summary>
        /// Checks for updates - updates UI via log when done. presents update prompt if one is available
        /// </summary>
        private void CheckForUpdates()
        {
            RAFManagerUpdater.Autoupdater.CheckUpdate(
                delegate(RAFManagerUpdater.UpdateResult result, string message, string newVersion)
                {
                    if (result == RAFManagerUpdater.UpdateResult.NewUpdate)
                    {
                        new UpdateAvailableDialog(message, newVersion).ShowDialog();
                        //MessageBox.Show(message);
                    }
                    else if (result == RAFManagerUpdater.UpdateResult.NoUpdates)
                    {
                        lock(consoleLogTB)
                        {
                            Log("The program is up to date.  RAFManager just checked!");
                            Log("Update Domain: " + RAFManagerUpdater.Autoupdater.updateDomain);
                        }
                    }
                    else
                    {
                        lock (consoleLogTB)
                        {
                            Log("Unable to connect to update server.");
                            Log("Please check http://www.leagueoflegends.com/board/showthread.php?t=704945");
                            Log("For more information, or redownload the client at www.ItzWarty.com/RAF/");
                        }
                    }
                    return null;
               }
            );
        }
        string archivesRoot;
        private void SetArchivesRoot()
        {
            string expectedPath = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";

            if (Directory.Exists(expectedPath)) archivesRoot = expectedPath;
            else if (File.Exists("lolroot.txt"))
            {
                string lastPath = File.ReadAllText("lolroot.txt");
                if (Directory.Exists(lastPath))
                {
                    archivesRoot = lastPath + @"\RADS\projects\lol_game_client\filearchives\";
                    return;
                }
            }
            else
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Please locate your League of Legends Folder (The folder that contains lol.launcher.exe)";
                fbd.ShowDialog();

                string result = fbd.SelectedPath;
                if (Directory.Exists(result + @"\RADS\projects\lol_game_client\filearchives\"))
                {
                    //success
                    archivesRoot = result + @"\RADS\projects\lol_game_client\filearchives\";
                    //save
                    File.WriteAllText("lolroot.txt", result);
                }
                else
                {
                    MessageBox.Show("Invalid directory: \r\n" + result + @"\RADS\projects\lol_game_client\filearchives\", "Couldn't find directory");
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
        }

        /// <summary>
        /// In this case, rafpack includes the preceeding 0.0.0.xx/
        /// </summary>
        /// <param name="rafPath"></param>
        /// <returns></returns>
        public RAFFileListEntry ResolveRAFPathToEntry(string rafPath)
        {
            int firstSlash = rafPath.IndexOf("/");
            string archiveId = rafPath.Substring(0, firstSlash);  //Get everything before first slash
            string internalPath = rafPath.Substring(firstSlash + 1); //Get everything after first slash

            foreach (RAFArchive archive in rafArchives.Values)
            {
                if (archive.GetID() == archiveId)
                {
                    return archive.GetDirectoryFile().GetFileList().GetFileEntry(internalPath);
                }
            }
            return null;
        }
        /// <summary>
        /// Creates the given directory and all directories leading up to it.
        /// </summary>
        public void PrepareDirectory(string path)
        {
            path = path.Replace("/", "\\");
            String[] dirs = path.Split("\\");
            for (int i = 1; i < dirs.Length; i++)
            {
                String dirPath = String.Join("\\", dirs.SubArray(0, i)) + "\\";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                //ostream.WriteLine(dirPath);
            }
        }
        /// <summary>
        /// Creates the given parent directory and all directories leading up to it.
        /// path should be a filename.
        /// </summary>
        public void PrepareFilesDirectory(string path)
        {
            path = path.Replace("/", "\\");
            String[] dirs = path.Split("\\");
            for (int i = 1; i < dirs.Length-1; i++)
            {
                String dirPath = String.Join("\\", dirs.SubArray(0, i)) + "\\";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                //ostream.WriteLine(dirPath);
            }
        }

        Dictionary<string, RAFArchive> rafArchives = new Dictionary<string, RAFArchive>();
        void MainForm_Load(object sender, EventArgs e)
        {
            MainWindowLoading loader = new MainWindowLoading();
            loader.Show();

            loader.Log("Begin Check For Updates");
            CheckForUpdates();

            loader.Log("Find Archives Folder");
            SetArchivesRoot();

            loader.Log("Begin Loading RAF Archives");
            consoleLogTB.Text = "www.ItzWarty.com Riot Archive File Packer/Unpacker " + ApplicationInformation.BuildTime;

            rafContentView.TreeViewNodeSorter = new RAFFSOTreeNodeSorter();

            //Enumerate RAF files
            string[] archivePaths = Directory.GetDirectories(archivesRoot);
            #region load_raf_archives
            for (int i = 0; i < archivePaths.Length; i++)
            {
                string archiveName = archivePaths[i].Replace(archivesRoot, "").Replace("/", "");

                loader.Log("Load Archive - " + archiveName +" [0%]");
                //Title("Loading RAF File - " + archiveName);
                //Log("Loading RAF Archive Folder: " + archiveName);

                RAFArchive raf = null;
                RAFInMemoryFileSystemObject archiveRoot = new RAFInMemoryFileSystemObject(null, RAFFSOType.ARCHIVE, archiveName);
                rafContentView.Nodes.Add(archiveRoot);
#if !DEBUG
                try
                {
#endif
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
                    // Console.WriteLine(entries[j].StringNameHash.ToString("x").PadLeft(8, '0').ToUpper());
                    if (j % 1000 == 0)
                        loader.Log("Load Archive - " + archiveName + " [" + (j * 100 / entries.Count) + "%]");
                    //Title("Loading RAF Files - " + archiveName +" - " + j+"/"+entries.Count);

                    RAFInMemoryFileSystemObject node = archiveRoot.AddToTree(RAFFSOType.FILE, entries[j].FileName);
                }
                //Log(entries.Count.ToString() + " Files");
#if !DEBUG
                }
                catch (Exception exception) { Log("FAILED:\r\n" + exception.Message + "\r\n"); }
#endif

                //Add to our tree displayer
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
            }
            #endregion

            try
            {
                while (loader.Visible) //Hack - i have no idea why this is necessary sometimes... probs race condition somewhere
                {
                    loader.Hide();
                    Application.DoEvents();
                }
            }catch{}

            lock (consoleLogTB)
            {
                Log("");
                Log("A simple guide for using RAF Manager can be located at About->Simple Guide.");
                Log("");

                if (File.Exists(".laststate.rmproj"))
                {
                    Log("Open last state");
                    OpenProject(".laststate.rmproj");
                }
            }
        }

        
        void t_Tick(object sender, EventArgs e)
        {
            ManageModEntriesLayout();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox().ShowDialog();
        }

        private void goToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.leagueoflegends.com/board/showthread.php?t=722943");
        }

        private void goToRAFManagerLeagueCraftThreadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.leaguecraft.com/index.php?/topic/31543-rafmanager-release/");
        }

        private void goToRAFManagerHomePageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ItzWarty.com/RAF/");
        }
        /// <summary>
        /// Allows the user to pick a RAF file from the rafContentView control...
        /// 
        /// TODO: 5 second timer for stopping
        /// TODO: This needs to be made better.  it's pretty annoying to work with on the user-viewpoint
        /// </summary>
        /// <returns></returns>
        private string PickRafPath(bool includeFiles)
        {
            RAFInMemoryFileSystemObject[] nodes = new RAFInMemoryFileSystemObject[this.rafContentView.Nodes.Count];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = (RAFInMemoryFileSystemObject)this.rafContentView.Nodes[i].Clone();
            RAFPathSelector selectorDialog = new RAFPathSelector(nodes, includeFiles);
            selectorDialog.ShowDialog();
            return selectorDialog.SelectedNodePath;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetProject();
        }
        private void ResetProject()
        {
            ClearModEntries();
            ManageModEntriesLayout();
        }

        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Console.Clear();
            Log("");
            Log("Begin Packing");
            for (int i = 0; i < modEntries.Count; i++)
            {
                ModEntry entry = modEntries[i];
                if (entry.Script == null || (entry.IsChecked?!entry.Script.HasPackTimeOverride:!entry.Script.HasRestoreTimeOverride))
                {
                    DLog("->Pack/Restore Nonscripted Entry: " + entry.ModName);
                    ModEntryNodeTag[] nodeTags = entry.GetModEntryTags();
                    for (int j = 0; j < nodeTags.Length; j++)
                    {
                        RAFFileListEntry rafEntry = ResolveRAFPathToEntry(nodeTags[j].rafPath);
                        string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + nodeTags[j].rafPath.Replace("/", "_");
                        if (entry.IsChecked)
                        {
                            if (!File.Exists(fileBackupLoc))
                            {
                                DLog("  Backing up " + nodeTags[j].rafPath);
                                PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                File.WriteAllBytes(fileBackupLoc, rafEntry.GetContent());
                            }
                            DLog("  Inserting " + nodeTags[j].rafPath);
                            rafEntry.RAFArchive.InsertFile(
                                rafEntry.FileName,
                                File.ReadAllBytes(nodeTags[j].localPath),
                                null
                            );
                        }
                        else if (File.Exists(fileBackupLoc))
                        {
                            DLog("  Restoring " + nodeTags[j].rafPath);
                            rafEntry.RAFArchive.InsertFile(
                                rafEntry.FileName,
                                File.ReadAllBytes(fileBackupLoc),
                                null
                            );
                        }
                    }
                    DLog("  Done");
                }
                else
                {
                    if (entry.IsChecked)
                    {
                        DLog("->Running RAF Manager Script : Packtime Command");
                        entry.Script.RunPacktimeCommand();
                        DLog("  Done");
                    }
                    else
                    {
                        DLog("->Running RAF Manager Script : Restoretime Command");
                        entry.Script.RunRestoreTimeCommand();
                        DLog("  Done");
                    }
                }
            }
            DLog("Saving Archive Directory Files (*.raf)");
            for (int i = 0; i < rafArchives.Count; i++)
                new List<RAFArchive>(rafArchives.Values)[i].SaveDirectoryFile();
            Log("Done Packing");
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "RAF Manager Project|*.rmproj";
            sfd.ShowDialog();
            if (sfd.FileName != "")
            {
                SaveProject(sfd.FileName);
            }
        }
        private void SaveProject(string filePath)
        {
            Log("Saving to: " + filePath);
            List<string> resultLines = new List<string>();
            resultLines.Add("RMPROJ1.4");
            resultLines.Add(projectNameLabel.Text);
            foreach (ModEntry modEntry in modEntries)
            {
                resultLines.Add(
                    "> \"" + modEntry.ModName + "\" \"" + modEntry.ModCreator + "\" \"" + modEntry.ModURL + "\""
                );

                if (modEntry.IconImage != null)
                    resultLines.Add("IMAGEFILE \"" + (string)modEntry.IconImage.Tag + "\"");

                if (modEntry.Script != null)
                {
                    resultLines.Add("SCRIPTFILE \"" + modEntry.Script.ScriptPath + "\"");
                    foreach (RMPropInterpreter.RMPropSetting setting in modEntry.Script.Settings)
                    {
                        resultLines.Add("OPTION \"" + setting.VarName + "\" \"" + setting.SelectedValue + "\"");
                    }
                }
                if (modEntry.IsChecked)
                    resultLines.Add("CHECKED 1");
                else
                    resultLines.Add("CHECKED 0");

                ModEntryNodeTag[] tags = modEntry.GetModEntryTags();
                for (int i = 0; i < tags.Length; i++)
                    resultLines.Add("\"" + tags[i].localPath + "\" \"" + tags[i].rafPath + "\"");

                resultLines.Add("<");
            }
            File.WriteAllLines(filePath, resultLines.ToArray());
            Log("->Done");
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RAF Manager Project|*.rmproj";
            ofd.ShowDialog();
            if (ofd.FileName != "")
            {
                OpenProject(ofd.FileName);
            }
        }
        private void OpenProject(string filePath)
        {
            ResetProject();

            string[] lines = File.ReadAllLines(filePath);
            if (lines[0] != "RMPROJ1.4")
            {
                Log("Invalid File Format.  Expected RMPROJ1.4... Perhaps you're trying to use a project file from an older version?");
                return;
            }
            projectNameLabel.Text = lines[1];

            ModEntry current = null;
            for (int i = 2; i < lines.Length; i++)
            {
                try
                {
                    string line = lines[i];
                    string[] lineParts = line.QASS(' ');
                    switch (lineParts[0].Trim().ToUpper())
                    {
                        case ">":
                        {
                            current = CreateAndAppendModEntry(lineParts[1], lineParts[2], lineParts[3], new TreeNode[] { });
                            break;
                        }
                        case "IMAGEFILE":
                        {
                            if (lineParts.Length == 1 || lineParts[1].Trim() == "")
                            {
                                current.IconImage = Properties.Resources.NoIcon;
                                current.IconImage.Tag = " ";
                            }
                            else
                            {
                                current.IconImage = Bitmap.FromFile(lineParts[1]);
                                current.IconImage.Tag = lineParts[1];
                            }
                            break;
                        }
                        case "SCRIPTFILE":
                        {
                            current.Script = new RMPropInterpreter(lineParts[1], this);
                            break;
                        }
                        case "OPTION":
                        {
                            current.Script.SetOption(lineParts[1], lineParts[2]);
                            break;
                        }
                        case "CHECKED":
                        {
                            current.IsChecked = lineParts[1] == "1";
                            break;
                        }
                        case "<":
                            break;//Really don't do anything.
                        default:
                        {
                            if (line != "")
                                current.AddFile(lineParts[0], lineParts[1]);
                            break;
                        }
                    }
                }
                catch 
                {
                    if (current != null)
                    {
                        Log("Error loading Entry named '" + current.ModName + "'... It will not be loaded.");
                        try
                        {
                            current.Delete();
                        }catch{}
                    }
                }
            }
        }

        private void projectNameChangeBTN_Click(object sender, EventArgs e)
        {
            StringQueryDialog sqd = new StringQueryDialog("Rename Project To: ", projectNameLabel.Text);
            sqd.ShowDialog();
            if (sqd.Value != "")
                projectNameLabel.Text = sqd.Value;
        }

        private void viewGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SimpleGuide(this).Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }
    }
}
