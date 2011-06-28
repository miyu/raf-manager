using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Diagnostics;
using System.Drawing;

using ItzWarty;

using System.Runtime.InteropServices;

using System.IO;

namespace RAFManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region alphaBuildWarning
            if (RAFManagerUpdater.Versioning.CurrentVersion.flags != "")
            {
                Console.WriteLine("Note: This is a beta/alpha build of RAF Manager.  Not all features have been");
                Console.WriteLine("      thoroughly tested. Expect the worst to happen.  You will be accessing");
                Console.WriteLine("      the nightly build updates.  You may choose to update to them if you wish,");
                Console.WriteLine("      though you must realize that there WILL occasionally be bugs stuck into");
                Console.WriteLine("      the nightly, which might take a while [a day or two] to fix...  Since");
                Console.WriteLine("      you are working with a different directory than your normal RAF Manager");
                Console.WriteLine("      RC, the backup directories will be different to, so if you pack a file");
                Console.WriteLine("      with 1 build, you can't revert that pack with the beta.  ");
                Console.WriteLine("");
                Console.WriteLine("If you plan on using the 'add to archive' feature, you must go to options and");
                Console.WriteLine("check the big checkbox.  To restore you MUST do this too!");
                Console.WriteLine("I am working on a prettier file browser for others to use... Release TBA...");
                Console.WriteLine("");
                Console.WriteLine("Build Timestamp: " + RAFManagerUpdater.Versioning.CurrentVersion.ApproximateBuildTime);
            }
            #endregion
            /*
            RMPropInterpreter interpreter = new RMPropInterpreter(
                File.ReadAllLines(@"C:\LoLModProjects\colorblindnomore_u3_v3\rafmanagerproperties")
            );
            Console.WriteLine("-------");
            interpreter.RunPacktimeCommand();
            Console.WriteLine("-------");
            interpreter.RunRestoreTimeCommand();
            return;
             */
            #region check for duplicate instances
            Process[] dupes = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            if (dupes.Length != 1)
            {
                //This doesn't always work.
                if (dupes[0] != Process.GetCurrentProcess())
                {
                    Console.WriteLine("This application is already open.  Setting focus to other application's main window");
                    Console.WriteLine("MWT: " + dupes[0].MainWindowTitle);
                    Rectangle rect = ItzWarty.WinAPI.GetWindowRect(dupes[0].MainWindowHandle);
                    WinAPI.MoveWindow(dupes[0].MainWindowHandle,
                        (Screen.PrimaryScreen.Bounds.Width - rect.Width) / 2,
                        (Screen.PrimaryScreen.Bounds.Height - rect.Height) / 2,
                        rect.Width, rect.Height, true
                    );
                    WinAPI.ShowWindow(dupes[0].MainWindowHandle, ItzWarty.WinAPI.ShowWindowParam.SW_SHOWNORMAL);
                    WinAPI.SetForegroundWindow(dupes[0].MainWindowHandle);
                    WinAPI.FLASHWINFO fwInfo = new WinAPI.FLASHWINFO();
                    fwInfo.cbSize = (UInt32)Marshal.SizeOf(fwInfo);
                    fwInfo.hwnd = dupes[0].MainWindowHandle;
                    fwInfo.dwFlags = WinAPI.FLASHW_TRAY | WinAPI.FLASHW_CAPTION;
                    fwInfo.uCount = 2;
                    fwInfo.dwTimeout = 0;
                    WinAPI.FlashWindowEx(ref fwInfo);
                    
                    //ItzWarty.WinAPI.SetFocus(dupes[0].MainWindowHandle);
                    Application.Exit();
                    Environment.Exit(0);
                }
            }
            #endregion

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Application.Run(new RAFManagerCleanWizard());
            new RAFManagerClass(new MainForm());
        }
    }
}
