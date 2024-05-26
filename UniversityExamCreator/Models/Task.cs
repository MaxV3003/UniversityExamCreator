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
        /// Module to which the Task belongs. 
        /// </summary>
        public string Module {  get; set; }

        /// <summary>
        /// List of the Topic of the Answer.
        /// </summary>
        public string Topic { get; set; } 

        /// <summary>
        /// Points for a Task.
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Difficulty-Indicator for the Task. 
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Type of the Task. 
        /// </summary>
        public string TaskType { get; set; }

        /// <summary>
        /// List of MC-Answers (Multiple Choice) for a Task which belongs to the Type: MC.
        /// </summary>
        public List<string> MCAnswers { get; set; }

        /// <summary>
        /// Constructor for a TaskItem.
        /// </summary>
        public Task (string module, string topic, string taskType, string difficulty, int points, string taskName)
        {
            Module = module;
            Topic = topic;  
            TaskType = taskType;
            Difficulty = difficulty;
            Points = points;
            TaskName = taskName;
            TaskContent = string.Empty;
            TaskAnswer = new Answer(taskName, string.Empty);
            MCAnswers = new List<string>();
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
        public void setTaskAnswer(string answerContent) 
        {
            TaskAnswer.Content = answerContent;
        }

        /// <summary>
        /// Methode to set the Content of the Task.
        /// </summary>
        public void setTaskContent(string content) 
        {
            TaskContent = content;
        }

        /// <summary>
        /// Methode to add MC-Answers for the MC-Task.
        /// </summary>
        public void setMCAnswer(string mcAnswer)
        {
            MCAnswers.Add(mcAnswer);
        }
    }
}
