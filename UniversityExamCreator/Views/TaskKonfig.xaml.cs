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
    /// Interaktionslogik für TaskKonfig.xaml
    /// </summary>
    public partial class TaskKonfig : Page
    {
        public TaskKonfig()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            NextPath();
        }
        /// <summary>
        /// NextPath entscheidet auf welche WPF-Seite der User als nächstes kommt, basierend auf den Eingaben auf der aktuellen Seite.
        /// </summary>
        private void NextPath()
        {
            if (MC.IsChecked == true)
            {
                NavigationService.Navigate(new TaskCreateMC());
            }
            else
            {
                NavigationService.Navigate(new TaskCreateOF());
            }
        }
    }
}
