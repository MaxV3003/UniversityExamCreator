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

            // Überprüfung, ob das Benutzername-Feld leer ist
            if (string.IsNullOrWhiteSpace(benutzername))
            {
                MessageBox.Show("Bitte geben Sie einen Benutzernamen ein!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Abbrechen, wenn der Benutzername fehlt
            }

            // Überprüfung, ob das Passwort-Feld leer ist
            if (string.IsNullOrWhiteSpace(passwort))
            {
                MessageBox.Show("Bitte ein Passwort eingeben!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return; // Abbrechen, wenn das Passwort fehlt
            }

            // Datenbankverbindung einrichten
            try
            {
                // Verwende PathFinder, um den Pfad zur SQLite-Datenbank zu finden
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                // SQLite-Verbindung erstellen und öffnen
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
                                // Navigation zur nächsten Seite
                                NavigationService.Navigate(new ToolsPage());
                            }
                            else
                            {
                                MessageBox.Show("Falsches Passwort. Bitte erneut versuchen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                            return; // Beende den Prozess nach dem Login-Versuch
                        }
                    }

                    // Wenn der Benutzername nicht existiert, neuen Benutzer erstellen
                    string insertQuery = "INSERT INTO user (username, password) VALUES (@Benutzername, @Passwort)";
                    using (var insertCommand = new SQLiteCommand(insertQuery, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@Benutzername", benutzername);
                        insertCommand.Parameters.AddWithValue("@Passwort", passwort);

                        int result = insertCommand.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Benutzer erfolgreich erstellt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                            // Navigation zur nächsten Seite
                            NavigationService.Navigate(new ToolsPage());
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Erstellen des Benutzers.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler bei der Datenbankverbindung: " + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}




