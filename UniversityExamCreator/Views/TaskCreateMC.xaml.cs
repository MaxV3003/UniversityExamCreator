using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
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
        private int textBoxCount = 0;
        private double currentY = 10;
        public TaskCreateMC(Models.Task task)
        {
            InitializeComponent();
            this.task = task;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.TextBox newTextBox = new System.Windows.Controls.TextBox();
            newTextBox.Width = 490; 
            newTextBox.Height = 60;
            newTextBox.Margin = new Thickness(0, 10, 0, 0); 
            Canvas.SetLeft(newTextBox, 100); 
            Canvas.SetTop(newTextBox, currentY); 

            currentY += 20; 

            TextBoxContainer.Children.Add(newTextBox);

            textBoxCount++;
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
