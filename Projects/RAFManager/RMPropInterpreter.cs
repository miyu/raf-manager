using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ItzWarty;
using RAFLib;

using System.Windows.Forms;
using System.IO;

namespace RAFManager
{
    public class RMPropInterpreter
    {
        public string Name { get; set; }
        public string Creator { get; set; }
        public string WebsiteURL { get; set; }

        public string GetVariableValue(string varName)
        {
            if(variables.ContainsKey(varName))
                return variables[varName];
            return "";
        }
        public bool HasPackTimeOverride
        {
            get
            {
                return GetVariableValue("$OVERRIDEPACKTIME").ToLower() == "true";
            }
        }
        public bool HasRestoreTimeOverride
        {
            get
            {
                return GetVariableValue("$OVERRIDERESTORETIME").ToLower() == "true";
            }
        }

        private int packtimeCodeStartIndex = -1;
        private int restoretimeCodeStartIndex = -1;

        private Dictionary<string, string> variables = new Dictionary<string, string>();
        public List<RMPropSetting> Settings { get; set; }

        private string scriptPath;
        public string ScriptPath { get { return scriptPath; } }
        private string scriptDir;
        private string[] lines;
        private MainForm mainForm;
        public RMPropInterpreter(string scriptPath, MainForm mainForm)
        {
            variables["$OVERRIDEPACKTIME"]      = "FALSE";
            variables["$OVERRIDERESTORETIME"]   = "FALSE";

            Settings = new List<RMPropSetting>();
            string[] prefLines = File.ReadAllLines(scriptPath);

            this.scriptPath = scriptPath;
            this.scriptDir = new FileInfo(scriptPath).DirectoryName;
            this.mainForm = mainForm;

            Name = prefLines[0];
            Creator = prefLines[1];
            WebsiteURL = prefLines[2];
            
            //we actually pad an extra line to our lines, so we can have a 1-indexed file lines array
            this.lines = new string[] { "" }.Concat(prefLines).ToArray();
            ProcessToEndStatement(lines, 4, true);
        }
        /// <param name="lines"></param>
        /// <param name="startLine"></param>
        /// <returns>Index of END</returns>
        private int ProcessToEndStatement(string[] lines, int startLine, bool isInitializerCall)
        {
            return ProcessToEndStatement(lines, startLine, isInitializerCall, true);
        }

