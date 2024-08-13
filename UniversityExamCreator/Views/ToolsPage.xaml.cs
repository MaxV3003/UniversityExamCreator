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

        private void KlausurErstellen_Click(object sender, RoutedEventArgs e)
        {
            Examconfig examconfig = new Examconfig();
            NavigationService.Navigate(new ExamConfig(examconfig));
        }

        private void AufgabeErstellen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        private void AufgabeLöschen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskDelete());
        }

        private void ModulErstellen_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ModulCreate());
        }

        private void CommingSoon2_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
