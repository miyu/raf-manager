#define PACK
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using ItzWarty;

using System.IO;
using System.IO.Compression;

using zlib = ComponentAce.Compression.Libs.zlib;

namespace RAFLib
{
    static unsafe class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if PACK
            return;
#endif
            Console.WriteLine("Experimental RAF Dumper by ItzWarty @ ItzWarty.com April 28 2011 10:00pm pst");
            Console.WriteLine("RAF dogcumentation @ bit.ly/mSyYrR ");
            Console.WriteLine("USAGE: RAF_DUMP (Or just double click executable)");
            Console.WriteLine("Precondition: RAF_DUMP is sitting next to a Riot Archive File.");
            Console.WriteLine("Postcondition: 'dump' folder created.  RAF extracted into folder.");
            //string dumpDir = @"C:\Riot Games\League of Legends\RADS\projects\lol_game_client\filearchives\0.0.0.28\dump\";

            //We write the file containing "name hash" next to our program.
            string hashesFile = Environment.CurrentDirectory + "\\hashes.txt";
            string hashesLog = "";

            //We write to this file saying what we don't compress
            string notcompressedsFile = Environment.CurrentDirectory + "\\nocompress.txt";
            string notcompressedsLog = "";

            string dumpDir = Environment.CurrentDirectory+"\\dump\\";
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
                Console.WriteLine("Couldn't find RAF file!!");
                Console.WriteLine("This executable must be in the same folder as a *.raf and *.raf.dat file!");
                Console.WriteLine("Exiting... Press a key (or two) to exit");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Found RAF: " + rafName);
            RAFArchive raf = new RAFArchive(rafName);
            FileStream fStream = raf.GetDataFileContentStream();
            //DeflateStream dfStream = new DeflateStream(fStream, CompressionMode.Decompress);

            int loggingOffset = 10;
            List<RAFFileListEntry> files = raf.GetDirectoryFile().GetFileList().GetFileEntries();
            foreach (RAFFileListEntry entry in files)
            {
                Console.SetCursorPosition(0, 7);
                Console.WriteLine("Dumping: " + entry.FileName.Split("\\").Last()+" ".Repeat(40));
                Console.SetCursorPosition(0, 8);
                Console.WriteLine("  Data File Offset: $" + entry.FileOffset.ToString("x") + "; Size:" + entry.FileSize + " ($" + entry.FileSize.ToString("x") + ")" + " ".Repeat(40));
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
                    Console.SetCursorPosition(2, loggingOffset++);
                    Console.Write("UNABLE TO DECOMPRESS FILE, WRITING DEFLATED FILE:");
                    Console.SetCursorPosition(4, loggingOffset++);
                    Console.Write(entry.FileName);
                    File.WriteAllBytes((dumpDir + entry.FileName).Replace("/", "\\"), buffer.ToArray());
                }
            }
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Write hashes and nocompress file");
            File.WriteAllText(hashesFile, hashesLog);
            File.WriteAllText(notcompressedsFile, notcompressedsLog);
            Console.WriteLine("DONE!" + " ".Repeat(30));
            return;
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
        }
        /// <summary>
        /// ... no idea what this really does, is a checksum
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static UInt32 Adler32(string s)
        {
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(s);
            return Adler32(bytes, bytes.Length);
        }
        /// <summary>
        /// http://en.wikipedia.org/wiki/Adler-32
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private static UInt32 Adler32(byte[] data, int len)
        {
            UInt32 a = 1, b = 0;
            int index;
 
            /* Process each byte of the data in order */
            for (index = 0; index < len; ++index)
            {
                a = (a + data[index]) % 65521;
                b = (b + a) % 65521;
            }
 
            return (b << 16) | a;
        }
        private static void PrepareDirectory(string path)
        {
            path = path.Replace("/", "\\");
            String[] dirs = path.Split("\\");
            for (int i = 1; i < dirs.Length; i++)
            {
                String dirPath = String.Join("\\", dirs.SubArray(0, i))+"\\";
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
                //Console.WriteLine(dirPath);
            }
        }
    }
}
