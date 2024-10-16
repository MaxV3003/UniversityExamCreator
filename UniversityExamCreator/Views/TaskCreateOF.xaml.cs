using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using UniversityExamCreator.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für TaskCreateOF.xaml
    /// </summary>
    public partial class TaskCreateOF : Page
    {
        Models.Task task;

        public TaskCreateOF(Models.Task task)
        {
            InitializeComponent();
            this.task = task;
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            // Hole den Text aus der TextBox, die den Benutzernamen enthält
            string username = username_text.Text;

            // Wenn der username leer ist, setze ihn auf einen leeren String
            if (string.IsNullOrEmpty(username))
            {
                username = ""; // Fallback für leeren Benutzernamen
            }

            // Restlicher Code für das Erstellen der Aufgabe und das Speichern in der Datenbank
            task = AddContent(task);

            if (CheckFilled() == true)
            {
                if (string.IsNullOrEmpty(task.Author))
                {
                    task.Author = "Unbekannter Autor";
                }

                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();
                string connectionString = $"Data Source={databasePath};Version=3;";
                DataService dataService = new DataService(connectionString);

                try
                {
                    // Speichere die Aufgabe und erhalte die generierte ID
                    task.Id = dataService.InsertTask(
                        topic: task.Topic,
                        taskType: task.TaskType,
                        difficulty: task.Difficulty,
                        points: task.Points,
                        taskName: task.TaskName,
                        taskContent: task.getTaskContent(),
                        dateCreated: DateTime.Now,
                        author: task.Author
                    );

                    // Speichere die Antwort in der Datenbank
                    dataService.InsertAnswer(task.Id, AnswerText.Text, username);

                    MessageBox.Show("Aufgabe wurde erfolgreich gespeichert.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Aufgabe konnte nicht gespeichert werden: " + ex.Message);
                }


                NavigationService.Navigate(new ToolsPage());
            }
        }


                private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        /// <summary>
        /// Fügt den Content und die Antwort zur Aufgabe hinzu.
        /// </summary>
        private Models.Task AddContent(Models.Task task)
        {
            // Setze den Inhalt und die Antwort in die Task-Instanz
            task.setTaskContent(ContentText.Text);
            task.setTaskAnswer(AnswerText.Text);
            return task;
        }

        /// <summary>
        /// Überprüft, ob alle Pflichtfelder ausgefüllt wurden.
        /// </summary>
        /// <returns></returns>
        private Boolean CheckFilled()
        {
            if (string.IsNullOrEmpty(ContentText.Text))
            {
                ContentText.BorderBrush = Brushes.Red;
                ContentText.BorderThickness = new Thickness(2);
                System.Windows.MessageBox.Show("Bitte einen Fragetext angeben");
                return false;
            }
            else if (string.IsNullOrEmpty(AnswerText.Text))
            {
                ContentText.BorderBrush = Brushes.Red;
                ContentText.BorderThickness = new Thickness(2);
                System.Windows.MessageBox.Show("Bitte einen Antworttext angeben");
                return false;
            }
            return true;
        }

        private void AnswerText_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Hier kannst du logische Dinge hinzufügen, falls sich die Antwort ändert
        }

        private void ContentText_TextChanged(object sender, TextChangedEventArgs e)
        {
        }
    }
}




