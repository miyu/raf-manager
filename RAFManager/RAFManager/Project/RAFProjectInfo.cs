﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFManager.Project
{
    public class RAFProjectInfo
    {
        private string projectName = "";
        private string projectPath = "";
        private string fileArchivesDirectory = "";

        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                projectName = value;
            }
        }
        public string ProjectPath
        {
            get
            {
                return projectPath;
            }
            set
            {
                projectPath = value;
            }
        }
        public string FileArchivesDirectory
        {
            get
            {
                return fileArchivesDirectory;
            }
            set
            {
                fileArchivesDirectory = value;
            }
        }
    }
}
