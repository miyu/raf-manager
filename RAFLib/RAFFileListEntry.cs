using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using zlib = ComponentAce.Compression.Libs.zlib;

namespace RAFLib
{
    public class RAFFileListEntry
    {
        private byte[] directoryFileContent = null;
        private UInt32 offsetEntry = 0;
        private RAFArchive raf = null;
        public RAFFileListEntry(RAFArchive raf, byte[] directoryFileContent, UInt32 offsetEntry)
        {
            this.raf = raf;
            this.directoryFileContent = directoryFileContent;
            this.offsetEntry = offsetEntry;
        }
        /// <summary>
        /// Hash of the string name
        /// </summary>
        public UInt32 StringNameHash
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry);
            }
        }
        /// <summary>
        /// Offset to the start of the archived file in the data file
        /// </summary>
        public UInt32 FileOffset
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+4);
            }
            set
            {
                byte[] valueBytes = BitConverter.GetBytes(value);
                Array.Copy(valueBytes, 0, directoryFileContent, offsetEntry + 4, 4);
            }
        }
        /// <summary>
        /// Size of this archived file
        /// </summary>
        public UInt32 FileSize
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+8);
            }
        }
        /// <summary>
        /// Index of the name of the archvied file in the string table
        /// </summary>
        public UInt32 FileNameStringTableIndex
        {
            get
            {
                return BitConverter.ToUInt32(directoryFileContent, (int)offsetEntry+12);
            }
        }

        public String FileName
        {
            get
            {
                return this.raf.GetDirectoryFile().GetStringTable()[this.FileNameStringTableIndex];
            }
        }

        public byte[] GetContent()
        {
            FileStream fStream = this.raf.GetDataFileContentStream();

            byte[] buffer = new byte[this.FileSize];            //Will contain compressed data
            fStream.Seek(this.FileOffset, SeekOrigin.Begin);
            fStream.Read(buffer, 0, (int)this.FileSize);

            try
            {
                MemoryStream mStream = new MemoryStream(buffer);
                zlib.ZInputStream zinput = new zlib.ZInputStream(mStream);

                List<byte> dBuffer = new List<byte>(); //decompressed buffer, arraylist to my knowledge...

                //This could be optimized in the future by reading a block and adding it to our arraylist..
                //which would be much faster, obviously
                int data = 0;
                while ((data = zinput.Read()) != -1)
                    dBuffer.Add((byte)data);
                //ComponentAce.Compression.Libs.zlib.ZStream a = new ComponentAce.Compression.Libs.zlib.ZStream();

                //File.WriteAllBytes((dumpDir + entry.FileName).Replace("/", "\\"), dBuffer.ToArray());
                return dBuffer.ToArray();
            }
            catch
            {
                //it's not compressed, just return original content
                return buffer;
            }
        }
        public RAFArchive RAFArchive
        {
            get
            {
                return raf;
            }
        }
        public override string ToString()
        {
            return FileName;
        }
    }
}
