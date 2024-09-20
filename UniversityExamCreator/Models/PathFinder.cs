using System;
using System.IO;

namespace UniversityExamCreator.Models
{
    internal class PathFinder
    {
        private String currentPath;

        public PathFinder(string dir, string file) 
        {
            // Get Path of Directory.
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\"));
            string relativePath = @""+dir+ "\\" + file;
            string fullPath = Path.Combine(projectDirectory, relativePath);

            currentPath = fullPath;
            //currentPath = ConvertBackslashesToSlashes(fullPath);
        }

        public PathFinder(string dir)
        {
            // Get Path of Directory.
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\"));
            string relativePath = @"" + dir;
            string fullPath = Path.Combine(projectDirectory, relativePath);

            currentPath = fullPath;
            //currentPath = ConvertBackslashesToSlashes(fullPath);
        }

        public string GetPath() 
        {
            return currentPath;
        }

        /*public string ConvertBackslashesToSlashes(string input)
        {
            return input.Replace("\\", "/");
        }*/
    }
}
