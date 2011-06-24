using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Dokan;


using RAFLib;

using RM = RAFManager.RAFManagerClass;
using RAFManager;

using System.Windows.Forms;

using System.IO;

using ItzWarty;

namespace RAFMount
{
    //Installer: Run DokanInstall_0.6.0.exe /S for silent mode install
    class RAFArchiveOperations : DokanOperations
    {
        private int count = 1;
        private RM rafManager = null;
        public RAFArchiveOperations(RM rafManager)
        {
            this.rafManager = rafManager;
        }

        //Windows System Error Codes @ http://www.hiteksoftware.com/knowledge/articles/049.htm
        public int CreateFile(string filename, System.IO.FileAccess access, System.IO.FileShare share, System.IO.FileMode mode, System.IO.FileOptions options, DokanFileInfo info)
        {
            Console.WriteLine("crFile: " + filename);

            info.Context = count++;

            if (filename.Trim() == "\\")
            {
                info.IsDirectory = true;
                return 0;
            }
            else
            {
                RAFInMemoryFileSystemObject fso = rafManager.ResolveRAFPathTOFSO(filename);
                Console.WriteLine("-> FSO NULL? " + (fso == null));
                if (fso == null) return -DokanNet.ERROR_ACCESS_DENIED;
                else
                {
                    if (fso.GetFSOType() == RAFFSOType.DIRECTORY)
                        info.IsDirectory = true;
                    return 0;
                }
            }
            //return -82;
        }

        public int OpenDirectory(string filename, DokanFileInfo info)
        {
            Console.WriteLine("openDir: " + filename);
            info.Context = count++;
            info.IsDirectory = true;
            return 0;
        }

        public int CreateDirectory(string filename, DokanFileInfo info)
        {
            Console.WriteLine("mkdir: " + filename);
            return -1;
            throw new NotImplementedException();
        }

        public int Cleanup(string filename, DokanFileInfo info)
        {
            //Console.WriteLine("Cleanup: "+filename);
            return 0;
            throw new NotImplementedException();
        }

        public int CloseFile(string filename, DokanFileInfo info)
        {
            //Console.WriteLine("CF: " + filename);
            return 0;
            throw new NotImplementedException();
        }

        public int ReadFile(string filename, byte[] buffer, ref uint readBytes, long offset, DokanFileInfo info)
        {
            Console.WriteLine("RF: " + filename + ", off" + offset + ", len" + buffer.Length);

            RAFInMemoryFileSystemObject fso = rafManager.ResolveRAFPathTOFSO(filename);
            Console.WriteLine("->FSO NULL: " + (fso == null));
            if (fso == null) return -DokanNet.ERROR_FILE_NOT_FOUND;
            else
            {
                RAFFileListEntry entry = rafManager.ResolveRAFPathToEntry(fso.GetRAFPath(true));
                byte[] content = entry.GetContent();

                readBytes = Math.Min((uint)(content.Length - offset), (uint)buffer.Length);

                Array.Copy(content, offset, buffer, 0, readBytes);
                Console.WriteLine("-> " + readBytes + " bytes read");
                return 0;
            }
            //throw new NotImplementedException();
        }

        public int WriteFile(string filename, byte[] buffer, ref uint writtenBytes, long offset, DokanFileInfo info)
        {
            return -DokanNet.ERROR_ACCESS_DENIED;
            throw new NotImplementedException();
        }

        public int FlushFileBuffers(string filename, DokanFileInfo info)
        {
            return -1;
            throw new NotImplementedException();
        }

