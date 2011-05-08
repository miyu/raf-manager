using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFManagerUpdater.Versioning
{
    /// <summary>
    /// Change these constants before loading to server
    /// </summary>
    public static class CurrentVersion
    {
        //Represents Major Great Leap Forwards.  Not the Mao Zedong type.
        public const int major = 1;

        //Represents addons that are nice but not huge, or big bugfixes
        public const int minor = 2;

        //Represents addons that are minor, such as changing text or fixing a small bug
        public const int revision = 0;

        //Represents Flags [dev, b], currently not used
        public const string flags = "";

        /// <summary>
        /// Yes, this is a duplicate of ItzWarty.ApplicationInformation.BuildTime, but it is not dependent
        /// on any other external library.
        /// http://stackoverflow.com/questions/1600962/c-displaying-the-build-date
        /// </summary>
        public static DateTime ApproximateBuildTime
        {
            get
            {
                string filePath = System.Reflection.Assembly.GetCallingAssembly().Location;
                const int c_PeHeaderOffset = 60;
                const int c_LinkerTimestampOffset = 8;
                byte[] b = new byte[2048];
                System.IO.Stream s = null;

                try
                {
                    s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    s.Read(b, 0, 2048);
                }
                finally
                {
                    if (s != null)
                    {
                        s.Close();
                    }
                }

                int i = System.BitConverter.ToInt32(b, c_PeHeaderOffset);
                int secondsSince1970 = System.BitConverter.ToInt32(b, i + c_LinkerTimestampOffset);
                DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0);
                dt = dt.AddSeconds(secondsSince1970);
                dt = dt.AddHours(TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
                return dt;
            }
        }
        public static string GetVersionString()
        {
            return major + "." + minor + "." + revision + flags;
        }
    }
}
