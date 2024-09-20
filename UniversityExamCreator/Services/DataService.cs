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
            connection = $"Data Source="+connectionString+";Version=3;";
        }
        
        public void InsertKlausurAufgabe(string topic, string taskType, string difficulty, int points, string taskName, string taskContent, DateTime dateCreated, string author)
        {

            //string test = "Data Source=C:/Users/Max/source/repos/UniversityExamCreator/UniversityExamCreator/Databases/database.db;Version=3;";
            Console.WriteLine(connection);
            using (SQLiteConnection conn = new SQLiteConnection(connection))
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
            string query = "DELETE FROM aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Aufgabe erfolgreich gelöscht.");
            }
        }

        public bool IfAufgabeExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausur(string course, string examiner, DateTime date)
        {
            connection.Open();
            string query = "INSERT INTO klausur (course, examiner, date) VALUES (@course, @examiner, @date)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@course", course);
                command.Parameters.AddWithValue("@examiner", examiner);
                command.Parameters.AddWithValue("@date", date);
                command.ExecuteNonQuery();
                Console.WriteLine("Klausur erfolgreich gespeichert.");
            }
        }

        public void DeleteKlausur(int id)
        {
            connection.Open();
            string query = "DELETE FROM klausur WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Klausur erfolgreich gelöscht.");
            }
        }

        public bool IfKlausurExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM klausur WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausurAufgabe(int klausur_id, int aufgabe_id)
        {
            connection.Open();
            string query = "INSERT INTO klausur_aufgabe (klausur_id, aufgabe_id) VALUES (@klausur_id, @aufgabe_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@klausur_id", klausur_id);
                command.Parameters.AddWithValue("@aufgabe_id", aufgabe_id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteKlausurAufgabe(int id)
        {
            connection.Open();
            string query = "DELETE FROM klausur_aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfKlausurAufgabeExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM klausur_aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertNutzer(string username, string password)
        {
            connection.Open();
            string query = "INSERT INTO nutzer (username, password) VALUES (@username, @password)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
                Console.WriteLine("Nutzer erfolgreich gespeichert.");
            }
        }

        public void DeleteNutzer(int id)
        {
            connection.Open();
            string query = "DELETE FROM nutzer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Nutzer erfolgreich gelöscht.");
            }
        }

        public bool IfNutzerExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM nutzer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertAntwort(int aufgabe_id, string answer_content, string username)
        {
            connection.Open();
            string query = "INSERT INTO antwort (aufgabe_id, answer_content, username) VALUES (@aufgabe_id, @answer_content, @username)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@aufgabe_id", aufgabe_id);
                command.Parameters.AddWithValue("@answer_content", answer_content);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
                Console.WriteLine("Antwort erfolgreich gespeichert.");
            }
        }

        public void DeleteAntwort(int id)
        {
            connection.Open();
            string query = "DELETE FROM antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Antwort erfolgreich gelöscht.");
            }
        }

        public bool IfAntwortExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausurConfig(int klausur_id, int nutzer_id)
        {
            connection.Open();
            string query = "INSERT INTO klausur_config (klausur_id, nutzer_id) VALUES (@klausur_id, @nutzer_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@klausur_id", klausur_id);
                command.Parameters.AddWithValue("@nutzer_id", nutzer_id);
                command.ExecuteNonQuery();
                Console.WriteLine("Config erfolgreich gespeichert.");
            }
        }

        public void DeleteKlausurConfig(int id)
        {
            connection.Open();
            string query = "DELETE FROM klausur_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Config erfolgreich gelöscht.");
            }
        }

        public bool IfKlausurConfigExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM klausur_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertAufgabeAntwort(int antwort_id, int aufgabe_id)
        {
            connection.Open();
            string query = "INSERT INTO aufgabe_antwort (antwort_id, aufgabe_id) VALUES (@antwort_id, @aufgabe_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@antwort_id", antwort_id);
                command.Parameters.AddWithValue("@aufgabe_id", aufgabe_id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAufgabeAntwort(int id)
        {
            connection.Open();
            string query = "DELETE FROM aufgabe_antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfAufgabeAntwortExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM aufgabe_antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertModul(int modul_id, string faculty)
        {
            connection.Open();
            string query = "INSERT INTO modul (modul_id, faculty) VALUES (@modul_id, @faculty)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@modul_id", modul_id);
                command.Parameters.AddWithValue("@faculty", faculty);
                command.ExecuteNonQuery();
                Console.WriteLine("Modul erfolgreich gespeichert.");
            }
        }

        public void DeleteModul(int id)
        {
            connection.Open();
            string query = "DELETE FROM modul WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
                Console.WriteLine("Modul erfolgreich gelöscht.");
            }
        }

        public bool IfModulExists(int id)
        {
            connection.Open();
            string query = "SELECT COUNT(1) FROM modul WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }*/
    }
    
}

/*
using System;
using System.Data.SQLite;

namespace UniversityExamCreator.Services
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string databasePath)
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;Initial Catalog=MeineDatenbank;Integrated Security=True";
        }

        public string GetConnectionString()
        {
            return connectionString;
        }
    }

    public class DatabaseOperations : IDisposable
    {
        private SQLiteConnection connection;

        public DatabaseOperations(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Öffnen der Datenbankverbindung: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (connection != null && connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public void InsertAufgabe(string task_content, int points, string difficulty, string subject, DateTime date_created, string author)
        {
            string query = "INSERT INTO aufgabe (task_content, points, difficulty, subject, date_created, author) VALUES (@task_content, @points, @difficulty, @subject, @date_created, @author)";
            ExecuteNonQuery(query, ("@task_content", task_content), ("@points", points), ("@difficulty", difficulty), ("@subject", subject), ("@date_created", date_created), ("@author", author));
        }

        public void DeleteAufgabe(int id)
        {
            string query = "DELETE FROM aufgabe WHERE id = @id";
            ExecuteNonQuery(query, ("@id", id));
        }

        public bool IfAufgabeExists(int id)
        {
            string query = "SELECT COUNT(1) FROM aufgabe WHERE id = @id";
            return ExecuteScalar(query, ("@id", id)) > 0;
        }

        public void InsertKlausur(string course, string examiner, DateTime date)
        {
            string query = "INSERT INTO klausur (course, examiner, date) VALUES (@course, @examiner, @date)";
            ExecuteNonQuery(query, ("@course", course), ("@examiner", examiner), ("@date", date));
        }

        public void DeleteKlausur(int id)
        {
            string query = "DELETE FROM klausur WHERE id = @id";
            ExecuteNonQuery(query, ("@id", id));
        }

        public bool IfKlausurExists(int id)
        {
            string query = "SELECT COUNT(1) FROM klausur WHERE id = @id";
            return ExecuteScalar(query, ("@id", id)) > 0;
        }

        // Weitere Methoden hier (für Klausur-Aufgabe, Nutzer, Antworten, etc.)

        // Hilfsmethode für Nicht-Abfrage-SQL-Befehle (Insert, Update, Delete)
        private void ExecuteNonQuery(string query, params (string, object)[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Item1, param.Item2);
                }

                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler bei der Ausführung der Abfrage: {ex.Message}");
                }
            }
        }

        // Hilfsmethode für Abfrage mit Rückgabe eines Werts 
        private int ExecuteScalar(string query, params (string, object)[] parameters)
        {
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                foreach (var param in parameters)
                {
                    command.Parameters.AddWithValue(param.Item1, param.Item2);
                }

                try
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fehler bei der Ausführung der Abfrage: {ex.Message}");
                    return 0;
                }
            }
        }
    }
}*/



