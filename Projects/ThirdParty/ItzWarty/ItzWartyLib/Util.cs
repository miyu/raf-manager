using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ItzWarty
{
    public static class Util
    {
        /// <summary>
        /// Gets all files in the given directory, and its subdirectories.
        /// This is used because apparently Directory.GetFiles("path", "*", SubDirectories) doesn't work with space
        /// </summary>
        /// <returns></returns>
        public static string[] GetAllChildFiles(string path)
        {
            List<string> result = new List<string>();
            result.AddRange(Directory.GetFiles(path));

            string[] childDirs = Directory.GetDirectories(path);
            foreach (string dir in childDirs)
                if (!File.GetAttributes(dir).HasFlag(FileAttributes.ReparsePoint))
                    result.AddRange(GetAllChildFiles(dir));

            return result.ToArray();
        }
    }
}
