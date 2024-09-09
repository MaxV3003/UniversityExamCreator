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
    /// Interaktionslogik für TaskCreateOF.xaml
    /// </summary>
    public partial class TaskCreateOF : Page
    {
        public TaskCreateOF()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            PreparedContent(); //Der Content der Aufgabe. Bekommt String Array wieder mit Index 0 = Content und Index 1 = Answer.

            if (CheckFilled() == true)
            {
                int n = 0;
                if (n == 0)
                {
                    MessageBox.Show("Aufgabe wurde gespeichert.");
                }
                else
                {
                    MessageBox.Show("Aufgabe konnte nicht gespeichert werden. Bitte Angaben überprüfen.");
                }
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine Fragestellung ein.");

            }
            NavigationService.Navigate(new ToolsPage());


        }
        /// <summary>
        /// Soll den eingegebenen Content in ein String Array packen.
        /// </summary>
        /// <returns></returns>
        private string[] PreparedContent()
        {
            string[] content = new string[2];

            content[0] = ContentText.Text;
            content[1] = AnswerText.Text;
            return content;
        }
        /// <summary>
        /// Checkt ob alle Pflichfelder gefüllt worden sind.
        /// </summary>
        /// <returns></returns>
        private Boolean CheckFilled()
        {
            if (ContentText.Text !=  "")
            {
                return true;
            }
            else
            {
                ContentText.BorderBrush = Brushes.Red;
                ContentText.BorderThickness = new Thickness(2);
                return false;
            }
        }
    }
}
