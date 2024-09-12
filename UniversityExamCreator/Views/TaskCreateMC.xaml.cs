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
    /// Interaktionslogik für TaskCreateMC.xaml
    /// </summary>
    public partial class TaskCreateMC : Page
    {
        Models.Task task;
        public TaskCreateMC(Models.Task task)
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
            NavigationService.Navigate(new ToolsPage());
        }
    }
}
