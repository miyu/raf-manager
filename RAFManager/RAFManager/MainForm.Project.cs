using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;

using RAFLib;
using RAFManager.Project;
using ItzWarty;

namespace RAFManager
{
    public partial class MainForm:Form
    {
        private RAFProjectInfo projectInfo  = null;
        private bool hasProjectChanged      = false;

        /// <summary>
        /// Initializes the project system
        /// </summary>
        public void InitializeProject()
        {
            ResetProject();

            projectNameTb.TextChanged += new EventHandler(projectNameTb_TextChanged);
        }

        /// <summary>
        /// Resets the project to its default settings
        /// 
        /// Also clears the changes log
        /// </summary>
        private void ResetProject()
        {
            projectInfo = new RAFProjectInfo();
            projectInfo.ProjectName = "Untitled Project";
            projectInfo.ProjectPath = "";
            projectInfo.FileArchivesDirectory = "";

            changesView.Nodes.Clear();
        }

        /// <summary>
        /// When save is clicked, a SaveFileDialog is opened,
        /// then the file is saved
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Title("Saving Project... ");
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".rmproj";
            dialog.AddExtension = true;
            dialog.Filter = "RAFManager Projects(*.rmproj)|*.rmproj";
            dialog.ShowDialog();
            if (dialog.CheckPathExists && dialog.FileName != null && dialog.FileName != "")
            {
                SaveProject(dialog.FileName);
            }
        }

