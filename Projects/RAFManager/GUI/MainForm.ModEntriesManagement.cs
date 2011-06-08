using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ItzWarty;
using RAFLib;
using System.IO;

using System.Diagnostics;

namespace RAFManager
{
    public partial class MainForm:Form
    {
        public void InitializeModEntriesManagement()
        {
            modEntriesPanel.AllowDrop = true;
            modEntriesPanel.DragOver += new DragEventHandler(modEntriesPanel_DragOver);
            modEntriesPanel.DragDrop += new DragEventHandler(modEntriesPanel_DragDrop);
        }

        /// <summary>
        /// Dragdrop operations - Allow file groups to be dropped in only.
        /// </summary>
        void modEntriesPanel_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
            {
                e.Effect = DragDropEffects.Copy;

                rafContentView.Select();
                TreeNode hoveredNode = rafContentView.GetNodeAt(rafContentView.PointToClient(new Point(e.X, e.Y)));
                rafContentView.SelectedNode = hoveredNode;
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Dragdrop - process file drops
        /// Add them if appropriate
        /// </summary>
        void modEntriesPanel_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data is DataObject && ((DataObject)e.Data).ContainsFileDropList())
            {
                StringCollection filePaths = ((DataObject)e.Data).GetFileDropList();

                //Get all files that were dropped in, by searching subdirectories too
                List<String> resultFiles = new List<string>();
                foreach (string path in filePaths)
                    if ((new FileInfo(path).Attributes & FileAttributes.Directory) != 0)
                    {
                        resultFiles.AddRange(Util.GetAllChildFiles(path));
                    }
                    else
                        resultFiles.Add(path);

                List<TreeNode> resultantNodes = new List<TreeNode>();
                string iconPath = null;

                //If there is a script it overrides default action
                RMPropInterpreter script = null;
                foreach (string path in resultFiles)
                {
                    string rafPath = GuessRafPathFromPath(path);
                    if (rafPath != "undefined")
                    {
                        TreeNode node = new TreeNode(path.Replace("\\", "/").Split("/").Last());
                        node.Nodes.Add("Local Path: " + path);
                        node.Nodes.Add("RAF Path: " + rafPath);
                        node.Tag = new ModEntryNodeTag()
                        {
                            localPath = path,
                            rafPath = rafPath
                        };
                        resultantNodes.Add(node);
                    }
                    else
                    {   //We usually skip it if it is undefined.  We have some special cases
                        if (path.EndsWith("rafmanagericon.jpg"))
                        {
                            iconPath = path;
                        }
                        else if (path.EndsWith("rafmanagerscript"))
                        {
                            try
                            {
                                script = new RMPropInterpreter(
                                    path,
                                    this
                                );
                            }catch(Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }else
                            Log("Unable to resolve local path to RAF path: " + path);
                    }
                }

                ModEntry modEntry = null;
                if (script == null)
                {
                    //resultFiles has all our files.
                    StringQueryDialog sqd = new StringQueryDialog("What is the name of the mod that you are dropping into RAF Manager?");
                    sqd.ShowDialog();
                    if (sqd.Value == "") return;
                    modEntry = CreateAndAppendModEntry(sqd.Value, "-", "-", resultantNodes.ToArray());
                }
                else
                {
                    modEntry = CreateAndAppendModEntry(script.Name, script.Creator, script.WebsiteURL, resultantNodes.ToArray());
                    modEntry.Script = script;
                }

                try
                {
                    if (iconPath != null)
                    {
                        modEntry.IconImage = Bitmap.FromFile(iconPath);
                        modEntry.IconImage.Tag = iconPath;
                    }
                }
                catch { }
            }
        }


