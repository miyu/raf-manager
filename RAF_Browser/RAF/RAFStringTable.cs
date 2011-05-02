using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAF
{
    public class RAFStringTable
    {
        /// <summary>
        /// size of all data including header
        /// </summary>
        private UInt32 sizeOfData = 0;

        /// <summary>
        /// Number of strings in the table
        /// </summary>
        private UInt32 stringCount = 0;

        private RAF raf = null; 
        private byte[] directoryFileContent = null;
        private UInt32 offsetTable = 0;
        public RAFStringTable(RAF raf, byte[] directoryFileContent, UInt32 offsetTable)
        {
            this.raf = raf;
            this.directoryFileContent = directoryFileContent;
            this.offsetTable = offsetTable;

            //Console.WriteLine("String Table Offset: " + offsetTable.ToString("x"));
            this.sizeOfData = BitConverter.ToUInt32(directoryFileContent, (int)offsetTable);
            this.stringCount = BitConverter.ToUInt32(directoryFileContent, (int)offsetTable+4);
        }
        public String this[UInt32 index]
        {
            get
            {
                UInt32 entryOffset = offsetTable + 8 + index*8; //+8 because of table header { size, count }
                //Above value points to the actual entry

                UInt32 entryValueOffset = BitConverter.ToUInt32(directoryFileContent, (int)entryOffset);
                UInt32 entryValueSize   = BitConverter.ToUInt32(directoryFileContent, (int)entryOffset+4);

                //-1 seems necessary.  I'd assume some null padding ends strings...
                byte[] stringBytes = directoryFileContent.SubArray((int)(entryValueOffset + offsetTable), (int)entryValueSize-1);
                return Encoding.ASCII.GetString(stringBytes);
            }
        }
    }
}
