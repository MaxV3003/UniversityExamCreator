using System;
using System.Data.SQLite; // Stelle sicher, dass du die SQLite-Bibliothek importiert hast
using System.Windows;
using System.Windows.Controls;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ModulCreate.xaml
    /// </summary>
    public partial class ModulCreate : Page
    {
        public ModulCreate()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string moduleIDText = ModulIDText.Text;
            string moduleName = ModulNameText.Text;  

            if (string.IsNullOrWhiteSpace(moduleIDText) || string.IsNullOrWhiteSpace(moduleName))
            {
                MessageBox.Show("Bitte geben Sie sowohl eine Modul-ID als auch einen Modulnamen ein.");
                return;
            }

            // Überprüfe, ob das Modul bereits existiert
            if (CheckModuleExists(moduleIDText))
            {
                MessageBox.Show("Modul bereits vorhanden.");
                return;
            }

            // Füge das Modul zur Datenbank hinzu
            InsertModuleIntoDatabase(moduleIDText, moduleName);
        }

        private void InsertModuleIntoDatabase(string moduleID, string moduleName)
        {
            try
            {
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO module (moduleID, name) VALUES (@moduleID, @name)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@moduleID", moduleID);
                        command.Parameters.AddWithValue("@name", moduleName);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Modul erfolgreich erstellt.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Erstellen des Moduls: " + ex.Message);
            }
        }

        private bool CheckModuleExists(string moduleID)
        {
            try
            {
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();
                    string checkQuery = "SELECT COUNT(*) FROM module WHERE moduleID = @moduleID";

                    using (var command = new SQLiteCommand(checkQuery, connection))
                    {
                        command.Parameters.AddWithValue("@moduleID", moduleID);
                        long count = (long)command.ExecuteScalar();
                        return count > 0; // Gibt true zurück, wenn das Modul bereits existiert
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Überprüfung: " + ex.Message);
                return false;
            }
        }
    }
}



