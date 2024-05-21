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
        public Answer TaskAnswer { get; set; }

        /// <summary>
        /// Constructor for a task without an Answer. 
        /// </summary>
        public Task(string taskName, string taskContent) 
        {
            TaskName = taskName;
            TaskContent = taskContent;
            TaskAnswer = new Answer(taskName,string.Empty);
        }

        /// <summary>
        /// Constructor for a task with an Answer. 
        /// </summary>
        public Task(string taskName, string taskContent, String answerContent) 
        {
            TaskName = taskName;
            TaskContent = taskContent;
            TaskAnswer = new Answer(taskName, answerContent);
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
        /// Methode to get the AnswerItem of a Task.
        /// </summary>
        /// <returns>AnswerItem</returns>
        public Answer getAnswer() 
        {
            return TaskAnswer;
        }

        /// <summary>
        /// Methode to set the Content of an AnswerItem. 
        /// </summary>
        public void setAnswer(string answerContent) 
        {
            TaskAnswer.Content = answerContent;
        }
    }
}
