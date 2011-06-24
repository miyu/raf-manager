using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using System.Windows.Forms;

using RAFLib;
using ItzWarty;

namespace RAFManager
{
    public class RAFManagerClass
    {
        IRAFManagerGUI gui = null;
        public TextWriter oWriter { get; set; }
        
        public IniFile Config { get; set; }

        public List<RAFArchive> Archives { get; set; }

        public List<RAFInMemoryFileSystemObject> ArchiveFSOs { get; set; }


        /// <summary>
        /// Calls InitializeRAFManager after setting its precondition
        /// </summary>
        public RAFManagerClass(IRAFManagerGUI gui)
        {
            this.gui = gui;
            this.gui.Init(this);

            oWriter = Console.Out;
            oWriter.WriteLine("RAFManager.ctor CurrentDirectory: " + Environment.CurrentDirectory);
            InitializeRAFManager();
        }
        /// <summary>
        /// Initializes Configuration file
        /// Resolves location of file archives path
        /// Loads File Archives
        /// 
        /// Precondition: gui is defined
        /// </summary>
        private void InitializeRAFManager()
        {
            //Show the RAF Manager is Loading window.
            gui.ShowLoaderWindow();
            gui.ClearLoaderWindow();

            gui.LogToLoader("Begin Async Update Check");
            CheckForUpdates();

            gui.LogToLoader("Load Configuration File");
            InitializeConfigurationFile();

            gui.LogToLoader("Resolve 'filearchives' Path");
            ResolveFileArchivesPath();  //Find where the raf archives are

            gui.LogToLoader("Load RAF Archives...");
            LoadRAFArchives();

            gui.LogToLoader("Prepare GUI Environment");
            gui.PrepareForShow();

            gui.HideLoaderWindow();

            Form mainWindow = gui.GetMainWindow();
            if (mainWindow != null) Application.Run(mainWindow);
        }

        /// <summary>
        /// Initializes the configuration file.
        /// </summary>
        private void InitializeConfigurationFile()
        {
            Config = new IniFile(Environment.CurrentDirectory+"/"+"rmconfig.ini");
        }

        //Location of the filearchives directory, where the version (0.0.0.xyz) directories are stored
        string fileArchivesPath = null;

