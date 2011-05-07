using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using RAFLib;


using System.IO;
using ItzWarty;

namespace RAFManager
{
    partial class MainForm:Form
    {
        private string baseTitle = null;

        public void InitializeUtil()
        {
            this.baseTitle = this.Text;
        }
        /// <summary>
        /// Sets the title of our form
        /// </summary>
        /// <param name="s"></param>
        private void Title(string s) { this.Text = baseTitle + " - " + s; Application.DoEvents(); }

        /// <summary>
        /// Allows the user to pick a RAF file from the rafContentView control...
        /// 
        /// TODO: 5 second timer for stopping
        /// TODO: This needs to be made better.  it's pretty annoying to work with on the user-viewpoint
        /// </summary>
        /// <returns></returns>
        private string PickRafPath()
        {
            RAFInMemoryFileSystemObject[] nodes = new RAFInMemoryFileSystemObject[this.rafContentView.Nodes.Count];
            for (int i = 0; i < nodes.Length; i++)
                nodes[i] = (RAFInMemoryFileSystemObject)this.rafContentView.Nodes[i].Clone();
            RAFPathSelector selectorDialog = new RAFPathSelector(nodes);
            selectorDialog.ShowDialog();
            return selectorDialog.SelectedNodePath;
        }
        /// <summary>
        /// In this case, rafpack includes the preceeding 0.0.0.xx/
        /// </summary>
        /// <param name="rafPath"></param>
        /// <returns></returns>
        private RAFFileListEntry ResolveRAFPathToEntry(string rafPath)
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
                        Log("The program is up to date.  RAFManager just checked!");
                        Log("Update Domain: " + RAFManagerUpdater.Autoupdater.updateDomain);
                    }
                    else
                    {
                        Log("Unable to connect to update server.");
                        Log("Please check http://www.leagueoflegends.com/board/showthread.php?t=704945");
                        Log("For more information, or redownload the client at www.ItzWarty.com/RAF/");
                    }
                    return null;
                }
            );
        }
        /// <summary>
        /// Creates the given directory and all directories leading up to it.
        /// </summary>
        private static void PrepareDirectory(string path)
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

        private void SetArchivesRoot()
        {
            string expectedPath = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";

            if(Directory.Exists(expectedPath)) archivesRoot = expectedPath;
            else if(File.Exists("riotgamesroot.txt"))
            {
                string lastPath = File.ReadAllText("riotgamesroot.txt");
                if(Directory.Exists(lastPath))
                {
                    archivesRoot = lastPath+@"\League of Legends\RADS\projects\lol_game_client\filearchives\";
                    return;
                }
            }else{
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Select your Riot Games folder";
                fbd.ShowDialog();

                string result = fbd.SelectedPath;
                if (Directory.Exists(result + @"\League of Legends\RADS\projects\lol_game_client\filearchives\"))
                {
                    //success
                    archivesRoot = result + @"\League of Legends\RADS\projects\lol_game_client\filearchives\";
                    //save
                    File.WriteAllText("riotgamesroot.txt", result);
                }
                else
                {
                    MessageBox.Show("Invalid directory: \r\n"+result + @"\League of Legends\RADS\projects\lol_game_client\filearchives\", "Couldn't find directory");
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
        }
    }
}
