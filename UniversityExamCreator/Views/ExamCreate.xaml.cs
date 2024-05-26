using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ExamCreate.xaml
    /// </summary>
    public partial class ExamCreate : Page
    {
        // ObservableCollection to hold the items
        public ObservableCollection<Item> Items { get; set; }

        public ExamCreate()
        {
            InitializeComponent();

            // Initialize the ObservableCollection and add items
            Items = new ObservableCollection<Item>
            {
                new Item { Name = "Item is cool but does it have much place to exist1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
                new Item { Name = "Item 1", IsSelected = false, Info = "Information about Item 1" },
                new Item { Name = "Item 2", IsSelected = false, Info = "Information about Item 2" },
                new Item { Name = "Item 3", IsSelected = false, Info = "Information about Item 3" },
            };

            // Set the DataContext to the current instance
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamPreview());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ExamConfig());
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the item from the Tag property
            var button = sender as Button;
            if (button != null && button.Tag is Item item)
            {
                MessageBox.Show(item.Info, "Item Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        // Item class
        public class Item
        {
            public string Name { get; set; }
            public bool IsSelected { get; set; }
            public string Info { get; set; }
        }
    }
}
