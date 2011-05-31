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

using System.Diagnostics;

namespace RAFManager
{
    public partial class MainForm:Form
    {
        public void InitializeRAFContentViewManagement()
        {
            rafContentView.NodeMouseDoubleClick += new TreeNodeMouseClickEventHandler(rafContentView_NodeMouseDoubleClick);
            rafContentView.AllowDrop = true;
            rafContentView.DragOver += new DragEventHandler(rafContentView_DragOver);
            rafContentView.MouseClick += new MouseEventHandler(rafContentView_MouseClick);

        }
        /// <summary>
        /// When the RAF Content view has a dragover operation
        /// ???  Likely have an insert file operation, though
        /// I think this wouldn't be friendly to the user.
        /// 
        /// Dragging to the changesview is what the user wants more
        /// 
        /// Perhaps show a dialog if the user actually intends to
        /// add to the dragged over dialog
        /// </summary>
        void rafContentView_DragOver(object sender, DragEventArgs e)
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

        void rafContentView_MouseClick(object sender, MouseEventArgs e)
        {
            rafContentView.SelectedNode = rafContentView.GetNodeAt(e.Location);
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                RAFInMemoryFileSystemObject fso = (RAFInMemoryFileSystemObject)rafContentView.SelectedNode;
                ContextMenu cm = new ContextMenu();
                if (fso != null)
                {
                    MenuItem dump = new MenuItem("Dump");
                    #region Dump Menu Entry
                    dump.Click += delegate(Object sender2, EventArgs e2)
                    {
                        if (fso.GetFSOType() == RAFFSOType.ARCHIVE || fso.GetFSOType() == RAFFSOType.DIRECTORY)
                        {
                            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
                            folderDialog.Description = "Select the folder where you wish to dump the files";
                            folderDialog.ShowDialog();
                            if (folderDialog.SelectedPath != "")
                            {
                                Title("Begin Dumping: " + fso.GetRAFPath());
                                DumpRafArchiveByFSO(fso, folderDialog.SelectedPath);
                                Log("Done dumping: " + fso.GetRAFPath());
                                Title("");
                            }
                        }
                        else
                        {
                            SaveFileDialog sfd = new SaveFileDialog();
                            string fileName = fso.GetRAFPath().Replace("\\", "/").Split("/").Last();
                            string fileExt = fileName.Substring(fileName.LastIndexOf(".") + 1);
                            sfd.Filter = "File|." + fileExt;
                            sfd.FileName = fileName;
                            sfd.ShowDialog();

                            if (sfd.FileName != "")
                            {
                                Log("Begin dumping: " + fileName);
                                File.WriteAllBytes(
                                    sfd.FileName,
                                    ResolveRAFPathToEntry(
                                        fso.GetTopmostParent().Text + "/" + fso.GetRAFPath()
                                    ).GetContent()
                                );
                                Log("Done dumping: " + fileName);
                            }
                        }
                    };
                    #endregion
                    cm.MenuItems.Add(dump);
                    #region file viewing entries
                    if (fso.GetFSOType() == RAFFSOType.FILE)
                    {
                        MenuItem viewAsTextFile = new MenuItem("View As Text File");
                        viewAsTextFile.Click += delegate(Object sender2, EventArgs e2)
                        {
                            new TextViewer(
                                ResolveRAFPathToEntry(
                                    fso.GetTopmostParent().Text + "/" + fso.GetRAFPath()
                                )
                            ).Show();
                        };
                        cm.MenuItems.Add(viewAsTextFile);

                        MenuItem viewAsBinary = new MenuItem("View As Binary File");
                        viewAsBinary.Click += delegate(Object sender2, EventArgs e2)
                        {
                            new BinaryViewer(this.baseTitle + " - " + fso.GetTopmostParent() + "/" + fso.GetRAFPath(),
                                ResolveRAFPathToEntry(
                                    fso.GetTopmostParent().Text + "/" + fso.GetRAFPath()
                                ).GetContent()
                            ).Show();
                        };
                        cm.MenuItems.Add(viewAsBinary);

                        if (fso.GetRAFPath().ToLower().EndsWith("inibin") || fso.GetRAFPath().ToLower().EndsWith("troybin"))
                        {
                            MenuItem viewAsINIBIN = new MenuItem("View As INIBIN File");
                            viewAsINIBIN.Click += delegate(Object sender2, EventArgs e2)
                            {
                                try
                                {
                                    new TextViewer(this.baseTitle + " - inibin/troybin view - " + fso.GetTopmostParent() + "/" + fso.GetRAFPath(),
                                        new InibinFile().main(
                                            ResolveRAFPathToEntry(
                                                fso.GetTopmostParent().Text + "/" + fso.GetRAFPath()
                                            ).GetContent()
                                        )
                                    ).Show();
                                    return;
                                }
                                catch { Log("Error parsing INIBIN/TROYBIN file"); }
                            };
                            cm.MenuItems.Add(viewAsINIBIN);
                        }
                    }
                    #endregion
                }
                MenuItem searchThis = new MenuItem("Search This Archive");
                searchThis.Click += delegate(object s2, EventArgs e2)
                {
                    new RAFSearchBox(fso).Show();
                };
                cm.MenuItems.Add(searchThis);
                MenuItem searchAll = new MenuItem("Search All Archives");
                searchAll.Click += delegate(object s2, EventArgs e2)
                {
                    RAFInMemoryFileSystemObject[] nodes = new RAFInMemoryFileSystemObject[rafContentView.Nodes.Count];
                    for (int i = 0; i < rafContentView.Nodes.Count; i++)
                        nodes[i] = (RAFInMemoryFileSystemObject)rafContentView.Nodes[i];
                    new RAFSearchBox(nodes).Show();
                };
                cm.MenuItems.Add(searchAll);
                cm.Show(rafContentView, new Point(e.X, e.Y));
            }
        }
        void DumpRafArchiveByFSO(RAFInMemoryFileSystemObject fso, string dumpDirectory)
        {
            Title("Dump: " + fso.GetRAFPath());
            if (fso.GetFSOType() == RAFFSOType.ARCHIVE || fso.GetFSOType() == RAFFSOType.DIRECTORY)
            {
                PrepareDirectory(dumpDirectory + "/" + fso.GetRAFPath());
            }
            else
            {
                //Gets the path of our containing folder in the raf archive.
                //This is akin to new FileInfo(path).DirectoryName if we had a localPath
                string containingFolderPath = fso.GetRAFPath().Replace("\\", "/").Reverse();
                containingFolderPath = containingFolderPath.Substring(Math.Max(containingFolderPath.IndexOf("/"), 0));
                containingFolderPath = containingFolderPath.Reverse();
                PrepareDirectory(dumpDirectory + "/" + fso.GetRAFPath());
                Console.WriteLine("Dump to: " + dumpDirectory + "/" + fso.GetRAFPath());
                File.WriteAllBytes(dumpDirectory + "/" + fso.GetRAFPath(),
                    ResolveRAFPathToEntry(fso.GetTopmostParent().Text + "/" + fso.GetRAFPath()).GetContent()
                );
            }
            for (int i = 0; i < fso.Nodes.Count; i++)
                DumpRafArchiveByFSO((RAFInMemoryFileSystemObject)fso.Nodes[i], dumpDirectory);
        }

