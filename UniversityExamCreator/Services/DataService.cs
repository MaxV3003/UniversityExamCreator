using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Services
{
    public class DataService
    {
        private string connection;

        public DataService(string connectionString)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            
            connection = $"Data Source="+connectionString+";Version=3;";
            

        }
        
        public void InsertKlausurAufgabe(string topic, string taskType, string difficulty, int points, string taskName, string taskContent, DateTime dateCreated, string author)
        {

            string test = "Data Source=C:/Users/Max/source/repos/UniversityExamCreator/UniversityExamCreator/Databases/database.db;Version=3;";
            using (SQLiteConnection conn = new SQLiteConnection(test))
            {
                conn.Open();
                string insertQuery = @"
                    INSERT INTO aufgabe (topic, type, difficulty, points, name, content, date_created, author) 
                    VALUES (@topic, @type, @difficulty, @points, @name, @content, @date_created, @author)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, conn))
                {
                    command.Parameters.AddWithValue("@topic", topic);
                    command.Parameters.AddWithValue("@type", taskType);
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    command.Parameters.AddWithValue("@points", points);
                    command.Parameters.AddWithValue("@name", taskName);
                    command.Parameters.AddWithValue("@content", taskContent);
                    command.Parameters.AddWithValue("@date_created", dateCreated);
                    command.Parameters.AddWithValue("@author", author);

                    command.ExecuteNonQuery();
                }
            }
        }


        //-----------------------------------
        // Delete Aufgabe
        //-----------------------------------
        /*
        public void DeleteAufgabe(int id)
        {
            connection.Open();
            string query = "INSERT INTO exam (course, examiner, date) VALUES (@course, @examiner, @date)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@course", course);
                command.Parameters.AddWithValue("@examiner", examiner);
                command.Parameters.AddWithValue("@date", date);
                command.ExecuteNonQuery();
                Console.WriteLine("Exam erfolgreich gespeichert.");
            }
        }

        public void InsertExamTask(int exam_id, int task_id)
        {
            connection.Open();
            string query = "INSERT INTO exam_task (exam_id, task_id) VALUES (@exam_id, @task_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@exam_id", exam_id);
                command.Parameters.AddWithValue("@task_id", task_id);
                command.ExecuteNonQuery();
            }
        }

        public void InsertUser(string username, string password)
        {
            connection.Open();
            string query = "INSERT INTO user (username, password) VALUES (@username, @password)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
                Console.WriteLine("User erfolgreich gespeichert.");
            }
        }

        public void InsertAnswer(int task_id, string answer_content, string username)
        {
            connection.Open();
            string query = "INSERT INTO answer (task_id, answer_content, username) VALUES (@task_id, @answer_content, @username)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@task_id", task_id);
                command.Parameters.AddWithValue("@answer_content", answer_content);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
                Console.WriteLine("Answer erfolgreich gespeichert.");
            }
        }

        public void InsertExamConfig(int exam_id, int user_id)
        {
            connection.Open();
            string query = "INSERT INTO exam_config (exam_id, user_id) VALUES (@exam_id, @user_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@exam_id", exam_id);
                command.Parameters.AddWithValue("@user_id", user_id);
                command.ExecuteNonQuery();
                Console.WriteLine("Config erfolgreich gespeichert.");
            }
        }

        public void InsertTaskAnswer(int answer_id, int task_id)
        {
            connection.Open();
            string query = "INSERT INTO task_answer (answer_id, task_id) VALUES (@answer_id, @task_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@answer_id", answer_id);
                command.Parameters.AddWithValue("@task_id", task_id);
                command.ExecuteNonQuery();
            }
        }

        public void InsertModule(int module_id, string faculty)
        {
            connection.Open();
            string query = "INSERT INTO module (module_id, faculty) VALUES (@module_id, @faculty)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@module_id", module_id);
                command.Parameters.AddWithValue("@faculty", faculty);
                command.ExecuteNonQuery();
                Console.WriteLine("Module erfolgreich gespeichert.");
            }
        }

        //----------------------------------- 
        // Delete Section
        //-----------------------------------

        public void DeleteTask(int id)
        {
            connection.Open();
            string query = "DELETE FROM task WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Task erfolgreich gelöscht.");
            }
        }

        public void DeleteExam(int id)
        {
            connection.Open();
            string query = "DELETE FROM exam WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Exam erfolgreich gelöscht.");
            }
        }

        public void DeleteExamTask(int id)
        {
            connection.Open();
            string query = "DELETE FROM exam_task WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteUser(int id)
        {
            connection.Open();
            string query = "DELETE FROM user WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("User erfolgreich gelöscht.");
            }
        }

        public void DeleteAnswer(int id)
        {
            connection.Open();
            string query = "DELETE FROM answer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Answer erfolgreich gelöscht.");
            }
        }

        public void DeleteExamConfig(int id)
        {
            connection.Open();
            string query = "DELETE FROM exam_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Config erfolgreich gelöscht.");
            }
        }

        public void DeleteTaskAnswer(int id)
        {
            connection.Open();
            string query = "DELETE FROM task_answer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteModule(int id)
        {
            connection.Open();
            string query = "DELETE FROM module WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Module erfolgreich gelöscht.");
            }
        }

        //----------------------------------- 
        // If-Exists Section
        //-----------------------------------

        public bool IfTaskExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM task WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfExamExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM exam WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfExamTaskExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM exam_task WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfUserExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM user WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfAnswerExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM answer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfExamConfigExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM exam_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfTaskAnswerExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM task_answer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public bool IfModuleExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM module WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }
}
