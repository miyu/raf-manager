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

            changesView.Rows.Clear();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = ".rmproj";
            dialog.AddExtension = true;
            dialog.Filter = "RAFManager Projects(*.rmproj)|*.rmproj";
            dialog.ShowDialog();
            if (dialog.CheckPathExists)
            {
                SaveProject(dialog.FileName);
            }
        }
        private void SaveProject(string location)
        {
            changesView.ClearSelection();
            /**
             * Output:
             * ProjectName
             * PathToProject [reserved, leave as space]
             * RafDirectory [reserved, leave as space]
             * 1 localPathTag | rafPathIncludingArchive
             * 1 localPathTag | rafPathIncludingArchive
             * 1 localPathTag | rafPathIncludingArchive
             * ...etc
             * 
             * 1 is either 1 or 0.  1 = use (check the box), 0 = don't use (uncheck)
             * localPath and rafPath should be trimmed before use.
             */
            string serialization = projectInfo.ProjectName;
            serialization += "\n" + " ";
            serialization += "\n" + " ";
            for (int i = 0; i < changesView.RowCount; i++)
            {
                DataGridViewRow row = changesView.Rows[i];
                RAFFileListEntry entry = ((RAFFileListEntry)row.Cells[CN_RAFPATH].Tag);
                if (entry != null)
                {
                    bool check = true;
                    if (row.Cells[CN_USE].Value == null)
                        check = false;
                    else
                        check = (bool)row.Cells[CN_USE].Value;

                    serialization += "\n" + (check ? "1" : "0")
                        + " " + (string)row.Cells[CN_LOCALPATH].Tag
                        + " | " + entry.RAFArchive.GetID() + "/" + entry.FileName;
                }
            }
            HasProjectChanged = false;
            File.WriteAllText(location, serialization);
            UpdateProjectGUI();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".rmproj";
            dialog.AddExtension = true;
            dialog.Filter = "RAFManager Projects(*.rmproj)|*.rmproj";
            dialog.ShowDialog();
            if (dialog.CheckPathExists)
            {
                LoadProject(dialog.FileName);
            }
        }
        private void LoadProject(string location)
        {
            //Get a clean project first, before we load in contents
            ResetProject();

            string[] lines = File.ReadAllLines(location);
            string projectName = lines[0];
            string projectPath = lines[1];
            string rafDirectory= lines[2];

            projectNameTb.Text = projectName;
            projectInfo.ProjectName = projectName.Trim();
            projectInfo.ProjectPath = projectPath.Trim();
            projectInfo.FileArchivesDirectory = rafDirectory;
            for (int i = 3; i < lines.Length; i++)
            {
                string line = lines[i];
                bool check = line[0] == '1';
                string afterCheck = line.Substring(1); //get everything after the check
                string[] parts = afterCheck.Split("|"); //yields {localPath, rafPath}
                string localPath = parts[0].Trim();
                string rafPath = parts[1].Trim();       //includes RAF Archive Id (0.0.0.xx)

                int rowId = changesView.Rows.Add();

                changesView.Rows[rowId].Cells[CN_USE].Value = check;

                changesView.Rows[rowId].Cells[CN_LOCALPATH].Value = localPath;
                changesView.Rows[rowId].Cells[CN_LOCALPATH].Tag = localPath;

                changesView.Rows[rowId].Cells[CN_RAFPATH].Value = rafPath;
                changesView.Rows[rowId].Cells[CN_RAFPATH].Tag = ResolveRAFPathToEntry(rafPath);
            }

            UpdateProjectGUI();
            UpdateChangesGUI();
        }

        private void UpdateProjectGUI()
        {
            projectNameTb.Text = this.projectInfo.ProjectName;
            Title(GetWindowTitle());
        }
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
    }
}