        /// <summary>
        /// Sets the fileArchivesPath to the correct path, or kills the application.
        /// It first looks at the config file.  If that fails [as in, the path isn't set]
        /// it looks at the expected path.  If that fails, it prompts the user to find
        /// the LoL root folder.
        /// </summary>
        private void ResolveFileArchivesPath()
        {
            //Our Override
            if (Config["Application.lolroot"] != "")
            {
                oWriter.WriteLine("Use config file for lol root");
                string expectedLolRoot = Config["Application.lolroot"];
                string expectedFileArchivesPath = expectedLolRoot + @"\RADS\projects\lol_game_client\filearchives\";
                if (Directory.Exists(expectedLolRoot) && Directory.Exists(expectedFileArchivesPath))
                {
                    oWriter.WriteLine("->lol: " + expectedLolRoot);
                    oWriter.WriteLine("->" + expectedFileArchivesPath);
                    fileArchivesPath = expectedFileArchivesPath;
                    return;
                }
            }

            //If we get here, no override existed, or override was invalid
            //Expected path - default install location of the filearchives directory
            string expectedPath = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\";
            if (Directory.Exists(expectedPath))
            {
                oWriter.WriteLine("Use expected path for lol root");
                fileArchivesPath = expectedPath;
                Config["Application.lolroot"] = @"C:\Riot Games\League of Legends\";
            }
            else
            {
                //Have the user select the LoL directory... 
                oWriter.WriteLine("Use file browser for lol root");
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Please locate your League of Legends Folder (The folder that contains lol.launcher.exe)";
                fbd.ShowDialog();

                string result = fbd.SelectedPath;
                if (Directory.Exists(result + @"\RADS\projects\lol_game_client\filearchives\"))
                {
                    //success
                    fileArchivesPath = result + @"\RADS\projects\lol_game_client\filearchives\";
                    //save
                    Config["Application.lolroot"] = result;
                }
                else
                {
                    //We couldn't find the filearchives directory.  Exit the application, as we can't do anything now.
                    MessageBox.Show("Invalid directory: \r\n" + result + @"\RADS\projects\lol_game_client\filearchives\", "Couldn't find directory");
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
        }
        /// <summary>
        /// Gets the file archives path.
        /// </summary>
        public string GetFileArchivesPath()
        {
            return fileArchivesPath;
        }

        /// <summary>
        /// Loads the RAF Archives
        /// </summary>
        public void LoadRAFArchives()
        {
            ArchiveFSOs = new List<RAFInMemoryFileSystemObject>();
            Archives = new List<RAFArchive>();
            TreeView temp = new TreeView();
            //temp.TreeViewNodeSorter = new RAFFSOTreeNodeSorter();

            string[] archivePaths = Directory.GetDirectories(fileArchivesPath);

            for (int i = 0; i < archivePaths.Length; i++)
            {
                string archiveName = archivePaths[i].Replace(fileArchivesPath, "").Replace("/", "");

                gui.LogToLoader("Load Archive - " + archiveName + " [0%]");
                //Title("Loading RAF File - " + archiveName);
                //Log("Loading RAF Archive Folder: " + archiveName);
                RAFArchive raf = null;
                RAFInMemoryFileSystemObject archiveRoot = new RAFInMemoryFileSystemObject(null, RAFFSOType.ARCHIVE, archiveName);
                temp.Nodes.Add(archiveRoot);
                ArchiveFSOs.Add(archiveRoot);
#if !DEBUG
                try
                {
#endif
                //Load raf file table and add to our list of archives
                Archives.Add(
                    raf = new RAFArchive(
                        Directory.GetFiles(archivePaths[i], "*.raf")[0]
                    )
                );

                //Enumerate entries and add to our tree... in the future this should become sorted
                List<RAFFileListEntry> entries = raf.GetDirectoryFile().GetFileList().GetFileEntries();
                for (int j = 0; j < entries.Count; j++)
                {
                    // Console.WriteLine(entries[j].StringNameHash.ToString("x").PadLeft(8, '0').ToUpper());
                    if (j % 1000 == 1000)
                        gui.SetLastLoaderLine("Load Archive - " + archiveName + " [" + (j * 100 / entries.Count) + "%]");
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
                        lock (gui)
                        {
                            gui.Log("The program is up to date.  RAFManager just checked!");
                            gui.Log("Update Domain: " + RAFManagerUpdater.Autoupdater.updateDomain);
                        }
                    }
                    else
                    {
                        lock (gui)
                        {
                            gui.Log("Unable to connect to update server.");
                            gui.Log("Please check http://www.leagueoflegends.com/board/showthread.php?t=704945");
                            gui.Log("For more information, or redownload the client at www.ItzWarty.com/RAF/");
                        }
                    }
                    return null;
                }
            );
        }

        /// <summary>
        /// Resolves RAF Path, including RafID in path, to fso
        /// </summary>
        public RAFInMemoryFileSystemObject ResolveRAFPathTOFSO(string path)
        {
            path = path.Replace("\\", "/");
            if (path[0] == '/') path = path.Substring(1);
            string archiveId = path.Split("/")[0];
            List<RAFInMemoryFileSystemObject> archiveFSOs =
                new List<RAFInMemoryFileSystemObject>(
                    ArchiveFSOs.Where(
                        (Func<RAFInMemoryFileSystemObject, bool>)delegate(RAFInMemoryFileSystemObject fso)
                        {
                            return fso.Name.ToLower() == archiveId.ToLower();
                        }
                    )
                );
            if (archiveFSOs.Count == 0) return null;
            else
            {
                string innerPath = path.Replace(archiveId+"/", "").Replace(archiveId, "");
                Console.WriteLine("inner path: " + innerPath);
                if (innerPath == "")
                    return archiveFSOs[0];
                else
                    return archiveFSOs[0].GetChildFSO(innerPath);
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

            foreach (RAFArchive archive in Archives)
            {
                if (archive.GetID() == archiveId)
                {
                    return archive.GetDirectoryFile().GetFileList().GetFileEntry(internalPath);
                }
            }
            return null;
        }
    }
}
