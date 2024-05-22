using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    internal class Module
    {
        /// <summary>
        /// Name of the Module
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// Every Module has its own ID, given by the universty.
        /// This Stat is for identification.
        /// </summary>
        public int ModuleID { get; set; }

        /// <summary>
        /// The Professor who is in charge
        /// </summary>
        public string Professor { get; set; }

        /// <summary>
        /// Constructor for a module
        /// Contains the name of the professor in charge
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        public Module(string name, int ID)
        {
            ModuleName = name;
            ModuleID = ID;
            Professor = string.Empty;

            //maybe hier noch der User -> entweder Professor = user; oder user noch extra hinzufügen
        }

        /// <summary>
        /// Constructor for a module with ProfessorName.
        /// </summary>
        public Module(string name, int identifire, string professor) 
        {
            ModuleName = name;
            ModuleID = identifire;
            Professor = professor;
        }

        /// <summary>
        /// Function to add a Professor to the Module-Element after it was declared
        /// </summary>
        public void addProfessor(string profName) 
        {
            Professor = profName;
        }

        /// <summary>
        /// Function to add the module to the database.
        /// </summary>
        /// <param name="modulename"></param>
        /// <param name="moduleID"></param>
        /// <param name="professor"></param>
        public void AddToDB (string modulename, int moduleID, string professor)
        {
            // SQL Code
        }
    }
}
