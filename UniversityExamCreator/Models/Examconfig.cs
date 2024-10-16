using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    internal class Examconfig
    {
        public int ExamId { get; set; }

        /// <summary>
        /// Name / Identifire of the Examconfig-Element.
        /// </summary>
        public string ConfigName { get; set; }

        /// <summary>
        /// Identifire of the Module. 
        /// </summary>
        public string ModuleID {  get; set; }

        /// <summary>
        /// ExamName. 
        /// </summary>
        public string ExamName { get; set; }

        /// <summary>
        /// Typ of the Exam. "Einheitliche Form" or "Mischform".
        /// </summary>
        public string ExamType { get; set; }

        /// <summary>
        /// Amount of Tasks which should be included in the Exam.
        /// </summary>
        public int TaskAmount { get; set; }

        /// <summary>
        /// Amount of Points the Exam contains. 
        /// </summary>
        public double PointAmount {  get; set; }

        /// <summary>
        /// List of Taskgroups the Exam contains. 
        /// </summary>
        public List<Taskgroupe> TaskGroups { get; set; }

        /// <summary>
        /// Empty Constructor for an Examconfig-Element.
        /// </summary>
        public Examconfig() 
        {
            ConfigName = string.Empty;
            ExamName = string.Empty;
            ModuleID = string.Empty;
            ExamType = string.Empty;
            TaskAmount = 0;
            PointAmount = 0.0;
            TaskGroups = new List<Taskgroupe>();
        }
        /// <summary>
        /// Constructor for an Examconfig-Element.
        /// </summary>
        public Examconfig(string configName, string examName, string moduleID, string examTyp) 
        {
            ConfigName = configName;
            ExamName = examName;
            ModuleID = moduleID;
            ExamType = examTyp;
            TaskAmount = 0;
            PointAmount = 0;
            TaskGroups = new List<Taskgroupe>();
        }

        /// <summary>
        /// Methode to set the Amount of Tasks an Exam should contain.
        /// </summary>
        public void setTaskAmount(int taskAmount)
        {
            TaskAmount = taskAmount;
        }

        /// <summary>
        /// Methode to set the Amount of Points an Exam shoukd contain.
        /// </summary>
        public void setPointAmount(int pointAmount) 
        { 
            PointAmount = pointAmount;   
        }

        /// <summary>
        /// Taskgroupe the Exam should contain. 
        /// </summary>
        public void addTaskGroupe(Taskgroupe taskGroupe) 
        {
            TaskGroups.Add(taskGroupe);
        }

        public void toString() 
        {
            Console.WriteLine("ConfigName: " + ConfigName + "\n" + 
                "ExamName: " + ExamName + "\n" +
                "ModuleID: " + ModuleID + "\n" +
                "ExamType: " + ExamType + "\n" +
                "TaskAmount: " + TaskAmount + "\n" + 
                "PointAmount: " + PointAmount + "\n" +
                "TaskGroups: " + TaskGroups.ToString());
        }

        public bool isEmpty() 
        {
            if(ConfigName.Equals(string.Empty) && ExamName.Equals(string.Empty) && ModuleID.Equals(string.Empty) && ExamType.Equals(string.Empty) && (TaskAmount==0) && (!((PointAmount > 0) && (PointAmount < 0))))
            {
                return true;
            }
            else 
            { 
                return false; 
            }
        }
    }
}
