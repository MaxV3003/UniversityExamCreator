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
            if (MCDD.Text == "Multiple Choice")
            {
                NavigationService.Navigate(new TaskCreateMC());
            }
            else
            {
                NavigationService.Navigate(new TaskCreateOF());
            }
        }

        private string CheckEerythingFilled()
        {
            return "";
        }
        
        private void AddToList (List contentList,string name, string content)
        {

        }

        /// <summary>
        /// greift aud den eigegebenen Text zu.
        /// </summary>
        private void Text()
        {
            string a = ThemeText.Text;
            MessageBox.Show(a);
        }
        /// <summary>
        /// ZUm testen neuer Funktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            Text();
        }

        /// <summary>
        /// Die Felder MCCCountDD und MCRulesText werden erst benutzbar gemacht, wenn Multiple Choice als Art der Klausur ausgewählt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MCDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MCDD.Text == "Offene Frage")
            {
                MCCountDD.IsEnabled = true;
                MCRulesText.IsEnabled = true;
            }
            else
            {
                MCCountDD.IsEnabled = false;
                MCRulesText.IsEnabled = false;
            }
        }
    }
}
