using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

using ItzWarty;

namespace RAFLib
{
    public static class RAFHashManager
    {
        private static Dictionary<string, UInt32> hashes = null;

        public static void Init()
        {
            string[] hashFileContentLines = File.ReadAllText(Environment.CurrentDirectory + "\\hashes.txt").Split("\n");
            foreach (string s in hashFileContentLines)
            {
                if (s != "")
                {
                    string[] parts = s.Split("|||");
                    hashes.Add(parts[0].ToLower(), UInt32.Parse(parts[1]));
                }
            }
        }
        public static List<string> GetKeys()
        {
            return new List<string>(hashes.Keys);
        }
        public static UInt32 GetHash(string s)
        {
            return hashes[s];
        }
    }
}
