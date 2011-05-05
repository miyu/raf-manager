using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ItzWarty;

using zlib = ComponentAce.Compression.Libs.zlib;

namespace RAFLib
{
    public class RAFArchive
    {
        private FileStream dataFileStream = null;
        private RAFDirectoryFile directoryFile = null;
        private string rafPath = "";
        public RAFArchive(string rafPath)
        {
            this.rafPath = rafPath;
            dataFileStream = new FileStream(rafPath+".dat", FileMode.Open);
            this.directoryFile  = new RAFDirectoryFile(this, rafPath);
        }

        /// <summary>
        /// Gets a FileStream of our RAF data file
        /// </summary>
        /// <returns></returns>
        public FileStream GetDataFileContentStream()
        {
            return this.dataFileStream;
        }
        /// <summary>
        /// Gets the RAFDirectoryFile wrapper
        /// </summary>
        /// <returns></returns>
        public RAFDirectoryFile GetDirectoryFile() { return this.directoryFile; }


        /// <summary>
        /// Packs the /dump/ child directory of the given directory.
        /// Outputs to the given ostream.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="ostream"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Dumps the content of the first .raf/.raf.dat archive seen in the given directory.
        /// Outputs to the given ostream
        /// </summary>
        public static bool Dump(string directory, StreamWriter ostream)
        {
            #region output_usage_header
            ostream.WriteLine("Experimental RAF Dumper by ItzWarty @ ItzWarty.com April 28 2011 10:00pm pst");
            ostream.WriteLine("RAF dogcumentation @ bit.ly/mSyYrR ");
            ostream.WriteLine("USAGE: RAF_DUMP (Or just double click executable)");
            ostream.WriteLine("Precondition: RAF_DUMP is sitting next to a Riot Archive File.");
            ostream.WriteLine("Postcondition: 'dump' folder created.  RAF extracted into folder.");
            #endregion
            //string dumpDir = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\0.0.0.28\dump\";
            
            //We write the file containing "name hash" next to our program.
            string hashesFile = Environment.CurrentDirectory + "\\hashes.txt";
            string hashesLog = "";

            //We write to this file saying what we don't compress
            string notcompressedsFile = Environment.CurrentDirectory + "\\nocompress.txt";
            string notcompressedsLog = "";

            string dumpDir = Environment.CurrentDirectory + "\\dump\\";
            String[] filesInDir = Directory.GetFiles(Environment.CurrentDirectory);
            String rafName = "";
            foreach (String fileName in filesInDir)
            {
                if (fileName.Split("\\").Last().EndsWith(".raf", StringComparison.InvariantCultureIgnoreCase))
                {
                    rafName = fileName;
                }
            }
            if (rafName == "")
            {
                ostream.WriteLine("Couldn't find RAF file!!");
                //ostream.WriteLine("This executable must be in the same folder as a *.raf and *.raf.dat file!");
                //ostream.WriteLine("Exiting... Press a key (or two) to exit");
                //ostream.ReadKey();
                return false;
            }
            ostream.WriteLine("Found RAF: " + rafName);
            RAFArchive raf = new RAFArchive(rafName);
            FileStream fStream = raf.GetDataFileContentStream();
            //DeflateStream dfStream = new DeflateStream(fStream, CompressionMode.Decompress);

            //int loggingOffset = 10; //No longer applicable since we aren't logging to system.out console directly
            List<RAFFileListEntry> files = raf.GetDirectoryFile().GetFileList().GetFileEntries();
            foreach (RAFFileListEntry entry in files)
            {
                //ostream.SetCursorPosition(0, 7);
                ostream.WriteLine("Dumping: " + entry.FileName.Split("\\").Last() + " ".Repeat(40));
                //ostream.SetCursorPosition(0, 8);
                ostream.WriteLine("  Data File Offset: $" + entry.FileOffset.ToString("x") + "; Size:" + entry.FileSize + " ($" + entry.FileSize.ToString("x") + ")" + " ".Repeat(40));
                //Console.SetCursorPosition(0, 7);
                //Console.WriteLine("  Expected FileName Checksum: $" + entry.StringNameHash.ToString("x") + "; Calculated:" + Adler32(entry.GetFileName).ToString("x")/*entry.GetFileName.GetHashCode().ToString("x")*/ + " ".Repeat(40));
                //Console.WriteLine("  To: " + dumpDir + entry.GetFileName+";;");
                hashesLog += entry.FileName + "|||" + entry.StringNameHash + "\n";
                //zlib.ad
                byte[] buffer = new byte[entry.FileSize];
                fStream.Seek(entry.FileOffset, SeekOrigin.Begin);
                fStream.Read(buffer, 0, (int)entry.FileSize);

                PrepareDirectory((dumpDir + entry.FileName).Replace("/", "\\"));
                try
                {
                    MemoryStream mStream = new MemoryStream(buffer);
                    zlib.ZInputStream zinput = new zlib.ZInputStream(mStream);

                    List<byte> dBuffer = new List<byte>(); //decompressed buffer, arraylist to my knowledge...
                    int data = 0;
                    while ((data = zinput.Read()) != -1)
                        dBuffer.Add((byte)data);
                    //ComponentAce.Compression.Libs.zlib.ZStream a = new ComponentAce.Compression.Libs.zlib.ZStream();

                    File.WriteAllBytes((dumpDir + entry.FileName).Replace("/", "\\"), dBuffer.ToArray());
                }
                catch
                {
                    notcompressedsLog += entry.FileName.ToLower() + "\n";
                    //Console.SetCursorPosition(2, loggingOffset++);
                    ostream.Write("UNABLE TO DECOMPRESS FILE, WRITING DEFLATED FILE:");
                    //Console.SetCursorPosition(4, loggingOffset++);
                    ostream.Write("  " + entry.FileName);
                    File.WriteAllBytes((dumpDir + entry.FileName).Replace("/", "\\"), buffer.ToArray());
                }
            }
            //Console.SetCursorPosition(0, 0);

            ostream.WriteLine("Write hashes and nocompress file");
            File.WriteAllText(hashesFile, hashesLog);
            File.WriteAllText(notcompressedsFile, notcompressedsLog);
            ostream.WriteLine("DONE!" + " ".Repeat(30));
            return true;
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
        public bool InsertFile(string fileName, byte[] content)
        {
            RAFFileListEntry fileentry = this.GetDirectoryFile().GetFileList().GetFileEntry(fileName);
            return false; //TODO
        }
        private static void PrepareDirectory(string path)
        {
            path = path.Replace("/", "\\");
            String[] dirs = path.Split("\\");
            for (int i = 1; i < dirs.Length; i++)
            {
                String dirPath = String.Join("\\", dirs.SubArray(0, i)) + "\\";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                //Console.WriteLine(dirPath);
            }
        }
        /// <summary>
        /// Returns what i'm calling the ID of an archive, though it's probably related to LoL versioning.
        /// IE: 0.0.0.25, 0.0.0.26
        /// </summary>
        /// <returns></returns>
        public string GetID()
        {
            return new FileInfo(this.rafPath).Directory.Name;
        }
    }
}
