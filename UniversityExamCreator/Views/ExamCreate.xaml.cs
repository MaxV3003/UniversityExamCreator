using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UniversityExamCreator.Models;
using UniversityExamCreator.Services;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ExamCreate.xaml
    /// </summary>
    public partial class ExamCreate : Page
    {
        // ObservableCollection to hold the tasks
        public ObservableCollection<Task> Tasks { get; set; }
        public ObservableCollection<Task> SelectedTasks { get; set; }
        public ObservableCollection<Task> FilteredTasks { get; set; }

        // Lists to save the filter options
        public List<string> Themes { get; set; }
        public List<int> Points { get; set; }
        public List<string> Difficulties { get; set; }
        public string SelectedTheme { get; set; }
        public int SelectedPoints { get; set; }
        public string SelectedDifficulty { get; set; }

        //Examconfig
        Examconfig Examconfig { get; set; }
        
        internal ExamCreate(Examconfig examconfig)
        {
            Examconfig = examconfig;
            InitializeComponent();

            Task task1 = new Task("Module1", "Test", "Typ1", "leicht", 2, "Task 1");
            task1.setTaskContent("Test for Button");

            // Initialize the ObservableCollection and add tasks
            Tasks = new ObservableCollection<Task>
            {
                task1,
                new Task("Module1", "Datenbanken", "Typ2", "leicht", 2, "Task 2"),
                new Task("Module2", "Datenbanken", "Typ3", "schwer", 3, "Task 3"),
                new Task("Module3", "Programmieren", "Typ4", "normal", 3, "Task 4"),
                new Task("Module4", "Programmieren", "Typ5", "schwer", 1, "Task 5"),
                // Add more tasks as needed
            };

            // Initialize the filtered tasks collection
            FilteredTasks = new ObservableCollection<Task>();
            SelectedTasks = new ObservableCollection<Task>();

            // Initialize the themes list and populate it with distinct themes
            Themes = Tasks.Select(t => t.Topic).Distinct().ToList();
            Themes.Insert(0, "Alle Themen");
            SelectedTheme = Themes[0];

            // Initialize the points list and populate it with distinct points
            Points = Tasks.Select(t => t.Points).Distinct().ToList();
            Points.Insert(0, 0);
            SelectedPoints = Points[0];

            Difficulties = Tasks.Select(t => t.Difficulty).Distinct().ToList();
            Difficulties.Insert(0, "Alle Schwierigkeiten");
            SelectedDifficulty = Difficulties[0];

            // Set the DataContext to the current instance
            DataContext = this;

            // Apply initial filters
            ApplyFilters();
        }

        /// <summary>
        /// Button to get to the next Page.
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List <Task> tasks = new List<Task>();

            //muss hier noch in die ExamConfig geschrieben werden
            foreach (var task in SelectedTasks) {
                tasks.Add(task);
            }
            NavigationService.Navigate(new ExamPreview(Examconfig, tasks));
        }

        /// <summary>
        /// Button to get to the previous Page.
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamConfig(Examconfig));
            (NavigationService.Content as ExamConfig)?.ConfigLoader(Examconfig);
        }

        /// <summary>
        /// Button to get the Info of a Task.
        /// </summary>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null && button.Tag is Task task)
            {
                MessageBox.Show(task.TaskContent, "Task Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Dropdown-Changes will be initialized after selecting a new filter option. 
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Filter the tasks based on the selected theme, points, and difficulty
            ApplyFilters();
        }

        /// <summary>
        /// Methode to swap the Tasks in the left Box.
        /// </summary>
        private void ApplyFilters()
        {
            // Apply theme filter
            var filteredByTheme = Tasks.Where(t => SelectedTheme == null || SelectedTheme == "Alle Themen" || t.Topic == SelectedTheme);

            // Apply points filter
            var filteredByPoints = SelectedPoints == 0 ? filteredByTheme : filteredByTheme.Where(t => t.Points == SelectedPoints);

            // Apply difficulty filter
            var filteredByDifficulty = SelectedDifficulty == null || SelectedDifficulty == "Alle Schwierigkeiten"
                ? filteredByPoints
                : filteredByPoints.Where(t => t.Difficulty == SelectedDifficulty);

            // Update FilteredTasks
            FilteredTasks.Clear();
            foreach (var task in filteredByDifficulty)
            {
                FilteredTasks.Add(task);
            }
        }

        /// <summary>
        /// Button to add the Tasks that should be in the Exam.
        /// </summary>
        private void AddSelectedTasksButton_Click(object sender, RoutedEventArgs e)
        {
            var tasksToAdd = Tasks.Where(t => t.IsSelected).ToList();
            foreach (var task in tasksToAdd)
            {
                if (!SelectedTasks.Contains(task))
                {
                    SelectedTasks.Add(task);
                }
                task.IsSelected = false; // Reset the IsSelected property
            }
            UpdateSelectedTasksPoints();
        }

        /// <summary>
        /// Button to delete the Tasks from the List of the Tasks which should be in the Exam.
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
        /// Methode to Update the Pointscounter under the selected Task-Box.
        /// </summary>
        private void UpdateSelectedTasksPoints()
        {
            int totalPoints = SelectedTasks.Sum(task => task.Points);
            TotalPointsTextBlock.Text = $"Total Points: {totalPoints}";
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

