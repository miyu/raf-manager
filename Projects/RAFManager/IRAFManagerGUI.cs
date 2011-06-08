using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFManager
{
    public interface IRAFManagerGUI
    {
        void Init(RAFManager rafManager);

        void Log(string s);
        void DLog(string s);

        /// <summary>
        /// Shows an about box for RAF Manager
        /// </summary>
        void ShowAboutWindow();

        /// <summary>
        /// Shows update available prompt
        /// </summary>
        /// <returns>Boolean - Whether or not user wishes to update RAF Manager</returns>
        void ShowUpdateAvailableWindow(string patchNotes, string newVersion);

        /// <summary>
        /// Shows the RAF Manager is Loading window...
        /// </summary>
        void ShowLoaderWindow();

        /// <summary>
        /// Hides the RAF Manager is Loading window...
        /// </summary>
        void HideLoaderWindow();

        /// <summary>
        /// Clears the loader window's log contents.
        /// </summary>
        void ClearLoaderWindow();

        void LogToLoader(string line);

        void SetLastLoaderLine(string line);

        void PrepareForShow();

        System.Windows.Forms.Form GetMainWindow();
    }
}
