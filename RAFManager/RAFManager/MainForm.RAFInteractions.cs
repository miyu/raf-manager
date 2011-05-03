using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

using System.Drawing;

using ItzWarty;

using RAFLib;

using System.IO;

namespace RAFManager
{
    partial class MainForm : Form
    {
        //Names of columns - For finding cells in rows
        private const string CN_USE = "shouldUseMod";
        private const string CN_LOCALPATH = "localPathColumn";
        private const string CN_LOCALPATHPICKER = "pickLocalPathColumn";
        private const string CN_RAFPATH = "rafPathColumn";
        private const string CN_RAFPATHPICKER = "pickRafPathColumn";

        /// <summary>
        /// Initializes the changes view - Sizes columns and sets an event handler for them to autosize
        /// </summary>
        private void InitializeChangesView()
        {
            AdjustModificationsView();

            changesView.CellClick += new DataGridViewCellEventHandler(changesView_CellClick);
        }
        /// <summary>
        /// Sizes the columns in the modifications view
        /// </summary>
        private void AdjustModificationsView()
        {
            changesView.Columns[0].Width = 50;
            changesView.Columns[1].Width = (changesView.Width - 110 - 20) / 2;
            changesView.Columns[2].Width = 30;
            changesView.Columns[3].Width = (changesView.Width - 110 - 20) / 2;
            changesView.Columns[4].Width = 30;
            changesView.ScrollBars = ScrollBars.Vertical;
        }

        /// <summary>
        /// When a cell is clicked on the changes view, finds out what was clicked and interacts appropriately
        /// IE: Opening file dialog for selecting a skin, or starting a raf file selection operation
        /// </summary>
        void changesView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCell cell = changesView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            DataGridViewRow row = changesView.Rows[cell.RowIndex];

            if (cell.OwningColumn.Name == CN_RAFPATHPICKER)
            {
                string rafPath = PickRafPath();
                if (rafPath != "")
                {
                    row.Cells[CN_RAFPATH].Value = rafPath;
                    if (cell.RowIndex == changesView.Rows.Count - 1)
                    {
                        //Tell the view that the currently selected cell is "dirty", so it makes a
                        //new one under this one
                        changesView.NotifyCurrentCellDirty(true); //Gotta love these names...
                    }
                }
            }
            else if (cell.OwningColumn.Name == CN_LOCALPATHPICKER)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.ShowDialog();

                if (ofd.FileName != "")
                {
                    row.Cells[CN_LOCALPATH].Value = ofd.FileName;
                    if (cell.RowIndex == changesView.Rows.Count - 1)
                    {
                        //Tell the view that the currently selected cell is "dirty", so it makes a
                        //new one under this one
                        changesView.NotifyCurrentCellDirty(true); //Gotta love these names...    
                    }
                }
            }
        }

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
                if (entry.FileName.ToLower().EndsWith("inibin") || entry.FileName.ToLower().EndsWith("troybin"))
                {
                    new TextViewer(this.baseTitle + " - inibin/troybin view - " + nodeInternalPath,
                        new InibinFile().main(entry.GetContent())
                    ).Show();
                }
                else if (entry.FileSize < 10000 || //If > 200, ask, then continue
                       MessageBox.Show("This file is quite large ({0} bytes).  Sure you want to read it?".F(entry.FileSize), "", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (entry.GetContent().All(c => c >= ' ' && c <= '~')) //All content is ascii
                    {
                        new TextViewer(this.baseTitle + " - Text View - " + nodeInternalPath,
                            Encoding.ASCII.GetString(entry.GetContent())
                        ).Show();
                    }
                    else
                    {
                        new BinaryViewer(this.baseTitle + " - Binary View by Be.HexEditor http://sourceforge.net/projects/hexbox/- " + nodeInternalPath,
                            entry.GetContent()
                        ).Show();
                    }
                }
            }
        }
    }
}