        /// <summary>
        /// When a RafContentView node is double clicked, extract and view its content
        /// </summary>
        void rafContentView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            RAFInMemoryFileSystemObject node = (RAFInMemoryFileSystemObject)e.Node;
            string nodeInternalPath = node.GetRAFPath();
            if (node.GetFSOType() == RAFFSOType.FILE)
            {
                //We have double clicked a file... find out what file it was
                List<RAFFileListEntry> entries = this.rafArchives[node.GetTopmostParent().Name]
                    .GetDirectoryFile().GetFileList().GetFileEntries();

                //Find the RAF File entry that corresponds to the clicked file...
                RAFFileListEntry entry = entries.Where(
                    (Func<RAFFileListEntry, bool>)delegate(RAFFileListEntry theEntry)
                    {
                        return theEntry.FileName == nodeInternalPath;
                    }
                ).First();

                //Now select a viewer to use for the file.
                //Unnecessary for the most part, can be enabled in the future: if (entry.FileSize < 10000 || //If > 200, ask, then continue MessageBox.Show("This file is quite large ({0} bytes).  Sure you want to read it?".F(entry.FileSize), "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)

                if (entry.FileName.ToLower().EndsWith("dds"))
                    ShowDirectDrawSurface(entry.GetContent());
                else if (entry.FileName.ToLower().EndsWith("inibin") || entry.FileName.ToLower().EndsWith("troybin"))
                    ShowInibinFile(this.baseTitle + " - inibin/troybin view - " + nodeInternalPath, entry.GetContent());
                else if (entry.FileName.EndsWithAny(new string[] { ".cfg", ".ini", ".txt", ".log", ".list", ".xml" }))
                    ShowEditableTextFile(ResolveRAFPathToEntry(node.GetTopmostParent().Text + "/" + node.GetRAFPath()));
                else if (entry.FileName.EndsWithAny(new string[] { ".bmp", ".jpg" }))
                    new BitmapViewer("Bitmap Viewer", entry.GetContent()).Show();
                else //If all else fails, just use the binary viewer
                    ShowBinaryFile(this.baseTitle + " - " + nodeInternalPath, entry.GetContent());
            }
        }

        /// <summary>
        /// Shows the given content as a directdrawsurface texture file
        /// </summary>
        void ShowDirectDrawSurface(byte[] content)
        {
            //Extract it, then view it
            string fName = "temp" + (new DateTime(1970, 1, 1) - DateTime.Now).TotalMilliseconds + ".dds";
            File.WriteAllBytes(fName, content);
            Process p = Process.Start("DDSViewer.exe", fName);
            p.Exited += delegate(object s2, EventArgs e2)
            {
                File.Delete(fName);
            };
        }
        /// <summary>
        /// Presents a non-editable text file viewer to the user
        /// </summary>
        void ShowTextFile(string title, string content)
        {
            new TextViewer(title, content).Show();
        }

        /// <summary>
        /// Presents an editable text file viewer to the user
        /// </summary>
        void ShowEditableTextFile(RAFFileListEntry entry)
        {
            new TextViewer(entry).Show();
        }

        /// <summary>
        /// Presents a noneditable inibin viewer to the user
        /// </summary>
        void ShowInibinFile(string title, byte[] content)
        {
            try
            {
                new TextViewer(title,
                    new InibinFile().main(content)
                ).Show();
            }
            catch (Exception e)
            {
                Log("Inibin load failed.  e:\r\n" + e.ToString());
                Log("Will fall to binary viewer");
                ShowBinaryFile(title, content);
            }
        }
        /// <summary>
        /// Presents a noneditable binary viewer to the user
        /// </summary>
        void ShowBinaryFile(string title, byte[] content)
        {
            new BinaryViewer(title,
                content
            ).Show();
        }
    }
}
