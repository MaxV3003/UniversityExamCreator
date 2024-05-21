using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Models
{
    internal class User
    {
        /// <summary>
        /// So we can connect the User to his Password.
        /// Would be wise to use the E-mail adress fron the Ovgu.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// To indentify ever User
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Every User has to set a password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Constructor for a User.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ID"></param>
        /// <param name="password"></param>
        public User(string name, int ID, string password)
        {
            Username = name;
            UserID = ID;
            Password = "";
        }
        /// <summary>
        /// To encrypt the password, so it cant be read out of the database.
        /// </summary>
        /// <param name="password"></param>
        public void encrypt (string password) 
        {
            //encryption with an text we have to choose (forgot the name oft it)
        }
    }
}
