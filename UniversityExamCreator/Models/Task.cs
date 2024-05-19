using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    internal class Task
    {
        /// <summary>
        /// Name of a Task. Its the Identifire for it.
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
        /// Constructor for a task without an Answer. 
        /// </summary>
        public Task(string taskName, string taskContent) 
        {
            TaskName = taskName;
            TaskContent = taskContent;
        }

        /// <summary>
        /// Constructor for a task with an Answer. 
        /// </summary>
        public Task(string taskName, string taskContent, string taskAnswer) 
        {
            TaskName = taskName;
            TaskContent = taskContent;
            TaskAnswer = taskAnswer;
        }

        /// <summary>
        /// Methode to create a new Task without an Answer.
        /// </summary>
        /// <returns>Returns a new Task.</returns>
        public Task createTask(string taskName, string taskContent) 
        {
            return new Task(taskName, taskContent);
        }

        /// <summary>
        /// Methode to get a String of the Name of the Task.
        /// </summary>
        /// <returns>String-Name</returns>
        public string getName() 
        {
            return TaskName;
        }

        /// <summary>
        /// Methode to get a String of the Content of the Task.
        /// </summary>
        /// <returns>String-Content</returns>
        public string getContent() 
        {
            return TaskContent;
        }

        /// <summary>
        /// Mehtode to create a new Task with an Answer
        /// </summary>
        /// <returns>Returns a new Task.</returns>
        public Task createTask(string taskName, string taskContent, string taskAnswer)
        {
            return new Task(taskName, taskContent, taskAnswer);
        }
    }
}