        ///<summary>
        ///Returns a guess of the RAF Path, including the archive id or "undefined"
        ///</summary>
        public string GuessRafPathFromPath(string basePath)
        {
            string[] pathParts = basePath.Replace("\\","/").Split("/");
            RAFFileListEntry matchedEntry = null;
            List<RAFFileListEntry> lastMatches = null;
            bool done = false;

            //Smart search insertion
            for (int i = 1; i < pathParts.Length + 1 && !done; i++)
            {
                string[] searchPathParts = pathParts.SubArray(pathParts.Length - i, i);
                string searchPath = String.Join("/", searchPathParts);
                //Console.WriteLine(searchPath);
                List<RAFFileListEntry> matches = new List<RAFFileListEntry>();
                RAFArchive[] archives = rafArchives.Values.ToArray();
                for (int j = 0; j < archives.Length; j++)
                {
                    List<RAFFileListEntry> newmatches = archives[j].GetDirectoryFile().GetFileList().SearchFileEntries(searchPath);
                    matches.AddRange(newmatches);
                }
                if (matches.Count == 1)
                {
                    matchedEntry = matches[0];
                    done = true;
                }
                else if (matches.Count == 0)
                {
                    done = true;
                }
                else
                {
                    lastMatches = matches;
                }
            }
            if (matchedEntry == null)
            {
                if (lastMatches != null && lastMatches.Count > 0)
                {
                    //Resolve ambiguity
                    FileEntryAmbiguityResolver ambiguityResolver = new FileEntryAmbiguityResolver(lastMatches.ToArray(), "Notes: "+basePath);
                    ambiguityResolver.ShowDialog();
                    RAFFileListEntry resolvedItem = (RAFFileListEntry)ambiguityResolver.SelectedItem;
                    if (resolvedItem != null)
                    {
                        matchedEntry = resolvedItem;
                    }
                }
                else if (permitExperimentalFileAddingCB.Checked)//advanced user
                {
                    //We'll use the file browser to select where we want to save...
                    string rafPath = PickRafPath(false) + "/";
                    RAFArchive archive = rafArchives[rafPath.Replace("\\", "/").Split("/").First()];
                    rafPath = rafPath.Substring(rafPath.IndexOf("/") + 1); //remove the archive name now...
                    if (rafPath.Length != 0)
                    {
                        Console.WriteLine("FRP: " + "len!= 0");
                        if (rafPath[rafPath.Length - 1] == '/')
                        {
                            Console.WriteLine("FRP: " + rafPath);
                            rafPath = rafPath.Substring(0, rafPath.Length - 1);//remove the trailing /, since we add it later
                        }
                    }
                    Console.WriteLine("FRP: " + rafPath);
                    if (rafPath == "")
                        matchedEntry = new RAFFileListEntry(archive, pathParts.Last(), UInt32.MaxValue, (UInt32)new FileInfo(basePath).Length, UInt32.MaxValue);
                    else
                        matchedEntry = new RAFFileListEntry(archive, rafPath + "/" + pathParts.Last(), UInt32.MaxValue, (UInt32)new FileInfo(basePath).Length, UInt32.MaxValue);

                    //Add the tree node to the raf viewer
                }
            }
            if (matchedEntry != null) //If it is resolved
            {
                /*
                node.Tag = new ChangesViewEntry(filePath, matchedEntry, node);
                node.Nodes.Add(new TristateTreeNode("Local Path: " + filePath));
                node.Nodes.Add(new TristateTreeNode("RAF Path: " + matchedEntry.RAFArchive.GetID() + "/" + matchedEntry.FileName));
                node.Nodes[0].HasCheckBox = false;
                node.Nodes[1].HasCheckBox = false;
                 */
                //Don't add it
                return matchedEntry.RAFArchive.GetID() + "/" + matchedEntry.FileName;
                //changesView.Rows[rowIndex].Cells[CN_RAFPATH].Value = matchedEntry.RAFArchive.GetID() + "/" + matchedEntry.FileName;
                //changesView.Rows[rowIndex].Cells[CN_RAFPATH].Tag = matchedEntry;
            }
            else
            {
                /*
                node.Tag = new ChangesViewEntry(filePath, null, node);
                node.Nodes.Add(new TristateTreeNode("Local Path: " + filePath));
                node.Nodes.Add(new TristateTreeNode("RAF Path: " + "undefined"));
                node.Nodes[0].HasCheckBox = false;
                node.Nodes[1].HasCheckBox = false;
                Log("Unable to link file '" + filePath + "' to RAF Archive.  Please manually select RAF path");
                 */
                //Log("Unable to resolve local path to RAF path: " + basePath);
                return "undefined";
            }
        }
    }
}
