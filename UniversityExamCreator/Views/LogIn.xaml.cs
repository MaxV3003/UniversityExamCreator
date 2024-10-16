using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für LogIn.xaml
    /// </summary>
    public partial class LogIn : Page
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Hole die Eingaben des Benutzers
            string benutzername = ExamTitle_Kopieren.Text;
            string passwort = ExamTitle.Text;

            if (string.IsNullOrWhiteSpace(benutzername))
            {
                MessageBox.Show("Bitte geben Sie einen Benutzernamen ein!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(passwort))
            {
                MessageBox.Show("Bitte ein Passwort eingeben!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    // Überprüfe, ob der Benutzername bereits existiert
                    string checkQuery = "SELECT password FROM user WHERE username = @Benutzername";
                    using (var checkCommand = new SQLiteCommand(checkQuery, connection))
                    {
                        checkCommand.Parameters.AddWithValue("@Benutzername", benutzername);
                        var existingPassword = checkCommand.ExecuteScalar()?.ToString();

                        if (existingPassword != null)
                        {
                            // Benutzername existiert, prüfe das Passwort
                            if (existingPassword == passwort)
                            {
                                MessageBox.Show("Erfolgreich eingeloggt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                                NavigationService.Navigate(new ToolsPage());
                            }
                            else
                            {
                                MessageBox.Show("Falsches Passwort. Bitte erneut versuchen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            return;
                        }
                    }

                    // Wenn der Benutzername nicht existiert, neuen Benutzer erstellen
                    string connectionString = $"Data Source={databasePath};Version=3;";
                    DataService dataService = new DataService(connectionString);
                    dataService.InsertUser(benutzername, passwort);

                    MessageBox.Show("Benutzer erfolgreich erstellt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService.Navigate(new ToolsPage());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Datenbankverbindung: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}




