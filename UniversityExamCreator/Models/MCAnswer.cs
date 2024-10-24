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
        /// Content of the MC-Answer
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// ID of the MC-Answer. From 1-10.
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// Flag for the answer if it is right or wrong. 1 for true. 0 for flase.
        /// </summary>
        public int AnswerFlag { get; set; }

        public MCAnswer(string content,int identifire, int answerFlag)
        {
            Content = content;
            ID = identifire;
            AnswerFlag = answerFlag;
        }
    }
}
