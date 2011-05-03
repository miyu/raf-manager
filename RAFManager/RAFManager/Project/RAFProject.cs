using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAFManager.Project
{
    public class RAFProject
    {
        RAFProjectInfo projectInfo  = null;
        public bool hasChanged      = false;
        public RAFProject()
        {
            projectInfo = new RAFProjectInfo();
            projectInfo.ProjectName = "Untitled Project";
            projectInfo.ProjectPath = "";
            projectInfo.FileArchivesDirectory = "";
        }
        public RAFProjectInfo ProjectInfo
        {
            get
            {
                return projectInfo;
            }
        }
        public bool HasChanged
        {
            get
            {
                return hasChanged;
            }
        }

        public void Save(string location)
        {
            string serialization = projectInfo.ProjectName;
        }
    }
}
