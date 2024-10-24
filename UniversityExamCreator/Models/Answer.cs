using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Models
{
    public class Answer
    {
        public int TaskID { get; set; }
        public string Content { get; set; }

        public Answer(int taskID, string content) 
        { 
            TaskID = taskID;
            Content = content;
        }

        public Answer(string content) 
        {
            Content = content;
        }
    }
}
