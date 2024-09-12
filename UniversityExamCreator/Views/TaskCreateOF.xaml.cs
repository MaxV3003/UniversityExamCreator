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
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für TaskCreateOF.xaml
    /// </summary>
    public partial class TaskCreateOF : Page
    {
        Models.Task task;
        public TaskCreateOF(Models.Task task)
        {
            InitializeComponent();
              this.task = task;
            
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }
        private void Create_Click(object sender, RoutedEventArgs e)
        {
            AddContent(task);//returnt Task muss dann noch in die DB

            if (CheckFilled() == true)
            {
                
                if (0 == 0)//Für DB abfrage ob Task gespeichert
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


        }/// <summary>
        /// Fügt den COntent und die Answer zur Task hinzu. 
        /// </summary>
        private Models.Task AddContent(Models.Task task)
        {
            task.setTaskContent(ContentText.Text);
            task.setTaskAnswer(AnswerText.Text);
            return task;

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
