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

namespace UniversityExamCreator.Views
{
    public partial class ExamCreate : Page
    {
        // Opperating Itemlists for task-Items
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<Task> SelectedTasks { get; set; }
        public ObservableCollection<Task> FilteredTasks { get; set; }

        // Operating Itemlists for Exam-/Task-Attributes 
        public List<string> Themes { get; set; }
        public List<int> Points { get; set; }
        public List<string> Difficulties { get; set; }

        // Selected Value for Combobox
        public string SelectedTheme { get; set; }
        public int SelectedPoints { get; set; }
        public string SelectedDifficulty { get; set; }

        // Examconfig-Item
        Examconfig Examconfig { get; set; }

        // Examconfig-Attributes
        public string Module;
        public string TaskType;

        internal ExamCreate(Examconfig examconfig)
        {
            Examconfig = examconfig;
            InitializeComponent();

            Tasks = new ObservableCollection<Task>();
            SelectedTasks = new ObservableCollection<Task>();
            FilteredTasks = new ObservableCollection<Task>();

            // Retrieve the specific task items from the database.
            Module = examconfig.ModuleID;
            TaskType = examconfig.ExamType;

            LoadTasksFromDatabase();

            // Add items to comboboxes.
            Themes = Tasks.Select(t => t.Topic).Distinct().ToList();
            Themes.Insert(0, "Alle Themen");
            SelectedTheme = Themes[0];

            Points = Tasks.Select(t => t.Points).Distinct().ToList();
            Points.Insert(0, 0);
            SelectedPoints = Points[0];

            Difficulties = Tasks.Select(t => t.Difficulty).Distinct().ToList();
            Difficulties.Insert(0, "Alle Schwierigkeiten");
            SelectedDifficulty = Difficulties[0];

            // If no pointamount was selected. 
            if (Examconfig.PointAmount.Equals(null))
            {
                Examconfig.PointAmount = 0;
            }

            DataContext = this;
            ApplyFilters();
        }

        /// <summary>
        /// Load the Dataitems based on the configurations of the last page.
        /// </summary>
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

                // Tasks which match the selected module and tasktype.
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    string query = "SELECT id, topic, type, difficulty, points, name, content FROM task WHERE module=";
                    query += "'" + Module + "'";

                    if (TaskType != "Mischform")
                    {
                        query += " AND type = '" + TaskType + "'";
                    }

                    // Create the Task-items. 
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
                                    taskName: reader["name"].ToString(),
                                    content: reader["content"].ToString(),
                                    taskID: Convert.ToInt32(reader["id"])
                                );
                                try
                                {
                                    // Retrieve the Answer for the task and add it to the Task-item.
                                    string query2 = "SELECT * FROM answer WHERE task_id = @task_id";
                                    using (var command2 = new SQLiteCommand(query2, connection))
                                    {
                                        command2.Parameters.AddWithValue("@task_id", task.Id);
                                        using (var reader2 = command2.ExecuteReader())
                                        {
                                            while (reader2.Read())
                                            {
                                                task.TaskAnswer.Content = reader2["answer_content"].ToString();
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex) 
                                {
                                    //
                                }

                                // Retrieve the MC-Answer for the task and add it to the Task-item.
                                if (task.TaskType == "Multiple Choice")
                                {
                                    string query3 = "SELECT * FROM mcanswer WHERE task_id = @task_id";
                                    using (var command3 = new SQLiteCommand(query3, connection))
                                    {
                                        command3.Parameters.AddWithValue("@task_id", task.Id);

                                        using (var reader3 = command3.ExecuteReader())
                                        {
                                            while (reader3.Read())
                                            {
                                                var mcAnswer = new MCAnswer(
                                                    content: reader3["content"].ToString(),
                                                    identifire: Convert.ToInt32(reader3["id"]),
                                                    answerFlag: Convert.ToInt32(reader3["is_correct"])
                                                );
                                                task.MCAnswers.Add(mcAnswer);
                                            }
                                        }
                                    }
                                }
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

        /// <summary>
        /// Apply the filters from the dropdown menus. Only tasks that match the filters will be displayed. 
        /// </summary>
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

        /// <summary>
        /// Button to add tasks to the exam. 
        /// </summary>
        private void AddSelectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasksToAdd = Tasks.Where(t => t.IsSelected).ToList();
            double totalPoints = SelectedTasks.Sum(task => task.Points);
            totalPoints += tasksToAdd.Sum(task => task.Points);
            int selectedTaskAmount = SelectedTasks.Count + tasksToAdd.Count;

            if (selectedTaskAmount <= Examconfig.TaskAmount)
            {
                if (totalPoints <= Examconfig.PointAmount || Examconfig.PointAmount == 0)
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

        /// <summary>
        /// Button to delete selected items for the exam. 
        /// </summary>
        private void DeleteSelectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasksToRemove = SelectedTasks.Where(t => t.IsSelectedForDeletion).ToList();
            foreach (var task in tasksToRemove)
            {
                SelectedTasks.Remove(task);
            }
            UpdateSelectedTasksPoints();
        }

        /// <summary>
        /// Change the Pointcounter in the UI.
        /// </summary>
        private void UpdateSelectedTasksPoints()
        {
            int totalPoints = SelectedTasks.Sum(task => task.Points);
            TotalPointsTextBlock.Text = $"Total Points: {totalPoints}";
        }

        private void SaveSelectedTasksToList()
        {
            ApplyFilters();
        }

        /// <summary>
        /// Get the Taskcontent as Information for the user. 
        /// </summary>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            Button infoButton = sender as Button;
            Task selectedTask = infoButton?.Tag as Task;
            
            //Console.WriteLine("selecetedTask: " + selectedTask.TaskContent);
            if (selectedTask != null)
            {
                MessageBox.Show($"{selectedTask.TaskContent}", selectedTask.TaskName);
            }
        }
        
        /// <summary>
        /// Navigate to the next page.
        /// </summary>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSelectedTasksToList(); 

            if (this.NavigationService != null)
            {
                this.NavigationService.Navigate(new ExamPreview(Examconfig, Tasks.ToList(),  SelectedTasks.ToList()));
            }
            else
            {
                MessageBox.Show("NavigationService ist nicht verfügbar.");
            }
        }

        /// <summary>
        /// Navigate to the previous page.
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamConfig(Examconfig));
        }

        /// <summary>
        /// Change-Event to change the combobox-itmes. 
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        /// <summary>
        /// Function to change behaviours of the itemlist based on the configuration.
        /// </summary>
        private void SelectedItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// Function to change behaviours of the itemlist which contains the selected exam-tasks.
        /// </summary>
        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}







