using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//Cannot have external non-.net dependencies
using System.Net;
using System.Threading;

using System.IO;
using System.Windows.Forms;

using RAFManagerUpdater.Versioning;
namespace RAFManagerUpdater
{
    public enum UpdateResult
    {
        UnableToConnect,
        NoUpdates,
        NewUpdate
    }
    public class Autoupdater
    {
        private static Func<UpdateResult, string, string, object> callback = null;

        public static void CheckUpdate(Func<UpdateResult, string, string, object> callback)
        {
            Autoupdater.callback = callback;
            new Thread(delegate() { CheckUpdatesThread(); }).Start();
        }
        private static UpdateResult CheckUpdatesThread()
        {
            string cver = CurrentVersion.major + "." + CurrentVersion.minor + "." + CurrentVersion.revision;
            Console.WriteLine("Check Updates.  Current Version: " + CurrentVersion.GetVersionString());
            System.Net.WebClient client = new WebClient();
            try
            {
                string response = client.DownloadString(updateDomain+"version.txt");
                Console.WriteLine("Got most recent version of flag '"+CurrentVersion.flags+"' from server: " + response);

                if (response.Trim() != cver.Trim()+CurrentVersion.flags)
                {
                    Console.WriteLine("New update available");
                    if(callback != null)
                        callback(UpdateResult.NewUpdate, client.DownloadString(updateDomain+"updatenotes.txt"), response.Trim());
                    return UpdateResult.NewUpdate;
                }
                else
                {
                    Console.WriteLine("No update available");
                    if (callback != null)
                        callback(UpdateResult.NoUpdates, "", "");
                    return UpdateResult.NoUpdates;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Update Check failed");
                Console.WriteLine(e.ToString());
                if (callback != null)
                    callback(UpdateResult.UnableToConnect, "", "");
                return UpdateResult.UnableToConnect;
            }
        }
        public const string updateDomain = "http://www.ItzWarty.com/raf/releases/latest/"; //location of updatenotes.txt and version.txt
        public const string latestURI = "http://www.ItzWarty.com/raf/releases/latest/"; //include ending folder name slash
        public static void Main(string[] args)
        {
            //Check arguments to see if we've been spawned to only do one thing
            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "-phase1": //We should delete/replace update.exe and copy nupdate.exe to it [we are nupdate]
                    {
                        Console.WriteLine("phase1");
                        System.Threading.Thread.Sleep(1000);
                        File.WriteAllBytes("update.exe", File.ReadAllBytes("nupdate.exe"));
                        AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e)
                        {
                            System.Diagnostics.Process.Start("update.exe", "-phase2");
                        };
                        Application.Exit();
                        return;
                    }
                    case "-phase2": //we are update.exe, and we are now deleting nupdate.exe, then running rafmanager
                    {
                        Console.WriteLine("phase2");
                        AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e)
                        {
                            File.Delete("nupdate.exe");
                            System.Diagnostics.Process.Start("rafmanager.exe", "-phase2");
                        };
                        Application.Exit();
                        return;
                    }
                }
            }
            Console.WriteLine("ItzWarty's RAFManager Autoupdater");
            Console.WriteLine("Current Version: " + CurrentVersion.major + "." + CurrentVersion.minor + "." + CurrentVersion.flags+
                                "; build time: "+CurrentVersion.ApproximateBuildTime);

            switch (CheckUpdatesThread())
            {
                case UpdateResult.NewUpdate:
                    WebClient wc = new WebClient();
                    string[] files = wc.DownloadString(latestURI + "filelist.txt").Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    Console.WriteLine("Files to download: ");
                    foreach(string i in files)
                        Console.WriteLine("  "+i);
                    //Note that the new update.exe will be saved as nupdate.exe
                    //the running update.exe will run nupdate.exe -phase1 when exiting
                    //nupdate.exe will delete update.exe and copy itself to update.exe
                    //
                    //nupdate.exe will then run update.exe -phase2 to delete nupdate.exe
                    //
                    //After the -delete command, update.exe will run RAFManager.exe again
                    foreach (string fName in files)
                    {
                        string fileName = fName.Trim();

                        Console.WriteLine("Write: " + fileName);
                        string fullURI = latestURI + fileName;
                        byte[] fileContent = wc.DownloadData(fullURI);
                        if (fileName.ToLower() == "update.exe") fileName = "nupdate.exe";
                        File.WriteAllBytes(fileName, fileContent);
                    }

                    if (File.Exists("nupdate.exe"))
                    {
                        AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e)
                        {
                            System.Diagnostics.Process.Start("nupdate.exe", "-phase1");
                        };
                        Application.Exit();
                    }
                    else
                    {
                        AppDomain.CurrentDomain.ProcessExit += delegate(object sender, EventArgs e)
                        {
                            System.Diagnostics.Process.Start("rafmanager.exe");
                        };
                        Application.Exit();
                    }
                    break;
                case UpdateResult.NoUpdates:
                    Console.WriteLine("No updates available!");
                    break;
                case UpdateResult.UnableToConnect:
                    Console.WriteLine("Unable to connect to update server!");
                    Console.WriteLine("Please check www.ItzWarty.com/raf/ for updates");
                    Console.WriteLine("If that fails, check the LeagueCraft forums");
                    break;
            }
        }
    }
}
