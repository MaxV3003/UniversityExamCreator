using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UniversityExamCreator.Models;
using UniversityExamCreator.Services;

namespace UniversityExamCreator.Views
{
    public partial class ExamCreate : Page
    {
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<Task> SelectedTasks { get; set; }
        public ObservableCollection<Task> FilteredTasks { get; set; }

        public List<string> Themes { get; set; }
        public List<int> Points { get; set; }
        public List<string> Difficulties { get; set; }
        public string SelectedTheme { get; set; }
        public int SelectedPoints { get; set; }
        public string SelectedDifficulty { get; set; }

        Examconfig Examconfig { get; set; }

        internal ExamCreate(Examconfig examconfig)
        {
            Examconfig = examconfig;
            InitializeComponent();

            Tasks = new ObservableCollection<Task>();
            SelectedTasks = new ObservableCollection<Task>();
            FilteredTasks = new ObservableCollection<Task>();

            LoadTasksFromDatabase();

            Themes = Tasks.Select(t => t.Topic).Distinct().ToList();
            Themes.Insert(0, "Alle Themen");
            SelectedTheme = Themes[0];

            Points = Tasks.Select(t => t.Points).Distinct().ToList();
            Points.Insert(0, 0);
            SelectedPoints = Points[0];

            Difficulties = Tasks.Select(t => t.Difficulty).Distinct().ToList();
            Difficulties.Insert(0, "Alle Schwierigkeiten");
            SelectedDifficulty = Difficulties[0];

            DataContext = this;

            ApplyFilters();
        }

        private void LoadTasksFromDatabase()
        {
            try
            {
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                if (!File.Exists(databasePath))
                {
                    throw new FileNotFoundException("Datenbankdatei nicht gefunden: " + databasePath);
                }

                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    string query = "SELECT topic, type, difficulty, points, name, content FROM task";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var task = new Task(
                                    module: "ModulePlaceholder",
                                    topic: reader["topic"].ToString(),
                                    taskType: reader["type"].ToString(),
                                    difficulty: reader["difficulty"].ToString(),
                                    points: Convert.ToInt32(reader["points"]),
                                    taskName: reader["name"].ToString()
                                );
                                task.setTaskContent(reader["content"].ToString());
                                Tasks.Add(task);
                            }
                        }
                    }
                }
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Tasks: " + ex.Message);
            }
        }

        private void ApplyFilters()
        {
            var filteredByTheme = Tasks.Where(t => SelectedTheme == null || SelectedTheme == "Alle Themen" || t.Topic == SelectedTheme);
            var filteredByPoints = SelectedPoints == 0 ? filteredByTheme : filteredByTheme.Where(t => t.Points == SelectedPoints);
            var filteredByDifficulty = SelectedDifficulty == null || SelectedDifficulty == "Alle Schwierigkeiten"
                ? filteredByPoints
                : filteredByPoints.Where(t => t.Difficulty == SelectedDifficulty);

            FilteredTasks.Clear();
            foreach (var task in filteredByDifficulty)
            {
                FilteredTasks.Add(task);
            }
        }

        private void AddSelectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            // neue Tasks die geaddet werden sollen
            var tasksToAdd = Tasks.Where(t => t.IsSelected).ToList();

            // Punkte aus den schon Seleceteten Tasks
            double totalPoints = SelectedTasks.Sum(task => task.Points);

            // Punkte aus den Tasks die hinzukommen sollen
            totalPoints += tasksToAdd.Sum(task => task.Points);
            int selecetedTaskAmoount = 0;
            
            foreach (var task in tasksToAdd)
            {
                selecetedTaskAmoount++;
            }

            foreach (var task in SelectedTasks)
            {
                selecetedTaskAmoount++;
            }

            if (!(selecetedTaskAmoount > Examconfig.TaskAmount))
            {
                if (!(totalPoints > Examconfig.PointAmount))
                {
                    foreach (var task in tasksToAdd)
                    {
                        if (!SelectedTasks.Contains(task))
                        {
                            SelectedTasks.Add(task);
                        }
                        task.IsSelected = false;
                    }
                    UpdateSelectedTasksPoints();
                }
                else
                {
                    MessageBox.Show("Die ausgewählten Aufgaben überschreiten das Punktelimit!");
                }
            }
            else
            {
                MessageBox.Show("Die maximale Anzahl an Aufgaben wurde überschritten!");
            }
        }

        private void DeleteSelectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasksToRemove = SelectedTasks.Where(t => t.IsSelectedForDeletion).ToList();
            foreach (var task in tasksToRemove)
            {
                SelectedTasks.Remove(task);
            }
            UpdateSelectedTasksPoints();
        }

        private void UpdateSelectedTasksPoints()
        {
            int totalPoints = SelectedTasks.Sum(task => task.Points);
            TotalPointsTextBlock.Text = $"Total Points: {totalPoints}";
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();  // ComboBox-Filter aktualisieren die angezeigten Aufgaben
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Button infoButton = sender as Button;
            Task selectedTask = infoButton?.Tag as Task;

            if (selectedTask != null)
            {
                MessageBox.Show($"Task Info:\nName: {selectedTask.TaskName}\nContent: {selectedTask.TaskContent}", "Task Info");
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.NavigationService != null)
            {
                // Stelle sicher, dass du die ausgewählten Aufgaben übergibst
                this.NavigationService.Navigate(new ExamPreview(Examconfig, SelectedTasks.ToList()));
            }
            else
            {
                MessageBox.Show("NavigationService ist nicht verfügbar.");
            }
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Auswahländerungen können hier bearbeitet werden, wenn nötig
        }
    }
}






