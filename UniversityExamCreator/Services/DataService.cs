using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Services
{
    internal class DataService
    {


public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        public void InsertAufgabe()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertAufgabe = @"
                INSERT INTO aufgabe (task_content, points, difficulty, subject, date_created, author) 
                VALUES (@task_content, @points, @difficulty, @subject, @date_created, @author);";

                using (SQLiteCommand cmd = new SQLiteCommand(insertAufgabe, conn))
                {
                    Console.Write("Enter task content: ");
                    string taskContent = Console.ReadLine();
                    Console.Write("Enter points: ");
                    int points = int.Parse(Console.ReadLine());
                    Console.Write("Enter difficulty: ");
                    string difficulty = Console.ReadLine();
                    Console.Write("Enter subject: ");
                    string subject = Console.ReadLine();
                    Console.Write("Enter author: ");
                    string author = Console.ReadLine();

                    cmd.Parameters.AddWithValue("@task_content", taskContent);
                    cmd.Parameters.AddWithValue("@points", points);
                    cmd.Parameters.AddWithValue("@difficulty", difficulty);
                    cmd.Parameters.AddWithValue("@subject", subject);
                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                    cmd.Parameters.AddWithValue("@author", author);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayAufgabe()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectAufgabe = "SELECT * FROM aufgabe;";

                using (SQLiteCommand cmd = new SQLiteCommand(selectAufgabe, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Task Content: {reader["task_content"]}, Points: {reader["points"]}, Difficulty: {reader["difficulty"]}, Subject: {reader["subject"]}, Date Created: {reader["date_created"]}, Author: {reader["author"]}");
                    }
                }
            }
        }

        public void InsertKlausur()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertKlausur = @"
                INSERT INTO klausur (course, examiner, date) 
                VALUES (@course, @examiner, @date);";

                using (SQLiteCommand cmd = new SQLiteCommand(insertKlausur, conn))
                {
                    Console.Write("Enter course: ");
                    string course = Console.ReadLine();
                    Console.Write("Enter examiner: ");
                    string examiner = Console.ReadLine();
                    Console.Write("Enter date (yyyy-mm-dd): ");
                    DateTime date = DateTime.Parse(Console.ReadLine());

                    cmd.Parameters.AddWithValue("@course", course);
                    cmd.Parameters.AddWithValue("@examiner", examiner);
                    cmd.Parameters.AddWithValue("@date", date);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayKlausur()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectKlausur = "SELECT * FROM klausur;";

                using (SQLiteCommand cmd = new SQLiteCommand(selectKlausur, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Course: {reader["course"]}, Examiner: {reader["examiner"]}, Date: {reader["date"]}");
                    }
                }
            }
        }

        public void InsertKlausurAufgabe()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertKlausurAufgabe = @"
                INSERT INTO klausur_aufgabe (klausur_id, aufgabe_id) 
                VALUES (@klausur_id, @aufgabe_id);";

                using (SQLiteCommand cmd = new SQLiteCommand(insertKlausurAufgabe, conn))
                {
                    Console.Write("Enter klausur_id: ");
                    int klausurId = int.Parse(Console.ReadLine());
                    Console.Write("Enter aufgabe_id: ");
                    int aufgabeId = int.Parse(Console.ReadLine());

                    cmd.Parameters.AddWithValue("@klausur_id", klausurId);
                    cmd.Parameters.AddWithValue("@aufgabe_id", aufgabeId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayKlausurAufgabe()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectKlausurAufgabe = "SELECT * FROM klausur_aufgabe;";

                using (SQLiteCommand cmd = new SQLiteCommand(selectKlausurAufgabe, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Klausur ID: {reader["klausur_id"]}, Aufgabe ID: {reader["aufgabe_id"]}");
                    }
                }
            }
        }

        public void InsertNutzer()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertNutzer = @"
                INSERT INTO nutzer (username, password) 
                VALUES (@username, @password);";

                using (SQLiteCommand cmd = new SQLiteCommand(insertNutzer, conn))
                {
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();
                    Console.Write("Enter password: ");
                    string password = Console.ReadLine();

                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayNutzer()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectNutzer = "SELECT * FROM nutzer;";

                using (SQLiteCommand cmd = new SQLiteCommand(selectNutzer, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Username: {reader["username"]}, Password: {reader["password"]}");
                    }
                }
            }
        }

        public void InsertAntwort()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string insertAntwort = @"
                INSERT INTO antwort (aufgabe_id, answer_content, username) 
                VALUES (@aufgabe_id, @answer_content, @username);";

                using (SQLiteCommand cmd = new SQLiteCommand(insertAntwort, conn))
                {
                    Console.Write("Enter aufgabe_id: ");
                    int aufgabeId = int.Parse(Console.ReadLine());
                    Console.Write("Enter answer content: ");
                    string answerContent = Console.ReadLine();
                    Console.Write("Enter username: ");
                    string username = Console.ReadLine();

                    cmd.Parameters.AddWithValue("@aufgabe_id", aufgabeId);
                    cmd.Parameters.AddWithValue("@answer_content", answerContent);
                    cmd.Parameters.AddWithValue("@username", username);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DisplayAntwort()
        {
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                conn.Open();

                string selectAntwort = "SELECT * FROM antwort;";

                using (SQLiteCommand cmd = new SQLiteCommand(selectAntwort, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader["id"]}, Aufgabe ID: {reader["aufgabe_id"]}, Answer Content: {reader["answer_content"]}, Username: {reader["username"]}");
                    }
                }
            }
        }
    }

}
}

