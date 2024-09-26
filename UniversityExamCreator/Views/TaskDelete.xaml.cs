using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;
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

        //Lists to save the the Filteropotion-Items 
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
            // Add default option to Themes
            Themes.Insert(0, "Alle Themen");
            // Set default selected theme
            SelectedTheme = Themes[0];

            // Initialize the points list and populate it with distinct points
            Points = Items.Select(i => i.Points).Distinct().ToList();
            // Add default option to Points
            Points.Insert(0, 0);
            // Set default selected points
            SelectedPoints = Points[0];

            Difficulties = Items.Select(i => i.Difficulty).Distinct().ToList();
            Difficulties.Insert(0, "Alle Schwierigkeiten");
            SelectedDifficulty = Difficulties[0];

            // Set the DataContext to the current instance
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
                MessageBox.Show(item.Info, "Item Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        /// <summary>
        /// Dropdown-Changes will be initialized after selecting a new Filteroption. 
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Filter the items based on the selected theme and points
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
        /// Button to delete the Items from the List of the Items which sould be in the Exam.
        /// </summary>
        private void DeleteSelectedItemsButton_Click(object sender, RoutedEventArgs e)
        {
            var itemsToRemove = SelectedItems.Where(i => i.IsSelectedForDeletion).ToList();
            foreach (var item in itemsToRemove)
            {
                SelectedItems.Remove(item);
            }
        }


        // Item class
        public class Item
        {
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
                // Pfad zur SQLite-Datenbankdatei ermitteln
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                // Überprüfen, ob die Datenbankdatei existiert
                if (!File.Exists(databasePath))
                {
                    throw new FileNotFoundException("Datenbankdatei nicht gefunden: " + databasePath);
                }

                // Verbindung zur SQLite-Datenbank öffnen
                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    // SQL-Abfrage zum Abrufen der Aufgaben aus der Tabelle "task"
                    string query = "SELECT topic, type, difficulty, points, name, content FROM task";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            // Überprüfen, ob Einträge vorhanden sind
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Erstelle ein neues Item-Objekt für jede geladene Aufgabe
                                    var taskItem = new Item
                                    {
                                        Name = reader["name"].ToString(),
                                        Info = reader["content"].ToString(),
                                        Points = Convert.ToInt32(reader["points"]),
                                        Theme = reader["topic"].ToString(),
                                        Difficulty = reader["difficulty"].ToString(),
                                        IsSelected = false
                                    };

                                    // Füge die Aufgabe der ObservableCollection hinzu
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

                // Filter anwenden, um die Daten in der Observer-Box anzuzeigen
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
    }
}
