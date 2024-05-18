using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation.Peers;
using System.Windows.Markup;

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
        public List<Groupe> Groups { get; set; } 



        /// <summary>
        /// Constructor for an Exam. 
        /// It contains the specific Name and a List of GroupeItems which contains the Tasks for the exam. 
        /// </summary>
        public Exam(string name, string userID)
        {
            Identifire = name;
            UserID = userID;
            Groups = new List<Groupe>();
        }

        /// <summary>
        /// Method to add a group to the GroupeList.
        /// </summary>
        public void AddGroup(Groupe group)
        {
            Groups.Add(group);
        }

        /// <summary>
        /// Method to display exam details.
        /// </summary>
        public void DisplayExam()
        {
            Console.WriteLine($"Exam Name: {Identifire}");
            Console.WriteLine("Groups:");
            foreach (var group in Groups)
            {
                group.DisplayGroup();
                Console.WriteLine();
            }
        }
    }


    /// <summary>
    /// A Goupe is a specific Frame for a Question. 
    /// It contains:
    ///     the Heading of the Question
    ///     the Content of the Question
    ///     the optional Answer for the Question
    /// </summary>
    internal class Groupe
    {
        /// <summary>
        /// Name of a Task.
        /// </summary>
        public string TaskName { get; set; }

        /// <summary>
        /// Content of the Task.
        /// </summary>
        public string TaskContent { get; set; }

        /// <summary>
        /// Optional Answer for a Task.
        /// </summary>
        public string TaskAnswer { get; set; }


        /// <summary>
        /// Constructor for a GoupeItem without Answer.
        /// </summary>
        public Groupe(string taskName, string taskContent)
        {
            TaskName = taskName;
            TaskContent = taskContent;

        }

        /// <summary>
        /// Constructor for a GoupeItem with an optional Answer. 
        /// </summary>
        public Groupe(string taskName, string taskContent, string taskAnswer)
        {
            TaskName = taskName;
            TaskContent = taskContent;
            TaskAnswer = taskAnswer;
        }

        /// <summary>
        /// Method to display group details
        /// </summary>
        public void DisplayGroup()
        {
            Console.WriteLine($"Task Name: {TaskName}");
            Console.WriteLine($"Task Content: {TaskContent}");
            if (TaskAnswer != null)
            {
                Console.WriteLine($"Task Answer: {TaskAnswer}");
            }
            else
            {
                Console.WriteLine("Task Answer: Not provided");
            }
        }
    }
}