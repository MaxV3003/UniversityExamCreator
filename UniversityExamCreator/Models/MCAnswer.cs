using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    public class MCAnswer
    {
        /// <summary>
        /// Name of the Task it belongs to.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Content of the MC-Answer
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ID of the MC-Answer. From 1-8.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Flag for the answer if it is right or wrong. 1 for true. 0 for flase.
        /// </summary>
        public int AnswerFlag { get; set; }

        public MCAnswer(string name, string content,int identifire, int answerFlag)
        {
            Name = name;
            Content = content;
            ID = identifire;
            AnswerFlag = answerFlag;
        }

        public void getTaskName(Task task) 
        {
            Name = task.TaskName;
        }
    }
}
