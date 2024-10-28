using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ToolsPage.xaml
    /// </summary>
    public partial class ToolsPage : Page
    {
        public ToolsPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Navigate to Examconfig.
        /// </summary>
        private void KlausurErstellen_Click(object sender, RoutedEventArgs e)
        {
            Examconfig examconfig = new Examconfig();
            NavigationService.Navigate(new ExamConfig(examconfig));
        }

        /// <summary>
        /// Navigate to Taskkonfig.
        /// </summary>
        private void AufgabeErstellen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        /// <summary>
        /// Navigate to TaskDelete.
        /// </summary>
        private void AufgabeLöschen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskDelete());
        }

        /// <summary>
        /// Navigate to Modulecreate.
        /// </summary>
        private void ModulErstellen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ModulCreate());
        }

        /// <summary>
        /// Navigate to DBTest.
        /// </summary>
        private void DBTest_Click(object sender, RoutedEventArgs e)
        {
            PathFinder pathFinder = new PathFinder("Databases", "database.db");
            string path = "Data Source=" + pathFinder.GetPath() + ";Version=3;";
            NavigationService.Navigate(new DBTest(path));
        }
    }
}
