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

            if (!CorrectPath(fullPath)) 
            {
                currentPath = fullPath;
            }
        }

        public PathFinder(string dir)
        {
            // Get Path of Directory.
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectDirectory = Path.GetFullPath(Path.Combine(basePath, @"..\..\"));
            string relativePath = @"" + dir;
            string fullPath = Path.Combine(projectDirectory, relativePath);

            if (CorrectDir(fullPath))
            {
                currentPath = fullPath;
            }
        }

        public bool CorrectPath(string path)
        {
            // Überprüfen, ob der Pfad existiert
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Console.WriteLine("Der Verzeichnispfad existiert nicht.");
                return false;
            }

            // Überprüfen, ob die Datei existiert
            if (!File.Exists(path))
            {
                Console.WriteLine("Die Datei existiert nicht.");
                return false;
            }

            return true;
        }

        public bool CorrectDir(string path)
        {
            // Überprüfen, ob der Pfad existiert
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Console.WriteLine("Der Verzeichnispfad existiert nicht.");
                return false;
            }

            return true;
        }

        public string GetPath() 
        {
            return currentPath;
        }
    }
}