        /// <summary>
        /// Performs a save operation of the current project
        /// to the given location
        /// </summary>
        private void SaveProject(string location)
        {
            //changesView.ClearSelection();
            /**
             * Output:
             * RAF
             * [Project Name]
             * -blank-
             * > NodeBegin
             * e entry
             * < End
             */
            string crnl = "\r\n"; //To make it viewable with notepad, as oposed to the original unix style
            string serialization = "RAF";
            serialization += crnl + projectInfo.ProjectName;
            serialization += crnl + ""; //blank

            Stack<TristateTreeNode> nodeStack = new Stack<TristateTreeNode>();
            for (int i = changesView.Nodes.Count - 1; i >= 0; i--)
                nodeStack.Push(changesView.Nodes[i]);
            while(nodeStack.Count != 0)
            {
                //If a node has no children, we ignore it, since that means it's just a description node
                //IE: the nodes sitting under the "fileName.dds" nodes, with no cboxes
                TristateTreeNode node = nodeStack.Pop();
                if (node == null)
                {
                    //Append < for done with this node
                    serialization += crnl + "<";
                }
                else if (node.Nodes.Count == 0) { } //Do nothing.  reserved?
                else //Node has children.  Push null (for done) and all children...
                {
                    if (node.Tag == null) //Is a group node
                    {
                        nodeStack.Push(null);
                        for (int i = node.Nodes.Count - 1; i >= 0; i--)
                            nodeStack.Push(node.Nodes[i]);
                        serialization += crnl + "> " + node.Text;
                    }
                    else
                    {
                        ChangesViewEntry entry = (ChangesViewEntry)node.Tag;
                        serialization += crnl + "e " + (entry.Checked ? "1" : "0") + " " + entry.LocalPath + " | " +
                                        entry.Entry.RAFArchive.GetID() + "/" + entry.Entry.FileName;
                    }
                }
            }

            HasProjectChanged = false;
            File.WriteAllText(location, serialization);
            UpdateProjectGUI();
        }
        /// <summary>
        /// When the Open entry is clicked, create an openfiledialog
        /// then save at the appropriate location
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Title("Opening Project... ");
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".rmproj";
            dialog.AddExtension = true;
            dialog.Filter = "RAFManager Projects(*.rmproj)|*.rmproj";
            dialog.ShowDialog();
            if (dialog.CheckPathExists && dialog.FileName != null && dialog.FileName != "")
            {
                LoadProject(dialog.FileName);
            }
        }

        /// <summary>
        /// preforms the action of loading a project, located
        /// at the given location
        /// </summary>
        private void LoadProject(string location)
        {
            //Get a clean project first, before we load in contents
            ResetProject();

            string[] lines = File.ReadAllLines(location);
            for (int i = 0; i < lines.Length; i++)
                lines[i] = lines[i].Split(";")[0].Trim();

            string header = lines[0];
            if (header != "RAF")
            {
                MessageBox.Show("Invalid RAF Project.\r\nPerhaps the RMProj file format has changed.\r\nYou will have to create a new project.", "=[");
                return;
            }
            string projectName = lines[1];
            string projectPath = location;// lines[1];
            string rafDirectory= lines[2];

            projectNameTb.Text = projectName;
            projectInfo.ProjectName = projectName.Trim();
            projectInfo.ProjectPath = location;
            projectInfo.FileArchivesDirectory = rafDirectory;
            Stack<TristateTreeNode> nodeStack = new Stack<TristateTreeNode>();
            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line != "")
                {
                    switch(line[0])
                    {
                        case '>':
                        {
                            TristateTreeNode node = new TristateTreeNode(line.Substring(1).Trim());
                            if (nodeStack.Count == 0)
                                changesView.Nodes.Add(node);
                            else
                                nodeStack.Peek().Nodes.Add(node);
                            node.HasCheckBox = true;
                            nodeStack.Push(node);
                            break;
                        }
                        case 'e': //entry
                        {
                            bool check = line[2] == '1';
                            string afterCheck = line.Substring(3); //get everything after the check
                            string[] parts = afterCheck.Split("|"); //yields {localPath, rafPath}
                            string localPath = parts[0].Trim().Replace("\\", "/");
                            string rafPath = parts[1].Trim().Replace("\\", "/");       //includes RAF Archive Id (0.0.0.xx)

                            TristateTreeNode node = new TristateTreeNode(localPath.Split("/").Last());
                            node.Nodes.Add(new TristateTreeNode("Local Path: " + localPath));
                            node.Nodes.Add(new TristateTreeNode("RAF Path: " + rafPath));
                            node.Nodes[0].HasCheckBox = false;
                            node.Nodes[1].HasCheckBox = false;
                            node.Tag = new ChangesViewEntry(localPath, ResolveRAFPathToEntry(rafPath), node);
                            node.HasCheckBox = true;
                            if (nodeStack.Count == 0)
                                changesView.Nodes.Add(node);
                            else
                                nodeStack.Peek().Nodes.Add(node);
                            break;
                        }
                        case '<':
                        {
                            nodeStack.Pop();
                            break;
                        }
                    }
                }
            }

            UpdateProjectGUI();
            //UpdateChangesGUI();
        }

        /// <summary>
        /// Updates the project GUI
        ///  - Updates the project name display
        ///  - Updates the window title
        /// </summary>
        private void UpdateProjectGUI()
        {
            projectNameTb.Text = this.projectInfo.ProjectName;
            Title(GetWindowTitle());
        }

        /// <summary>
        /// Gets the appropriate window title for this project
        /// Including the asterisk for hasedited
        /// </summary>
        private string GetWindowTitle()
        {
            string result = "";
            result += projectInfo.ProjectName;
            if (hasProjectChanged)
            {
                result += "*";
            }
            if (projectInfo.ProjectPath != "")
                result += " (" + projectInfo.ProjectPath + ")";
            return result;
        }

        void projectNameTb_TextChanged(object sender, EventArgs e)
        {
            this.projectInfo.ProjectName = projectNameTb.Text;
            Title(this.GetWindowTitle());
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.hasProjectChanged)
            {
                //Save Dialog                                                                                                                                                       HUEHUEHUEHUEHUEHUEHUEHUEHUE
                DialogResult result = MessageBox.Show("You have unsaved changes.\nClick cancel to save those changes before you do anything else.\nClick ok to continue onwards.", "Insert Funny Comment here", MessageBoxButtons.OKCancel);
                if (result == System.Windows.Forms.DialogResult.Cancel) return;
                else
                    Log("Rammus: ok");
            }

            this.ResetProject();
            this.UpdateProjectGUI();
        }

        /// <summary>
        /// Represents whether or not a change has been applied to this project since its last load
        /// </summary>
        public bool HasProjectChanged
        {
            get
            {
                return this.hasProjectChanged;
            }
            set
            {
                this.hasProjectChanged = value;
            }
        }

        /// <summary>
        /// When the toolstrip->pack is clicked,
        /// 1) Ask the user if archives are backed up
        /// 2) Verify Preconditions
        /// 3) Pack
        /// </summary>
        private void packToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are all archives backed up?  This application has been tested quite a bit, but errors might one day pop up.  You should backup your archive files (run the backup menu item).  Do you want to continue?  This is your last warning.", "Confirm backup", MessageBoxButtons.YesNo);

            //Verify
            if (!VerifyPackPrecondition())
            {
                MessageBox.Show("Not all preconditions for packing were met.  Read the log, located at the bottom of the main window.");
                return;
            }

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                for (int i = 0; i < changesView.Nodes.Count; i++)
                    PackNode(changesView.Nodes[i]);
            }
            Title(GetWindowTitle());
        }
        int packTick = 0;
        private void PackNode(TristateTreeNode node)
        {
            ChangesViewEntry cventry = (ChangesViewEntry)node.Tag;
            if (cventry == null) //Group node
                for (int i = 0; i < node.Nodes.Count; i++)
                    PackNode(node.Nodes[i]);
            else
            {
                RAFFileListEntry entry = cventry.Entry;
                string rafPath = entry.FileName;
                string localPath = cventry.LocalPath;
                bool useFile = cventry.Checked;

                packTick = (packTick + 1) % 10;
                if(packTick == 0 || verboseLoggingCB.Checked)
                    Title("Pack File: " + localPath.Replace("\\", "/").Split("/").Last());

                //Console.WriteLine(Environment.CurrentDirectory + "/backup/");
                PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + entry.FileName.Replace("/", "_");
                if (!File.Exists(fileBackupLoc))
                    File.WriteAllBytes(fileBackupLoc, entry.GetContent());

                //Open the RAF archive, insert.
                if (useFile)
                    entry.RAFArchive.InsertFile(
                        rafPath,
                        File.ReadAllBytes(localPath),
                        new LogTextWriter(
                            (Func<string, object>)delegate(string s)
                            {
                                if(verboseLoggingCB.Checked)
                                    Log(s);
                                return null;
                            }
                        )
                    );
                else
                {
                    //Insert backup
                    entry.RAFArchive.InsertFile(
                        rafPath,
                        File.ReadAllBytes(fileBackupLoc),
                        new LogTextWriter(
                            (Func<string, object>)delegate(string s)
                            {
                                if (verboseLoggingCB.Checked)
                                    Log(s);
                                return null;
                            }
                        )
                    );
                }
                List<RAFArchive> archives = new List<RAFArchive>(rafArchives.Values);
                for (int i = 0; i < archives.Count; i++)
                {
                    SetTaskbarProgress((i + 1) * 100 / (archives.Count + 1));
                    archives[i].SaveDirectoryFile();
                }

                SetTaskbarProgress(0);
            }
        }
        /// <summary>
        /// Verifies that the project is ready for packing:
        /// All rows need to be filled properly
        /// </summary>
        /// <returns></returns>
        private bool VerifyPackPrecondition()
        {
            return VerifyPackPrecondition(changesView.Nodes.ToList());
        }
        private bool VerifyPackPrecondition(List<TristateTreeNode> nodes)
        {
            Stack<TristateTreeNode> nodeStack = new Stack<TristateTreeNode>();
            for (int i = nodes.Count - 1; i >= 0; i--)
                nodeStack.Push(nodes[i]);
            while (nodeStack.Count != 0)
            {
                //If a node has no children, we ignore it, since that means it's just a description node
                //IE: the nodes sitting under the "fileName.dds" nodes, with no cboxes
                TristateTreeNode node = nodeStack.Pop();
                if (node.Nodes.Count == 0) { } //Do nothing.  Desciptor node such as RAF Path: nodes
                else //Node has children.  Push node
                {
                    if (node.Tag == null) //Is a group node
                    {
                        for (int i = node.Nodes.Count - 1; i >= 0; i--)
                            nodeStack.Push(node.Nodes[i]);
                    }
                    else
                    {
                        ChangesViewEntry entry = (ChangesViewEntry)node.Tag;
                        if (ResolveRAFPathToEntry(entry.Entry.RAFArchive.GetID() + "/" + entry.Entry.FileName) == null)
                        {
                            Log("Precondition fail!: ");
                            Log("RAF Path Doesnt Exist: "+entry.Entry.RAFArchive.GetID()+"/"+entry.Entry.FileName);
                            return false;
                        }
                        else if (!File.Exists(entry.LocalPath))
                        {
                            Log("Precondition fail!: ");
                            Log("Local Path Doesnt Exist: " + entry.LocalPath);
                            return false;
                        }
                    }
                }
            }
            return true;
        }

    }
}
