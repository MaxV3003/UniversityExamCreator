using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using UniversityExamCreator.Models;
using static UniversityExamCreator.Views.ExamCreate;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für TaskDelete.xaml
    /// </summary>
    public partial class TaskDelete : Page
    {
        // ObservableCollection to hold the items
        public ObservableCollection<Item> Items { get; set; }
        public ObservableCollection<Item> SelectedItems { get; set; }
        public ObservableCollection<Item> FilteredItems { get; set; }

        // Lists to save the Filteroption-Items 
        public List<string> Themes { get; set; }
        public List<int> Points { get; set; }
        public List<string> Difficulties { get; set; }
        public string SelectedTheme { get; set; }
        public int SelectedPoints { get; set; }
        public string SelectedDifficulty { get; set; }

        public TaskDelete()
        {
            InitializeComponent();

            // Initialize the ObservableCollection and add items
            Items = new ObservableCollection<Item>();

            // Initialize the filtered items collection
            FilteredItems = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();

            LoadTasksFromDatabase();

            // Initialize the themes list and populate it with distinct themes
            Themes = Items.Select(i => i.Theme).Distinct().ToList();
            Themes.Insert(0, "Alle Themen");
            SelectedTheme = Themes[0];

            // Initialize the points list and populate it with distinct points
            Points = Items.Select(i => i.Points).Distinct().ToList();
            Points.Insert(0, 0);
            SelectedPoints = Points[0];

            // Initialize the difficulties list and populate it with distinct difficulties
            Difficulties = Items.Select(i => i.Difficulty).Distinct().ToList();
            Difficulties.Insert(0, "Alle Schwierigkeiten");
            SelectedDifficulty = Difficulties[0];

            DataContext = this;

            // Apply initial filters
            ApplyFilters();
        }

        /// <summary>
        /// Button to get the Info of an Item.
        /// </summary>
        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the item from the Tag property
            var button = sender as Button;
            if (button != null && button.Tag is Item item)
            {
                MessageBox.Show(item.Info, "Aufgabeninformation", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Dropdown-Changes will be initialized after selecting a new Filteroption.
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Filter the items based on the selected theme, points, and difficulty
            ApplyFilters();
        }

        /// <summary>
        /// Methode to swap the Items in the left Box.
        /// </summary>
        private void ApplyFilters()
        {
            // Apply theme filter
            var filteredByTheme = Items.Where(i => SelectedTheme == null || SelectedTheme == "Alle Themen" || i.Theme == SelectedTheme);

            // Apply points filter
            var filteredByPoints = SelectedPoints == 0 ? filteredByTheme : filteredByTheme.Where(i => i.Points == SelectedPoints);

            // Apply difficulty filter
            var filteredByDifficulty = SelectedDifficulty == null || SelectedDifficulty == "Alle Schwierigkeiten"
                ? filteredByPoints
                : filteredByPoints.Where(i => i.Difficulty == SelectedDifficulty);

            // Update FilteredItems
            FilteredItems.Clear();
            foreach (var item in filteredByDifficulty)
            {
                FilteredItems.Add(item);
            }
        }

        /// <summary>
        /// Button to add the Items that should be in the Exam.
        /// </summary>
        private void AddSelectedItemsButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsToAdd = Items.Where(i => i.IsSelected).ToList();
            foreach (var item in itemsToAdd)
            {
                if (!SelectedItems.Contains(item))
                {
                    SelectedItems.Add(item);
                }
                item.IsSelected = false; // Reset the IsSelected property
            }
        }

        /// <summary>
        /// Button to delete the Items from the List of the Items which should be in the Exam.
        /// </summary>
        private void DeleteSelectedItemsButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = SelectedItems.Where(i => i.IsSelectedForDeletion).ToList();

            // open SQLite-Connecetion
            PathFinder pathFinder = new PathFinder("Databases", "database.db");
            string databasePath = pathFinder.GetPath();

            using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
            {
                connection.Open();

                foreach (var item in itemsToRemove)
                {
                    string query = "DELETE FROM task WHERE id = @id";

                    using (var command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", item.Id); 

                        command.ExecuteNonQuery();
                    }

                    SelectedItems.Remove(item);
                }
            }

            ApplyFilters();
            MessageBox.Show("Die ausgewählten Aufgaben wurden erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Item class
        public class Item
        {
            public int Id { get; set; }  
            public string Name { get; set; }
            public bool IsSelected { get; set; }
            public bool IsSelectedForDeletion { get; set; }
            public string Info { get; set; }
            public int Points { get; set; }
            public string Theme { get; set; }
            public string Difficulty { get; set; }
        }

        private void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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

                    string query = "SELECT id, topic, type, difficulty, points, name, content FROM task";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var taskItem = new Item
                                    {
                                        Id = Convert.ToInt32(reader["id"]),  
                                        Name = reader["name"].ToString(),
                                        Info = reader["content"].ToString(),
                                        Points = Convert.ToInt32(reader["points"]),
                                        Theme = reader["topic"].ToString(),
                                        Difficulty = reader["difficulty"].ToString(),
                                        IsSelected = false
                                    };

                                    Items.Add(taskItem);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Keine Aufgaben in der Datenbank gefunden.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                    }
                }
                ApplyFilters();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show("Fehler beim Laden der Aufgaben: " + ex.Message);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void SelectedItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
