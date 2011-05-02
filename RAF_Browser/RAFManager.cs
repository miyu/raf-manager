using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ItzWarty;

using System.Windows.Forms;

namespace RAFLib
{
    public class RAFManager
    {
        public static bool Pack(string directory, StreamWriter ostream)
        {
            //Environment.CurrentDirectory = directory;
            //Environment.CurrentDirectory = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\0.0.0.29\";
            ostream.WriteLine("Experimental RAF Packer by ItzWarty @ ItzWarty.com April 29 2011 3:37pm pst");
            ostream.WriteLine("Only use-able after RAF Dumper, for now.");
            //Pack whatever dump folder is sitting next to us, write to pack folder
            if (Directory.Exists(Environment.CurrentDirectory + @"\dump\"))
            {
                RAFPacker packer = new RAFPacker(ostream);
                return packer.PackRAF(
                    Environment.CurrentDirectory + @"\dump\",
                    Environment.CurrentDirectory + @"\pack\"
                );
            }
            else
            {
                ostream.WriteLine("Error"); //Very helpful error message
                return false;
            }
            //else
            //    MessageBox.Show("Place RAFDUMP executable next to 'dump' folder.  Upon run, the 'pack' folder will be created w/ raf+raf.dat");
        }
    }
}
