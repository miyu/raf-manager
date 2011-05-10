using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using System.Diagnostics;
using System.Drawing;

using ItzWarty;

using System.Runtime.InteropServices;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