        public int GetFileInformation(string filename, FileInformation fileinfo, DokanFileInfo info)
        {
            Console.WriteLine("GFI: "+filename);
            if (filename.Trim() == "\\")
            {
                Console.WriteLine("->Root");
                fileinfo.FileName = filename;
                fileinfo.Attributes = System.IO.FileAttributes.Directory | FileAttributes.ReadOnly | FileAttributes.NotContentIndexed;
                fileinfo.CreationTime = DateTime.Now;
                fileinfo.LastAccessTime = DateTime.Now;
                fileinfo.LastWriteTime = DateTime.Now;
                fileinfo.Length = 0;

                info.IsDirectory = true;
            }
            else
            {
                RAFInMemoryFileSystemObject fso = rafManager.ResolveRAFPathTOFSO(filename);
                if (fso == null) return -DokanNet.ERROR_FILE_NOT_FOUND;

                fileinfo.FileName = filename;
                if (fso.GetFSOType() == RAFFSOType.FILE)
                {
                    fileinfo.Attributes = System.IO.FileAttributes.Normal;
                    fileinfo.Length = rafManager.ResolveRAFPathToEntry(fso.GetRAFPath(true)).GetContent().Length;
                }
                else
                {
                    fileinfo.Attributes = System.IO.FileAttributes.Directory;
                    fileinfo.Length = 0;

                    info.IsDirectory = true;
                }
                fileinfo.CreationTime = DateTime.Now;
                fileinfo.LastAccessTime = DateTime.Now;
                fileinfo.LastWriteTime = DateTime.Now;
            }
            return 0;
        }
        public int FindFiles(string filename, System.Collections.ArrayList files, DokanFileInfo info)
        {
            Console.WriteLine("FF: " + filename);
            switch (filename)
            {
                case "\\":
                {
                    Console.WriteLine("! " + rafManager.ArchiveFSOs.Count);
                    for (int i = 0; i < rafManager.ArchiveFSOs.Count; i++)
                    {
                        files.Add(
                            new FileInformation()
                            {
                                FileName = rafManager.ArchiveFSOs[i].Text,
                                Attributes = System.IO.FileAttributes.Directory,
                                CreationTime = DateTime.Now,
                                LastAccessTime = DateTime.Now,
                                LastWriteTime = DateTime.Now,
                                Length = 0
                            }
                        );
                    }
                    break;
                }
                default:
                {
                    RAFInMemoryFileSystemObject fso = rafManager.ResolveRAFPathTOFSO(filename); //Get rid of the first \ in path
                    Console.WriteLine("FSO IS NULL?: "+(fso == null));
                    if (fso == null) return -DokanNet.ERROR_FILE_NOT_FOUND;
                    else
                    {
                        for (int i = 0; i < fso.Nodes.Count; i++)
                        {
                            RAFInMemoryFileSystemObject cFSO = (RAFInMemoryFileSystemObject)fso.Nodes[i];
                            FileAttributes attributes = FileAttributes.Normal;
                            long length = 0;
                            if (cFSO.GetFSOType() == RAFFSOType.DIRECTORY)
                                attributes = FileAttributes.Directory;
                            else if (cFSO.GetFSOType() == RAFFSOType.FILE)
                            {
                                attributes = FileAttributes.Normal;
                                length = rafManager.ResolveRAFPathToEntry(cFSO.GetRAFPath(true)).FileSize;
                            }
                            
                            files.Add(
                                new FileInformation()
                                {
                                    FileName = cFSO.Text,
                                    Attributes = attributes,
                                    CreationTime = DateTime.Now,
                                    LastAccessTime = DateTime.Now,
                                    LastWriteTime = DateTime.Now,
                                    Length = length
                                }
                            );
                        }
                    }
                    break;
                }
            }
            return 0;
            //throw new NotImplementedException();
        }

        public int SetFileAttributes(string filename, System.IO.FileAttributes attr, DokanFileInfo info)
        {
            Console.WriteLine("SFA " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int SetFileTime(string filename, DateTime ctime, DateTime atime, DateTime mtime, DokanFileInfo info)
        {
            Console.WriteLine("SFT " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int DeleteFile(string filename, DokanFileInfo info)
        {
            Console.WriteLine("DF " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int DeleteDirectory(string filename, DokanFileInfo info)
        {
            Console.WriteLine("DDIR " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int MoveFile(string filename, string newname, bool replace, DokanFileInfo info)
        {
            Console.WriteLine("moveFile " + filename + " " +newname);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int SetEndOfFile(string filename, long length, DokanFileInfo info)
        {
            Console.WriteLine("SetEOF " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int SetAllocationSize(string filename, long length, DokanFileInfo info)
        {
            Console.WriteLine("SetAllocSize " + filename);
            return -DokanNet.ERROR_ACCESS_DENIED;
        }

        public int LockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Console.WriteLine("Lock " + filename);
            return 0;
        }

        public int UnlockFile(string filename, long offset, long length, DokanFileInfo info)
        {
            Console.WriteLine("Unlock " + filename);
            return 0;
        }

        public int GetDiskFreeSpace(ref ulong freeBytesAvailable, ref ulong totalBytes, ref ulong totalFreeBytes, DokanFileInfo info)
        {
            Console.WriteLine("GFDS");
            return -1;
            freeBytesAvailable = 133713371337;
            totalFreeBytes = 133713371337;
            totalBytes = 13371337133713;
            return 0;
            throw new NotImplementedException();
        }

        public int Unmount(DokanFileInfo info)
        {
            return -1;
            throw new NotImplementedException();
        }
    }
    public class RAFMount
    {
        public static void Main(string[] args)
        {
            RM rm = new RM(new NullGUI());
            new RAFMount(rm);
        }
        public RAFMount(RM rafManager)
        {
            //Console.SetOut(TextWriter.Null);
            DokanOptions options = new DokanOptions();
            options.DebugMode = true;
            options.MountPoint = "r:\\";
            options.ThreadCount = 1;

            int status = DokanNet.DokanMain(options, new RAFArchiveOperations(rafManager));
            switch (status)
            {
                case DokanNet.DOKAN_DRIVE_LETTER_ERROR:
                    Console.WriteLine("Drvie letter error");
                    break;
                case DokanNet.DOKAN_DRIVER_INSTALL_ERROR:
                    Console.WriteLine("Driver install error");
                    break;
                case DokanNet.DOKAN_MOUNT_ERROR:
                    Console.WriteLine("Mount error");
                    break;
                case DokanNet.DOKAN_START_ERROR:
                    Console.WriteLine("Start error");
                    break;
                case DokanNet.DOKAN_ERROR:
                    Console.WriteLine("Unknown error");
                    break;
                case DokanNet.DOKAN_SUCCESS:
                    Console.WriteLine("Success");
                    break;
                default:
                    Console.WriteLine("Unknown status: %d", status);
                    break;

            }
        }
    }
}
