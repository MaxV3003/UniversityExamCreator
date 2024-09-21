using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für DBTest.xaml
    /// </summary>
    public partial class DBTest : Page
    {

        private string dbConnectionString;

        public DBTest(string connectionString)
        {
            dbConnectionString = connectionString;
            InitializeComponent();
            //CreateDatabaseAndTable();
            LoadDataFromDatabase(); // Lade die Daten beim Start der Anwendung
        }
        // Methode zum Erstellen der Datenbank und Tabelle
        private void CreateDatabaseAndTable()
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
            {
                connection.Open();
                string createTableQuery = @"CREATE TABLE IF NOT EXISTS People (
                                            Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                            Name TEXT NOT NULL,
                                            Age INTEGER NOT NULL)";
                using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        // Methode zum Laden der Daten aus der Datenbank
        private void LoadDataFromDatabase()
        {
            List<UniversityExamCreator.Models.Task> tasks = new List<UniversityExamCreator.Models.Task>();
           
            using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
            {
                connection.Open();
                string selectQuery = "SELECT id, topic, type, difficulty, points, name, content, date_created, author FROM aufgabe";

                using (SQLiteCommand command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Lese die Felder aus der Datenbank und weise sie der Task-Klasse zu
                            int id = reader.GetInt32(0);                // ID sieht man nicht weil sie nicht in der Taskklasse definiert ist und somit auch nicht im Grid angezeigt werden kann
                            string topic = reader.GetString(1);         // Topic
                            string type = reader.GetString(2);          // TaskType
                            string difficulty = reader.GetString(3);    // Difficulty
                            int points = reader.GetInt32(4);            // Points
                            string name = reader.GetString(5);          // TaskName
                            string content = reader.GetString(6);       // TaskContent
                            DateTime dateCreated = reader.GetDateTime(7); // DateCreated
                            string author = reader.GetString(8);        // Author

                            // Erstelle Task-Objekt und setze die richtigen Eigenschaften
                            tasks.Add(new UniversityExamCreator.Models.Task(topic, author, type, difficulty, points, name));
                        }
                    }
                }
            }

            // Binde die geladenen Daten an das DataGrid
            dataGrid.ItemsSource = tasks;
        }

        // Methode zum Hinzufügen eines Eintrags in die Datenbank
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            // Werte aus den Eingabefeldern lesen
            string topic = txtTopic.Text;                    // Thema der Aufgabe
            string taskType = txtTaskType.Text;              // Art der Aufgabe (z. B. Multiple Choice, Essay)
            string difficulty = txtDifficulty.Text;          // Schwierigkeitsgrad
            int points = int.Parse(txtPoints.Text);          // Punktewert der Aufgabe
            string taskName = txtTaskName.Text;              // Name der Aufgabe
            string taskContent = txtTaskContent.Text;        // Inhalt der Aufgabe
            DateTime dateCreated = dpDateCreated.SelectedDate ?? DateTime.Now;  // Erstellungsdatum (falls nicht ausgewählt, nutze das aktuelle Datum)
            string author = txtAuthor.Text;                  // Autor der Aufgabe

            // Aufgabe zur Datenbank hinzufügen
            AddTaskToDatabase(topic, taskType, difficulty, points, taskName, taskContent, dateCreated, author);

            // Aktualisiere die Anzeige im DataGrid
            LoadDataFromDatabase();

            // Felder nach dem Einfügen der Aufgabe leeren (optional)
            txtTopic.Clear();
            txtTaskType.Clear();
            txtDifficulty.Clear();
            txtPoints.Clear();
            txtTaskName.Clear();
            txtTaskContent.Clear();
            txtAuthor.Clear();
            dpDateCreated.SelectedDate = null;
        }

        // Methode zum Hinzufügen einer Person zur Datenbank
        private void AddTaskToDatabase(string topic, string taskType, string difficulty, int points, string taskName, string taskContent, DateTime dateCreated, string author)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
            {
                connection.Open();
                string insertQuery = @"
            INSERT INTO aufgabe (topic, type, difficulty, points, name, content, date_created, author) 
            VALUES (@topic, @type, @difficulty, @points, @name, @content, @date_created, @author)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
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

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            string taskName = deleteButton.Tag as string; // Erwartet einen string als Tag-Wert

            if (!string.IsNullOrEmpty(taskName))
            {
                DeleteTaskFromDatabase(taskName);

                // Aktualisiere die Anzeige im DataGrid
                LoadDataFromDatabase();
            }
        }

        private void DeleteTaskFromDatabase(string taskName)
        {
            using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
            {
                connection.Open();

                string deleteQuery = "DELETE FROM aufgabe WHERE name = @name";

                using (SQLiteCommand command = new SQLiteCommand(deleteQuery, connection))
                {
                    // Setze den Parameter der SQL-Abfrage
                    command.Parameters.AddWithValue("@name", taskName);

                    // Führe die Löschabfrage aus
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Aufgabe erfolgreich gelöscht!");
                    }
                    else
                    {
                        MessageBox.Show("Keine Aufgabe gefunden mit diesem Namen.");
                    }
                }
            }
        }
    }
}
