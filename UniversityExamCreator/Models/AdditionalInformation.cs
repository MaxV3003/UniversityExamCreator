using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    internal class AdditionalInformation
    {
        //muss Public sein damits in ListViewElementen angezeigt werden kann
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public string Content { get; set; }
        public AdditionalInformation(string name, bool check, string content) 
        {
            Name = name;
            IsChecked = check;
            Content = content;
        }
    }
}
