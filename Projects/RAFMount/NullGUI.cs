using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RAFMount
{
    public class NullGUI:RAFManager.IRAFManagerGUI
    {
        public void Init(RAFManager.RAFManager rafManager)
        {
        }
        public void Log(string s)
        {
            Console.WriteLine(s);
        }

        public void DLog(string s)
        {
            Console.WriteLine(s);
        }

        public void ShowAboutWindow()
        {
        }

        public void ShowUpdateAvailableWindow(string patchNotes, string newVersion)
        {
        }

        public void ShowLoaderWindow()
        {
        }

        public void HideLoaderWindow()
        {
        }

        public void ClearLoaderWindow()
        {
        }

        public void PrepareForShow()
        {
        }

        public System.Windows.Forms.Form GetMainWindow()
        {
            return null;
        }

        public void LogToLoader(string line)
        {
            Console.WriteLine("L: "+line);
        }

        public void SetLastLoaderLine(string line)
        {
            Console.WriteLine("L: " + line);
        }
    }
}
