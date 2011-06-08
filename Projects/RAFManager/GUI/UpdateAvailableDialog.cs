using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RAFManager
{
    public partial class UpdateAvailableDialog : Form
    {
        /// <summary>
        /// Opens a generic ambiguity resolver
        /// 
        /// It takes in values of any type and provides a selection box for them.
        /// </summary>
        public UpdateAvailableDialog(string patchNotes, string newVersion)
        {
            InitializeComponent();
            
            this.updateNotesTextbox.Text = patchNotes;

            updateButton.Click += new EventHandler(updateButton_Click);
            cancelButton.Click += new EventHandler(cancelButton_Click);

            this.messageLabel.Text = this.messageLabel.Text.Replace("{0}", newVersion);

            this.Resize += new EventHandler(UpdateAvailableDialog_Resize);
            this.ResizeEnd += new EventHandler(UpdateAvailableDialog_ResizeEnd);
        }

        /// <summary>
        /// When the cancel button is clicked, deselect the current entry and close the form
        /// We deselect because the SelectedItem field
        /// returns the selected item...
        /// </summary>
        void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Manages the GUI when the window is resized
        /// </summary>
        void UpdateAvailableDialog_Resize(object sender, EventArgs e)
        {
            this.updateNotesTextbox.Width = this.ClientSize.Width - this.updateNotesTextbox.Left * 2;
            this.cancelButton.Left = this.updateNotesTextbox.Right - this.cancelButton.Width;
            this.updateButton.Left = this.cancelButton.Left - this.updateButton.Width - 6;

            this.updateButton.Top = this.ClientSize.Height - this.updateButton.Height - 6;
            this.cancelButton.Top = this.ClientSize.Height - this.updateButton.Height - 6;

            this.updateNotesTextbox.Height = this.ClientSize.Height - this.updateNotesTextbox.Top - this.updateButton.Height - 12;
        }

        /// <summary>
        /// Updates the GUI when the window is resized
        /// </summary>
        void UpdateAvailableDialog_ResizeEnd(object sender, EventArgs e)
        {
            UpdateAvailableDialog_Resize(sender, e);
        }

        /// <summary>
        /// When an item is selected, enable the "Done" button
        /// </summary>
        void optionsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateButton.Enabled = true;
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            //perform update
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
            Application.Exit();
        }

        void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("update.exe");
        }
    }
}
