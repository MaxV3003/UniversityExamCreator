using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Models
{
    internal class Taskgroupe
    {
        /// <summary>
        /// Name for a Groupe. This is the Identifire for it. 
        /// </summary>
        public string GroupeName { get; set; }

        /// <summary>
        /// List of Tasks which belongs to the Groupe. 
        /// </summary>
        public List<Task> Task { get; set; }

        /// <summary>
        /// Constructor for a Taskgoupe. 
        /// </summary>
        public Taskgroupe(string groupeName)
        {
            GroupeName = groupeName;
            Task = new List<Task>();
        }

        /// <summary>
        /// Function to add a Task to the TaskList. 
        /// </summary>
        public void addTask(Task task)
        {
            Task.Add(task);
        }

        /// <summary>
        /// Methode to delete a specific Task from the List. 
        /// </summary>
        /// <param name="task">The Task object to be deleted from the list.</param>
        public void deleteTask(Task task)
        {
            Task.Remove(task);
        }

        /// <summary>
        /// Methode to check if an Item is in a Groupe. 
        /// </summary>
        /// <returns>True if the Task is in the Groupe.</returns>
        public bool containsTask(Task task) 
        {
            return Task.Contains(task);
        }

        /// <summary>
        /// Methode to get the Size of the Taskgroupe.
        /// </summary>
        /// <returns>Returns the Number of Tasks in the Groupe.</returns>
        public int groupeSize()
        {
            return Task.Count();
        }

        /// <summary>
        /// Method to display group details
        /// </summary>
        public void DisplayGroup()
        {
            //add Code
        }
    }
}
