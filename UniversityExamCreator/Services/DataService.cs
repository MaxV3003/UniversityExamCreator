using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityExamCreator.Services
{
    public class DatabaseManager
    {
        private string connectionString;

        public DatabaseManager(string databasePath)
        {
            connectionString = $"Data Source={databasePath};Version=3;";
        }
    }
    public class DatabaseOperations
    {
        private SQLiteConnection connection;

        public DatabaseOperations(string connectionString)
        {
            connection = new SQLiteConnection(connectionString);
            connection.Open();
        }

        public void InsertAufgabe(string task_content, int points, string difficulty, string subject, DateTime date_created, string author)
        {
            string query = "INSERT INTO aufgabe (task_content, points, difficulty, subject, date_created, author) VALUES (@task_content, @points, @difficulty, @subject, @date_created, @author)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@task_content", task_content);
                command.Parameters.AddWithValue("@points", points);
                command.Parameters.AddWithValue("@difficulty", difficulty);
                command.Parameters.AddWithValue("@subject", subject);
                command.Parameters.AddWithValue("@date_created", date_created);
                command.Parameters.AddWithValue("@author", author);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAufgabe(int id)
        {
            string query = "DELETE FROM aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfAufgabeExists(int id)
        {
            string query = "SELECT COUNT(1) FROM aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausur(string course, string examiner, DateTime date)
        {
            string query = "INSERT INTO klausur (course, examiner, date) VALUES (@course, @examiner, @date)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@course", course);
                command.Parameters.AddWithValue("@examiner", examiner);
                command.Parameters.AddWithValue("@date", date);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteKlausur(int id)
        {
            string query = "DELETE FROM klausur WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfKlausurExists(int id)
        {
            string query = "SELECT COUNT(1) FROM klausur WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausurAufgabe(int klausur_id, int aufgabe_id)
        {
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
            string query = "DELETE FROM klausur_aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfKlausurAufgabeExists(int id)
        {
            string query = "SELECT COUNT(1) FROM klausur_aufgabe WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertNutzer(string username, string password)
        {
            string query = "INSERT INTO nutzer (username, password) VALUES (@username, @password)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteNutzer(int id)
        {
            string query = "DELETE FROM nutzer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfNutzerExists(int id)
        {
            string query = "SELECT COUNT(1) FROM nutzer WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertAntwort(int aufgabe_id, string answer_content, string username)
        {
            string query = "INSERT INTO antwort (aufgabe_id, answer_content, username) VALUES (@aufgabe_id, @answer_content, @username)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@aufgabe_id", aufgabe_id);
                command.Parameters.AddWithValue("@answer_content", answer_content);
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteAntwort(int id)
        {
            string query = "DELETE FROM antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfAntwortExists(int id)
        {
            string query = "SELECT COUNT(1) FROM antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertKlausurConfig(int klausur_id, int nutzer_id)
        {
            string query = "INSERT INTO klausur_config (klausur_id, nutzer_id) VALUES (@klausur_id, @nutzer_id)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@klausur_id", klausur_id);
                command.Parameters.AddWithValue("@nutzer_id", nutzer_id);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteKlausurConfig(int id)
        {
            string query = "DELETE FROM klausur_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfKlausurConfigExists(int id)
        {
            string query = "SELECT COUNT(1) FROM klausur_config WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertAufgabeAntwort(int antwort_id, int aufgabe_id)
        {
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
            string query = "DELETE FROM aufgabe_antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfAufgabeAntwortExists(int id)
        {
            string query = "SELECT COUNT(1) FROM aufgabe_antwort WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }

        public void InsertModul(int modul_id, string faculty)
        {
            string query = "INSERT INTO modul (modul_id, faculty) VALUES (@modul_id, @faculty)";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@modul_id", modul_id);
                command.Parameters.AddWithValue("@faculty", faculty);
                command.ExecuteNonQuery();
            }
        }

        public void DeleteModul(int id)
        {
            string query = "DELETE FROM modul WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                command.ExecuteNonQuery();
            }
        }

        public bool IfModulExists(int id)
        {
            string query = "SELECT COUNT(1) FROM modul WHERE id = @id";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                return Convert.ToInt32(command.ExecuteScalar()) > 0;
            }
        }
    }
}


