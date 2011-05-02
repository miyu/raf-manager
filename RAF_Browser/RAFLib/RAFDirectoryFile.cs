using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RAFLib.Util;

using System.IO;

namespace RAFLib
{
    public unsafe class RAFDirectoryFile
    {
        /// <summary>
        /// Magic value used to identify the file type, must be 0x18BE0EF0
        /// </summary>
        UInt32 magic = 0;

        /// <summary>
        /// // Version of the archive format, must be 1
        /// </summary>
        UInt32 version = 0;

        /// <summary>
        /// An index that is used by the runtime, do not modify
        /// Have no idea what this really does, at the moment...
        /// </summary>
        UInt32 mgrIndex = 0;

        /// <summary>
        /// Offset to the table of contents from the start of the file
        /// </summary>
        UInt32 offsetFileList = 0;

        /// <summary>
        /// Offset to the string table from the start of the file
        /// </summary>
        UInt32 offsetStringTable = 0;

        byte[] content = null;
        RAFFileList fileList = null;
        RAFStringTable stringTable = null;
        public RAFDirectoryFile(RAFArchive raf, string location)
        {
            content = System.IO.File.ReadAllBytes(location);
            magic = BitConverter.ToUInt32(content.SubArray(0, 4), 0);
            version = BitConverter.ToUInt32(content.SubArray(4, 4), 0);
            mgrIndex = BitConverter.ToUInt32(content.SubArray(8, 4), 0);
            offsetFileList = BitConverter.ToUInt32(content.SubArray(12, 4), 0);
            offsetStringTable = BitConverter.ToUInt32(content.SubArray(16, 4), 0);

            //UINT32 is casted to INT32.  This should be fine, since i doubt that the RAF will become
            //a size of 2^31-1 in bytes.

            fileList = new RAFFileList(raf, content, offsetFileList);
            
            //Now we load our string table
            stringTable = new RAFStringTable(raf, content, offsetStringTable);
        }
        public RAFFileList GetFileList()
        {
            return this.fileList;
        }
        public RAFStringTable GetStringTable()
        {
            return this.stringTable;
        }
        /*
        private void PrintFileList()
        {
            Console.WriteLine("Begin PrintFileList.  FileListOffset:" + offsetFileList.ToString("x"));
            Console.WriteLine("File Count: " + fileListCount);
            UInt32 lastOffset = offsetFileList + 4;
            for(int i = 0; i < fileListCount; i++)
            {
                UInt32 newOffset = (UInt32)Array.IndexOf(content, (Byte)0x00, (int)lastOffset);
                byte[] fileNameBytes = content.SubArray((int)lastOffset, (int)(newOffset - lastOffset));
                Console.WriteLine("Name Length: " + fileNameBytes.Length);
                Console.WriteLine(
                    Encoding.ASCII.GetString(
                        fileNameBytes
                    )
                );
                lastOffset = newOffset;
            }
        }
         */
    }
}
