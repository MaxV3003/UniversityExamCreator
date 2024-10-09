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
        private List<System.Windows.Controls.TextBox> textBoxList = new List<System.Windows.Controls.TextBox>();
        private List<(System.Windows.Controls.RadioButton, System.Windows.Controls.RadioButton)> radioButtonGroups = new List<(System.Windows.Controls.RadioButton, System.Windows.Controls.RadioButton)>();
        public TaskCreateMC(Models.Task task)       
        {
            InitializeComponent();
            this.task = task;
        }
        /// <summary>
        /// Erzeugt die neuen Textfelder und Radiobutton bei Click des Butoon
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            
            System.Windows.Controls.TextBox newTextBox = new System.Windows.Controls.TextBox();
            newTextBox.Width = 490; 
            newTextBox.Height = 60;
            newTextBox.Margin = new Thickness(0, 10, 0, 0); 
            Canvas.SetLeft(newTextBox, 100); 
            Canvas.SetTop(newTextBox, currentY);

            System.Windows.Controls.RadioButton radioButton1 = new System.Windows.Controls.RadioButton();
            radioButton1.Content = "Richtig"; 
            radioButton1.GroupName = $"Group{textBoxCount}"; 
            radioButton1.Margin = new Thickness(5, 0, 0, 0); 

            
            System.Windows.Controls.RadioButton radioButton2 = new System.Windows.Controls.RadioButton();
            radioButton2.Content = "Falsch"; 
            radioButton2.GroupName = $"Group{textBoxCount}"; 
            radioButton2.Margin = new Thickness(5, 0, 0, 0);

            Canvas.SetLeft(radioButton1, 80);
            Canvas.SetTop(radioButton1, currentY - 10); 

            
            Canvas.SetLeft(radioButton2, 90); 
            Canvas.SetTop(radioButton2, currentY + 10);

            currentY += 20; 

            TextBoxContainer.Children.Add(newTextBox);
            TextBoxContainer.Children.Add(radioButton1);
            TextBoxContainer.Children.Add(radioButton2);

            textBoxList.Add(newTextBox);
            radioButtonGroups.Add((radioButton1, radioButton2));

        textBoxCount++;
        }
        /// <summary>
        /// Verarbeitet die eingaben der Textfelder
        /// </summary>
        private void ProcessInputs()
        {
            for (int i = 0; i < textBoxList.Count; i++)
            {
                
                string textBoxValue = textBoxList[i].Text;
                Console.WriteLine($"TextBox {i + 1}: {textBoxValue}");

                
                var (radioButton1, radioButton2) = radioButtonGroups[i];

                if (radioButton1.IsChecked == true)// Richtig
                {
                    
                }
                else if (radioButton2.IsChecked == true)//Falsch
                {
                    
                }
                else
                {
                    System.Windows.MessageBox.Show("Bitte auswählen, ob die Antworten Richttig oder Falsch sind");
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
            ProcessInputs();
        }

        private void FrageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
