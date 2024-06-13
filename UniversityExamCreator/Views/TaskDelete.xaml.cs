using System;
using System.Collections.Generic;
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
            Items = new ObservableCollection<Item>
            {
                new Item { Name = "Item is cool but does it have much place to exist1", IsSelected = false, Info = "Information about Item 1", Points=2, Theme ="Test", Difficulty="leicht"},
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2", Points = 2, Theme="Datenbanken", Difficulty="leicht"},
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3", Points = 3, Theme="Datenbanken", Difficulty="schwer"},
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1", Points = 3, Theme="Programmieren", Difficulty="normal"},
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2", Points = 1, Theme="Programmieren", Difficulty="schwer"},
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3", Points = 1, Theme="Lineare Algebra", Difficulty="schwer"},
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1", Points = 3, Theme="Lineare Algebra", Difficulty="normal"},
               };

            // Initialize the filtered items collection
            FilteredItems = new ObservableCollection<Item>();
            SelectedItems = new ObservableCollection<Item>();

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
            UpdateSelectedItemsPoints();

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
            UpdateSelectedItemsPoints();

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

    private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            //Selected Items aus der Datenbank löschen
            if (/*Check if deleted) == true*/0==0)
            {
                MessagegeBox.write("Aufgabe aus der Datenbank gelöscht");
            }
            else
            {
                MessagegeBox.write("Löschung konnte nicht ausgeführt werden");
            }

            NavigationService.Navigate(new ToolsPage());
        }
    }
}
