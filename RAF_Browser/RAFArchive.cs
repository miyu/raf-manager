using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace RAFLib
{
    /// <summary>
    /// Will be able to unpack/repack the Riot Archive Format, documented at http://www.leagueoflegends.com/board/showthread.php?t=701772
    /// 
    /// For now, it's only able to unpack
    /// </summary>
    public class RAFArchive
    {
        private FileStream dataFileStream = null;
        private RAFDirectoryFile directoryFile = null;

        public RAFArchive(string rafPath)
        {
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
    }
}