        /// <summary>
        /// Todo: this needs to be modularized
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="startLine"></param>
        /// <param name="isInitializerCall"></param>
        /// <param name="multilineExec"></param>
        /// <returns></returns>
        private int ProcessToEndStatement(string[] lines, int startLine, bool isInitializerCall, bool multilineExec)
        {
            int i = startLine;
            for (; i < lines.Length && lines[i].Trim().ToUpper() != "END";)
            {
                Console.WriteLine("ProcessToEndStatement @ ln" + i);
                string line = lines[i].Trim();
                string[] lineParts = line.QASS(' ');

                if (lineParts.Length == 0)
                {
                    i++;
                }
                else
                {
                    switch (lineParts[0].ToUpper())
                    {
                        case "SET":
                            string varName = lineParts[1].Trim();
                            string varValue = lineParts[2];

                            Console.WriteLine("SETTING VARIABLE " + varName + " TO " + varValue);
                            if (!variables.ContainsKey(varName))
                                variables.Add(varName, varValue);
                            else
                                variables[varName] = varValue;
                            i++;
                            break;
                        case "OPTIONS":
                            i = ProcessOptionsBlock(lines, i) + 1;
                            break;
                        case "BACKUP_ONCE":
                        {
                            if(lineParts.Length < 2) 
                                throw new Exception("BACKUP_ONCE invalid syntax.  Expected: 'BACKUP_ONCE rafPath'.  ERROR @ ln " + i);

                            string rafFilePath = ResolveVariableToValue(lineParts[1].Trim());
                            Console.WriteLine("RAF FILE PATH: " + rafFilePath);
                            Console.WriteLine(variables.ContainsKey(rafFilePath));
                            string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + rafFilePath.Replace("/", "_");
                            if (File.Exists(fileBackupLoc))
                            {
                                Console.WriteLine("BACKUP_ONCE called when backup already created.  Ignoring command @ ln " + i);
                            }
                            else
                            {
                                RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(rafFilePath);
                                if (entry == null)
                                    throw new Exception("BACKUP_ONCE: Entry '" + rafFilePath + "' did NOT exist, or could NOT be found. @ ln " + i + "\r\n" +
                                                        "Correct Syntax: BACKUP_ONCE [full raf path including archive]\r\n" +
                                                        "Ex: BACKUP_ONCE 0.0.0.25/DATA/CFG/defaults/Game.cfg");
                                else
                                {
                                    Console.WriteLine("Backing up to " + fileBackupLoc);
                                    mainForm.PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                    File.WriteAllBytes(fileBackupLoc, entry.GetContent());
                                }
                            }
                            i++;
                            break;
                        }
                        case "INI_SET":
                        {
                            if (lineParts.Length < 4)
                                throw new Exception("INI_SET invalid syntax.  Expected: 'INI_SET RAFPath Section.Key value'.  ERROR @ ln " + i);
                            string rafPath          = ResolveVariableToValue(lineParts[1]);
                            string iniSectionKey    = ResolveVariableToValue(lineParts[2]);
                            string newValue         = ResolveVariableToValue(lineParts[3]);

                            string[] sectionKeyParts= iniSectionKey.Split(".");
                            if (sectionKeyParts.Length != 2)
                                throw new Exception("Section.Key should only have 1 period in it.  ERROR @ ln " + i);
                            string iniSection       = sectionKeyParts[0];
                            string iniKey           = sectionKeyParts[1];

                            RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(rafPath);
                            if (entry == null)
                                throw new Exception("BACKUP_ONCE: Entry '" + rafPath + "' did NOT exist, or could NOT be found. @ ln "+i);
                            else
                            {
                                Console.WriteLine("INI_SET: Section:{0}; Key:{1}; Value:{2};".F(iniSection, iniKey, newValue));
                                //extract it
                                string tempPath = Environment.CurrentDirectory+"/temp" + DateTime.Now.ToFileTime() + ".ini";
                                File.WriteAllBytes(tempPath, entry.GetContent());
                                IniFile iniFile = new IniFile(tempPath);
                                iniFile.IniWriteValue(iniSection, iniKey, newValue);

                                //write it again
                                entry.RAFArchive.InsertFile(
                                    entry.FileName, File.ReadAllBytes(tempPath), null
                                );

                                //delete temp file
                                File.Delete(tempPath);
                            }
                            i++;
                            break;
                        }
                        case "PACKTIME":
                            if (isInitializerCall)
                            {
                                //We're done reading, since we're just running the initializer
                                //Store the PC of the packtime declaration
                                Console.WriteLine("Encountered PACKTIME command.  In Initializer call, skipping.");
                                packtimeCodeStartIndex = i;
                                i = FindNextEndOnSameLevel(lines, i) + 1;
                            }
                            else
                            {
                                //We're running the packtime code
                                i = ProcessToEndStatement(lines, i + 1, false);
                                Console.WriteLine("End of PACKTIME: " + i);
                                i++;
                                return i; //Return, as this is ALWAYS called by this.RunPacktimeCommand()
                            }
                            break;
                        case "RESTORETIME":
                            if (isInitializerCall)
                            {
                                //We're done reading, since we're just running the initializer
                                //Store the PC of the packtime declaration
                                Console.WriteLine("Encountered RESTORETIME command.  In Initializer call, returning.");
                                restoretimeCodeStartIndex = i;
                                i = FindNextEndOnSameLevel(lines, i) + 1;
                            }
                            else
                            {
                                //We're running the packtime code
                                i = ProcessToEndStatement(lines, i + 1, false) + 1;
                            }
                            break;
                        case "PACK":
                        {
                            Console.WriteLine("PACK COMMAND : " + lineParts[1]);
                            if (lineParts.Length < 2)
                                throw new Exception("PACK invalid syntax.  Expected: 'PACK LocalPath'.  ERROR @ ln " + i);

                            string path = ResolveVariableToValue(lineParts[1]);

                            string rafPath = mainForm.GuessRafPathFromPath(path);
                            if (rafPath == "undefined")
                            {
                                Console.WriteLine("Pack probably failed - Didn't pack " + path);
                            }
                            else
                            {
                                RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(
                                    mainForm.GuessRafPathFromPath(path)
                                );

                                string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + entry.RAFArchive.GetID() + "_" + entry.FileName.Replace("/", "_");
                                if(!File.Exists(fileBackupLoc))
                                {
                                    mainForm.PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                    File.WriteAllBytes(fileBackupLoc, entry.GetContent());
                                }

                                entry.RAFArchive.InsertFile(
                                    entry.FileName,
                                    File.ReadAllBytes(path),
                                    null
                                );
                            }
                            i++;
                            break;
                        }
                        case "PACKDIR":
                        {
                            Console.WriteLine("PACKDIR COMMAND : " + lineParts[1]);
                            if (lineParts.Length < 2)
                                throw new Exception("PACKDIR invalid syntax.  Expected: 'PACKDIR LocalPath'.  ERROR @ ln " + i);

                            string localPath = ResolveVariableToValue(lineParts[1]);
                            string[] paths = Util.GetAllChildFiles(scriptDir + "/" + localPath);
                            foreach (string path in paths)
                            {
                                string rafPath = mainForm.GuessRafPathFromPath(path);
                                if (rafPath == null || rafPath == "undefined")
                                {
                                    Console.WriteLine("Pack probably failed - Didn't pack " + path);
                                }
                                else
                                {
                                    RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(
                                        mainForm.GuessRafPathFromPath(path)
                                    );

                                    string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + entry.RAFArchive.GetID() + "_" + entry.FileName.Replace("/", "_");
                                    if (!File.Exists(fileBackupLoc))
                                    {
                                        mainForm.PrepareDirectory(Environment.CurrentDirectory + "/backup/");
                                        File.WriteAllBytes(fileBackupLoc, entry.GetContent());
                                    }

                                    entry.RAFArchive.InsertFile(
                                        entry.FileName,
                                        File.ReadAllBytes(path),
                                        null
                                    );
                                }
                            }
                            i++;
                            break;
                        }
                        //The parameter can either be a RAF path or a LOCAL path... doesn't matter
                        case "RESTORE":
                        {
                            Console.WriteLine("RESTORE COMMAND : " + lineParts[1]);
                            if (lineParts.Length < 2)
                                throw new Exception("RESTORE invalid syntax.  Expected: 'RESTORE RAFPath'.  ERROR @ ln " + i);
                            string rafPath = ResolveVariableToValue(lineParts[1]);

                            string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + rafPath.Replace("/", "_");

                            RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(rafPath);
                            if (entry == null)
                                throw new Exception("RESTORE: Entry '" + rafPath + "' did NOT exist, or could NOT be found. @ ln " + i);
                            else
                            {
                                entry.RAFArchive.InsertFile(
                                    entry.FileName, File.ReadAllBytes(fileBackupLoc), null
                                );
                            }
                            i++;
                            break;
                        }

                        //The parameter must be a LOCAL path
                        case "RESTOREDIR":
                        {
                            Console.WriteLine("RESTOREDIR COMMAND : " + lineParts[1]);
                            if (lineParts.Length < 2)
                                throw new Exception("RESTOREDIR invalid syntax.  Expected: 'RESTOREDIR LocalDirPath'.  ERROR @ ln " + i);
                            string localDirPath = ResolveVariableToValue(lineParts[1]);
                            string[] paths = Util.GetAllChildFiles(scriptDir + "/" + localDirPath);

                            foreach(string path in paths)
                            {
                                RAFFileListEntry entry = mainForm.ResolveRAFPathToEntry(
                                    mainForm.GuessRafPathFromPath(path)
                                );

                                string fileBackupLoc = Environment.CurrentDirectory + "/backup/" + entry.RAFArchive.GetID() + "_" + entry.FileName.Replace("/", "_");

                                if (entry == null)
                                    throw new Exception("RESTORE: Entry similar to '" + path + "' did NOT exist, or could NOT be found. @ ln " + i);
                                else
                                {
                                    if(File.Exists(fileBackupLoc)) //Only swap if a backup exists.  Otherwise, we haven't changed it either way.
                                        entry.RAFArchive.InsertFile(
                                            entry.FileName, File.ReadAllBytes(fileBackupLoc), null
                                        );
                                }
                            }
                            i++;
                            break;
                        }
                        case "IF":
                        {
                            Console.WriteLine("IF STATEMENT : " + line);
                            if (lineParts[2] == "==" || lineParts[2] == "!=")
                            {
                                bool eval = ResolveVariableToValue(lineParts[1]) == ResolveVariableToValue(lineParts[3]);
                                if (lineParts[2] == "!=") eval = !eval;

                                if (eval)
                                {
                                    Console.WriteLine("EVAL");
                                    if (lines[i + 1].ToUpper().Trim() == "THEN")
                                    {
                                        i = ProcessToEndStatement(lines, i + 1, false, false) + 1;
                                    }
                                    else
                                    {
                                        i ++;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("NO EVAL");
                                    if (lines[i + 1].ToUpper().Trim() == "THEN")
                                    {
                                        Console.WriteLine("THEN " + (i + 1));
                                        i = FindNextEndOnSameLevel(lines, i + 1) + 1;
                                    }
                                    else
                                    {
                                        i += 2;
                                        Console.WriteLine(i);
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception("IF STATEMENT only supports IF [varname/value] == [varname/value] ERROR @ ln " + i);
                            }
                            break;
                        }
                        case "THEN":
                            return ProcessToEndStatement(lines, i + 1, false);
                            break;
                        default:
                            throw new Exception("INVALID SYNTAX @ LN "+ i +" - Unknown command");
                            break;
                    }
                }
                if (!multilineExec) return i;
            }
            return i;
        }
        /// <param name="lines"></param>
        /// <param name="startIndex">The index of the OPTIONS block</param>
        /// <returns>index of last END</returns>
        private int ProcessOptionsBlock(string[] lines, int startIndex)
        {
            int i = startIndex + 1; //Move to the first SETTING or END
            for (; i < lines.Length && lines[i].Trim().ToUpper() != "END";)
            {
                Console.WriteLine("ProcessOptionsBlock @ ln" + i);
                string line = lines[i].Trim();
                string[] lineParts = line.QASS(' ');

                //The only valid statement inside of a OPTIONS block is either "end" or "setting"
                if (lineParts.Length > 0)
                {
                    if (lineParts[0].ToUpper() != "SETTING")
                        throw new Exception("Expected SETTING block inside OPTIONS block @ line " + i);
                    else
                    {
                        RMPropSetting setting;
                        if (lineParts.Length >= 4)
                            setting = new RMPropSetting(lineParts[1], lineParts[2], lineParts[3]);
                        else
                            setting = new RMPropSetting(lineParts[1], lineParts[2], "");

                        i = ProcessSettingsBlock(lines, i, setting) + 1;
                        Console.WriteLine(i);
                        Settings.Add(setting);
                    }
                }
                else
                {
                    i++;
                }
            }
            return i;
        }
        /// <param name="lines"></param>
        /// <param name="startIndex">Index of the first SETTING</param>
        /// <returns></returns>
        private int ProcessSettingsBlock(string[] lines, int startIndex, RMPropSetting setting)
        {
            int i = startIndex + 1; //move directly to the first OPTION or END
            for (; i < lines.Length && lines[i].Trim().ToUpper() != "END"; i++)
            {
                Console.WriteLine("ProcessSettingsBlock @ ln" + i);
                string line = lines[i].Trim();
                string[] lineParts = line.QASS(' ');

                //The only valid statement is OPTION.  The second parameter can be DEFAULT only.
                if (lineParts[0].ToUpper() != "OPTION")
                    throw new Exception("Expected OPTION statement inside SETTING block @ line " + i);
                else if(lineParts.Length < 2) //only OPTION on the line
                    throw new Exception("Expected OPTION statement followed by either DEFAULT or [option's text] @ line " + i);
                else
                {
                    if (lineParts[1] == "DEFAULT" && lineParts.Length == 3) //OPTION DEFAULT [text]
                        setting.AddOption(lineParts[2], true);
                    else
                        setting.AddOption(lineParts[1], false);
                }
            }
            return i;
        }
        
        public void RunPacktimeCommand()
        {
            if (packtimeCodeStartIndex == -1)
                Console.WriteLine("Packtime Command was not found.");
            else
            {
                Console.WriteLine("Running Packtime Command @ ln " + packtimeCodeStartIndex);
                ProcessToEndStatement(lines, packtimeCodeStartIndex, false);
            }
        }
        public void RunRestoreTimeCommand()
        {
            if (restoretimeCodeStartIndex  == -1)
                Console.WriteLine("Restoretime Command was not found.");
            else
            {
                Console.WriteLine("Running Restoretime Command @ ln " + restoretimeCodeStartIndex);
                ProcessToEndStatement(lines, restoretimeCodeStartIndex, false);
            }
        }
        
        public string ResolveVariableToValue(string varName)
        {
            return ResolveVariableToValue(varName, false);
        }
        private string ResolveVariableToValue(string varName, bool isResursiveCall)
        {
            if (variables.ContainsKey(varName))
                return variables[varName];
            else
            {
                //Check options
                for (int i = 0; i < Settings.Count; i++)
                    if (Settings[i].VarName == varName)
                    {
                        Console.WriteLine("USE SETTING : " + Settings[i].VarName + " : " + Settings[i].SelectedValue);
                        return Settings[i].SelectedValue;
                    }
                return varName;
            }
        }
        private int FindNextEndOnSameLevel(string[] lines, int startSearchIndex)
        {
            int inCount = 0; //How many blocks in we are.  return only when this becomes 0.
            for (int i = startSearchIndex; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                string[] lineParts = line.QASS(' ');
                if(line.Trim() != "")
                    switch (lineParts[0].ToUpper())
                    {
                        case "OPTIONS":
                        case "SETTING":
                        case "PACKTIME":
                        case "RESTORETIME":
                        case "THEN":
                            inCount++;
                            break;
                        case "END":
                            inCount--;
                            if (inCount == 0) return i;
                            break;
                    }
            }
            return -1; //Another end wasn't found
        }
        public void SetOption(string varName, string value)
        {
            foreach (RMPropSetting setting in Settings)
            {
                if (setting.VarName == varName)
                {
                    setting.SelectedValue = value;
                    return;
                }
            }
        }

        public class RMPropSetting
        {
            public string VarName { get; set; }
            public string Label { get; set; }
            public string Comment { get; set; }

            private List<string> options = new List<string>();

            private string defaultValue = "";
            public string SelectedValue { get; set; }
            public RMPropSetting(string varName, string label, string comment)
            {
                this.VarName = varName;
                this.Label = label;
                this.Comment = comment;
            }
            public void AddOption(string optionValue, bool isDefault)
            {
                options.Add(optionValue);
                if (isDefault)
                {
                    defaultValue = optionValue;
                    SelectedValue = optionValue;
                }
            }
            public string[] GetOptions()
            {
                return options.ToArray();
            }
            public string GetDefaultValue()
            {
                return defaultValue;
            }
        }
    }
}
