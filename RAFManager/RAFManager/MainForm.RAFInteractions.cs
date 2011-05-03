using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace RAF_Packer
{
    partial class MainForm : Form
    {
        //Names of columns
        private const string CN_USE = "shouldUseMod";
        private const string CN_LOCALPATH = "localPathColumn";
        private const string CN_LOCALPATHPICKER = "pickLocalPathColumn";
        private const string CN_RAFPATH = "rafPathColumn";
        private const string CN_RAFPATHPICKER = "pickRafPathColumn";

        private void InitializeChangesView()
        {
            AdjustModificationsView();

            changesView.CellClick += new DataGridViewCellEventHandler(changesView_CellClick);
        }

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
    }
}
