using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Models
{
    internal class Answer
    {
        public string Name { get; set; }
        public string Content { get; set; }

        public Answer(string name, string content) 
        { 
            Name = name;
            Content = content;
        }

        /// <summary>
        /// Methode to get the Name of a Task-Item.
        /// </summary>
        public void getTaskName(Task task) 
        {
            Name = task.getName();
        }
    }
}
