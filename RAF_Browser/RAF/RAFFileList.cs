using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;

using System.IO;

namespace RAF
{
    public class RAFFileList
    {
        /// <summary>
        /// Number of entries in the file list
        /// </summary>
        private UInt32 fileListCount = 0;
        private byte[] content = null;
        private UInt32 offsetFileListHeader = 0;
        private List<RAFFileListEntry> fileEntries = null;
        public RAFFileList(RAF raf, byte[] directoryFileContent, UInt32 offsetFileListHeader)
        {
            this.content = directoryFileContent;
            this.offsetFileListHeader = offsetFileListHeader;

            //The file list starts with a uint stating how many files we have
            fileListCount = BitConverter.ToUInt32(content.SubArray((Int32)offsetFileListHeader, 4), 0);

            //After the file list count, we have the actual data.
            UInt32 offsetEntriesStart = offsetFileListHeader + 4;
            this.fileEntries = new List<RAFFileListEntry>();
            for (UInt32 currentOffset = offsetEntriesStart;
                currentOffset < offsetEntriesStart + 16 * fileListCount; currentOffset += 16)
            {
                this.fileEntries.Add(new RAFFileListEntry(raf, directoryFileContent, currentOffset));
            }
        }
        public List<RAFFileListEntry> GetFileEntries()
        {
            return this.fileEntries;
        }
    }
}
