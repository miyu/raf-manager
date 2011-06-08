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
    //This needs some organizing. sort of hacked together
    public partial class MainForm : Form, IRAFManagerGUI
    {
        //public field =[
        public RAFManager rafManager = null;
        public MainForm()
        {
        }
        public void Init(RAFManager rafManager)
        {
            this.rafManager = rafManager;

            InitializeComponent();
            InitializeLayoutManagement();
            InitializeRAFContentViewManagement();
            InitializeModEntriesManagement();
            this.Load += delegate(object sender, EventArgs e)
            {
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
            };

            // When the form closes, we save our last state so we can load it when we restart
            this.FormClosing += delegate(object sender, FormClosingEventArgs e){
                SaveProject(".laststate.rmproj");
            };

            if (RAFManagerUpdater.Versioning.CurrentVersion.flags != "")
            {
                this.Text = this.Text + " BETA/Non-RC " + RAFManagerUpdater.Versioning.CurrentVersion.GetVersionString() + " [Build Time: " + RAFManagerUpdater.Versioning.CurrentVersion.ApproximateBuildTime + "]";
            }

            //HACK!  Fixes scrollbar overlapping content...
            Timer t = new Timer();
            t.Interval = 100;
            t.Tick += delegate(object sender, EventArgs e){
                ManageModEntriesLayout();
            };
            t.Start();
        }

        public void PrepareForShow()
        {
            Console.WriteLine(this.rafManager.ArchiveFSOs.Count);
            this.rafContentView.TreeViewNodeSorter = new RAFFSOTreeNodeSorter();
            foreach (RAFInMemoryFileSystemObject fso in this.rafManager.ArchiveFSOs)
                this.rafContentView.Nodes.Add((RAFInMemoryFileSystemObject)fso.Clone());
        }

        UpdateAvailableDialog updateAvailableDialog = null;
        public void ShowUpdateAvailableWindow(string patchnotes, string nextVersion)
        {
            updateAvailableDialog = new UpdateAvailableDialog(patchnotes, nextVersion);
            updateAvailableDialog.ShowDialog();
        }

        MainWindowLoading loaderWindow = new MainWindowLoading();
        public void ShowLoaderWindow()
        {
            loaderWindow.Show();
        }
        public void HideLoaderWindow()
        {
            loaderWindow.Hide();
        }
        public void LogToLoader(string s)
        {
            loaderWindow.Log(s);
        }
        public void SetLastLoaderLine(string s)
        {
            loaderWindow.Log(s); //TODO
        }
        public void ClearLoaderWindow()
        {
            //TODO
        }
        public void ShowAboutWindow()
        {
            new AboutBox().ShowDialog();
        }
        public Form GetMainWindow()
        {
            return this;
        }



        //The title we use in our wysiwyg editor.  We append to this when setting the title
        string baseTitle = null;
        public void Title(string s)
        {
            if (baseTitle == null) baseTitle = this.Text;

            this.Text = baseTitle + " - " + s;
        }

        #region logging: Log, DLog
        /// <summary>
        /// Log text, redrawing the gui and handling the message pump
        /// </summary>
        public void Log(string s)
        {
            Log(s, true);
        }

        /// <summary>
        /// Delayed Log - handles the message pump every 10 ms
        /// </summary>
        public void DLog(string s)
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
        #endregion

        Dictionary<string, RAFArchive> rafArchives = new Dictionary<string, RAFArchive>();

        #region GUI Menu Handling
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

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetProject();
        }
        #endregion
        /// <summary>
        /// Allows the user to pick a RAF file from the rafContentView control...
        /// 
        /// TODO: 5 second timer for stopping
        /// TODO: This needs to be made better.  it's pretty annoying to work with on the user-viewpoint
        /// </summary>
        /// <returns></returns>
        private string PickRafPath(bool includeFiles)
        {
            return PickRafPath(includeFiles, "Pick A " + (includeFiles ? "Directory/File" : "Directory"));
        }
        private string PickRafPath(bool includeFiles, string text)
        {
            RAFInMemoryFileSystemObject[] nodes = new RAFInMemoryFileSystemObject[this.rafContentView.Nodes.Count];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = (RAFInMemoryFileSystemObject)this.rafContentView.Nodes[i].Clone();
            RAFPathSelector selectorDialog = new RAFPathSelector(nodes, includeFiles, text);
            selectorDialog.ShowDialog();
            return selectorDialog.SelectedNodePath;
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
                        RAFFileListEntry rafEntry = rafManager.ResolveRAFPathToEntry(nodeTags[j].rafPath);
                        
                        string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + nodeTags[j].rafPath.Replace("/", "_");
                        if (entry.IsChecked)
                        {
                            if (rafEntry == null && permitExperimentalFileAddingCB.Checked)
                            {
                                //File doesn't exist in archive
                                //Our backup = "this file should be deleted"
                                DLog("  Marking for restore op deletion: " + nodeTags[j].rafPath);
                                Util.PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                File.WriteAllText(fileBackupLoc, "this file should be deleted");

                                DLog("  Adding " + nodeTags[j].rafPath);
                                string archiveName = nodeTags[j].rafPath.Split("/").First();
                                rafArchives[archiveName].InsertFile(
                                    nodeTags[j].rafPath.Replace(archiveName+"/", ""),
                                    File.ReadAllBytes(nodeTags[j].localPath),
                                    new LogTextWriter(
                                        (Func<string, object>)delegate(string s)
                                        {
                                            if (updateDuringLongOperationsCB.Checked)
                                                DLog(s);
                                            return null;
                                        }
                                    )
                                );
                            }
                            else
                            {
                                //File does exist in archive, do backup if not done already
                                if (!File.Exists(fileBackupLoc))
                                {
                                    DLog("  Backing up " + nodeTags[j].rafPath);
                                    Util.PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                    File.WriteAllBytes(fileBackupLoc, rafEntry.GetContent());
                                }
                                DLog("  Inserting " + nodeTags[j].rafPath);
                                rafEntry.RAFArchive.InsertFile( 
                                    rafEntry.FileName,
                                    File.ReadAllBytes(nodeTags[j].localPath),
                                    new LogTextWriter(
                                        (Func<string, object>)delegate(string s)
                                        {
                                            if (updateDuringLongOperationsCB.Checked)
                                                DLog(s);
                                            return null;
                                        }
                                    )
                                );
                            }
                        }
                        else if (File.Exists(fileBackupLoc))
                        {
                            if (permitExperimentalFileAddingCB.Checked && File.ReadAllText(fileBackupLoc) == "this file should be deleted")
                            {
                                DLog("  Deleting " + nodeTags[j].rafPath);
                                rafEntry.RAFArchive.GetDirectoryFile().DeleteFileEntry(rafEntry);
                            }
                            else
                            {
                                DLog("  Restoring " + nodeTags[j].rafPath);
                                rafEntry.RAFArchive.InsertFile(
                                    rafEntry.FileName,
                                    File.ReadAllBytes(fileBackupLoc),
                                    null
                                );
                            }
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
            //CheckForUpdates();
        }

        private void lOLMODRAFManagerScriptDocumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://forum.leaguecraft.com/index.php?/topic/32653-rafmanager-and-a-standardized-skin-release-format/");
        }
    }
}
