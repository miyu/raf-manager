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
    public partial class RAFManagerCleanWizard : Form
    {
        private enum CleanWizardState:int
        {
            Introduction = 1,
            BackupWarning = 2,
            Analyzing = 3,
            Minifying = 4
        }

        BackgroundWorker bw = new BackgroundWorker();

        CleanWizardState guiState = CleanWizardState.Introduction;
        Panel activePanel = null;

        RAFArchive[] archives = null;

        public RAFManagerCleanWizard(RAFArchive[] archives)
        {
            InitializeComponent();

            //By default, it's fully highlighted - this unfocuses it, basically
            introductionTextbox.Select(introductionTextbox.Text.Length, 0);

            this.archives = archives;

            ManageGUI();
        }

        private void ManageGUI()
        {
            switch (guiState)
            {
                case CleanWizardState.Introduction:
                    Panel1_Introduction.Show();
                    Panel2_BackupWarning.Hide();
                    Panel3_Analyzing.Hide();
                    Panel4_Minifying.Hide();
                    activePanel = Panel1_Introduction;
                    break;
                case CleanWizardState.BackupWarning:
                    Panel1_Introduction.Hide();
                    Panel2_BackupWarning.Show();
                    Panel3_Analyzing.Hide();
                    Panel4_Minifying.Hide();
                    activePanel = Panel2_BackupWarning;
                    break;
                case CleanWizardState.Analyzing:
                    Panel1_Introduction.Hide();
                    Panel2_BackupWarning.Hide();
                    Panel3_Analyzing.Show();
                    Panel4_Minifying.Hide();
                    activePanel = Panel3_Analyzing;
                    break;
                case CleanWizardState.Minifying:
                    Panel1_Introduction.Hide();
                    Panel2_BackupWarning.Hide();
                    Panel3_Analyzing.Hide();
                    Panel4_Minifying.Show();
                    activePanel = Panel4_Minifying;
                    break;
            }
            this.ClientSize = new Size(activePanel.Width + 4, activePanel.Height + 4);
            activePanel.Left = 2;
            activePanel.Top = 2;
        }

        private void BeginAnalyzeArchives()
        {
            Console.WriteLine("Begin analyzing archives");
            bw.DoWork += bw_AnalyzeArchiveWorker_DoWork;
            //Percentage progress = unimportant/not looked at. Userstate: [Current entry # (starting at 0), current archive # (starting at 0), bytes save-able]
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_AnalyzeProgressChanged);
            bw.WorkerReportsProgress = true;
            bw.RunWorkerAsync(archives);
        }

        void bw_AnalyzeProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("Progress Changed");
            long[] userState = (long[])e.UserState;
            long currentEntry = userState[0];
            long currentArchive = userState[1];
            long bytesWasted = userState[2];

            Panel3_ArchiveProgressBar.Maximum = archives.Length;
            Panel3_ArchiveProgressBar.Value = (int)currentArchive;
            Panel3_ArchiveProgressBar.AText = archives[currentArchive].GetID();

            Panel3_EntryProgressBar.Maximum = archives[currentArchive].GetDirectoryFile().GetFileList().GetFileEntries().Count;
            Panel3_EntryProgressBar.Value = (int)currentEntry;
            Panel3_EntryProgressBar.AText = archives[currentArchive].GetDirectoryFile().GetFileList().GetFileEntries()[(int)currentEntry].FileName;
            Panel3_EstimatedSpaceFreedLabel.Text = "Estimated Space Freed: " + bytesWasted.ToFileSize();
        }
        void bw_AnalyzeArchiveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("Analyzing - doing work");
            BackgroundWorker bw = (BackgroundWorker)sender;
            RAFArchive[] archives = (RAFArchive[])e.Argument;

            TimeSpan updateInterval = new TimeSpan(0, 0, 0, 0, 100);
            DateTime lastUpdate = DateTime.Now - updateInterval;

            long bytesWasted = 0;
            Console.WriteLine("Enter for.  Archives count: "+archives.Length);
            for (int i = 0; i < archives.Length; i++)
            {
                RAFArchive archive = archives[i];

                long archiveBytesUsed = 0; //We add to this each time, then find out how many bytes could have been saved.

                List<RAFFileListEntry> entries = archive.GetDirectoryFile().GetFileList().GetFileEntries();
                Console.WriteLine("Analyzing Archive: " + archive.GetID() +"; "+entries.Count+" entries");
                for (int j = 0; j < entries.Count; j++)
                {
                    //Console.WriteLine("  " + j);
                    //Report progress to main thread
                    if (j % 100 == 0)
                    {
                        bw.ReportProgress(0, new long[] { j, i, bytesWasted });
                        lastUpdate = DateTime.Now;
                    }
                    //Console.WriteLine("  !");
                    archiveBytesUsed += entries[j].FileSize;
                }
                bytesWasted += new FileInfo(archive.RAFFilePath + ".dat").Length - archiveBytesUsed;
                Console.WriteLine("Current Bytes Wasted: " + bytesWasted);
            }
        }

        private void BeginMinifyArchive(RAFArchive archive)
        {
            bw.RunWorkerAsync(archive);
        }
        void bw_MinifyArchiveWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = (BackgroundWorker)sender;
            RAFArchive archive = (RAFArchive)e.Argument;

            Console.WriteLine("Minifying Archive: " + archive.GetID());
            //Create temporary dat file
            Console.WriteLine("-> Creating temporary file");
            string tempDatFilePath = archive.RAFFilePath + ".dat.temp";
            FileStream datFs = File.Create(tempDatFilePath);

            Console.WriteLine("-> Begin Writing New Entries");
            List<RAFFileListEntry> entries = archive.GetDirectoryFile().GetFileList().GetFileEntries();
            for (int i = 0; i < entries.Count; i++)
            {
                //Report progress to main thread
                if ((i % 100) == 0)
                    bw.ReportProgress((int)(i * 100 / entries.Count), entries[i].FileName);

                byte[] rawContent = entries[i].GetRawContent();
                datFs.Write(rawContent, 0, rawContent.Length);
            }
            Console.WriteLine("    ->Done");
            Console.WriteLine("-> Delete old Archive File... (Closing File Stream)");
            archive.GetDataFileContentStream().Close(); //First we close the old .dat file stream, so we can replace the unowned file
            Console.WriteLine("   Stream Closed... Now deleting old DAT file");
            File.Delete(archive.RAFFilePath + ".dat");
            Console.WriteLine("   Move Replacement DAT File...");
            File.Move(tempDatFilePath, archive.RAFFilePath + ".dat");

            Console.WriteLine("-> Generate new Archive Directory File (*.raf).");
            archive.SaveDirectoryFile();
            Console.WriteLine("-> Done");
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            this.guiState++;
            ManageGUI();

            if (this.guiState == CleanWizardState.Analyzing)
                BeginAnalyzeArchives();
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            this.guiState--;
            ManageGUI();
        }
    }
}
