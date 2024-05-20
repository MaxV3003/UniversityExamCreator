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
        /// Methode to get the Properties of a Task-Item.
        /// </summary>
        /// <param name="task">Specific Task to extract from.</param>
        public void extract(Task task) 
        {
            Name = task.getName();
            Content = task.getContent();
        }
    }
}
