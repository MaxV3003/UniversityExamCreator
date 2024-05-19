using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Markup;
using UniversityExamCreator.Models;

//Ist aktuell eine Idee von der Erzeugung eines Exams. 
namespace UniversityExamCreator.Models
{
    internal class Exam
    {
        /// <summary>
        /// Name or ID of the Exam.
        /// </summary>
        public string Identifire { get; set; }

        /// <summary>
        /// ID from the specific User which is creating the Exam. 
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// List of GoupeItems. An Exam contains one or more GoupeItems
        /// </summary>
        public List<Taskgroupe> TaskGroups { get; set; } 

        public List<Task> Task { get; set; }


        /// <summary>
        /// Constructor for an Exam. 
        /// It contains the specific Name and a List of GroupeItems which contains the Tasks for the exam. 
        /// </summary>
        public Exam(string name, string userID)
        {
            Identifire = name;
            UserID = userID;
            TaskGroups = new List<Taskgroupe>();
            Task = new List<Task>();
        }

        /// <summary>
        /// Method to add a group to the GroupeList.
        /// </summary>
        public void AddGroup(Taskgroupe group)
        {
            TaskGroups.Add(group);
        }

        /// <summary>
        /// Method to display exam details.
        /// </summary>
        public void DisplayExam()
        {
            Console.WriteLine($"Exam Name: {Identifire}");
            Console.WriteLine("Groups:");
            foreach (var group in TaskGroups)
            {
                group.DisplayGroup();
                Console.WriteLine();
            }
        }
    }
}