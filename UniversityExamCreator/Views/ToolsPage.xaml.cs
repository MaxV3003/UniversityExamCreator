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
            NavigationService.Navigate(new ExamConfig());
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
    }
}
