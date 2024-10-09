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
using UniversityExamCreator.Models;

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
        private List<System.Windows.Controls.StackPanel> panelList = new List<System.Windows.Controls.StackPanel>();
        private List<System.Windows.Controls.TextBox> textBoxList = new List<System.Windows.Controls.TextBox>();
        private List<(System.Windows.Controls.RadioButton, System.Windows.Controls.RadioButton)> radioButtonGroups = new List<(System.Windows.Controls.RadioButton, System.Windows.Controls.RadioButton)>();
        public TaskCreateMC(Models.Task task)       
        {
            InitializeComponent();
            this.task = task;
        }
        /// <summary>
        /// Erzeugt neue Textfelder und Radiobuttons bei Klick auf den Button "Hinzufügen"
        /// </summary>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.TextBox newTextBox = new System.Windows.Controls.TextBox();
            newTextBox.Width = 400;
            newTextBox.Height = 60;
            newTextBox.Margin = new Thickness(0, 10, 0, 0);
            newTextBox.BorderBrush = new SolidColorBrush(Colors.White);

            System.Windows.Controls.RadioButton radioButton1 = new System.Windows.Controls.RadioButton();
            radioButton1.Content = "Richtig";
            radioButton1.GroupName = $"Group{textBoxCount}";
            radioButton1.Margin = new Thickness(5, 0, 0, 0);

            System.Windows.Controls.RadioButton radioButton2 = new System.Windows.Controls.RadioButton();
            radioButton2.Content = "Falsch";
            radioButton2.GroupName = $"Group{textBoxCount}";
            radioButton2.Margin = new Thickness(5, 0, 0, 0);

            // Erstelle ein horizontales StackPanel, um die TextBox und die RadioButtons nebeneinander anzuordnen
            System.Windows.Controls.StackPanel horizontalPanel = new System.Windows.Controls.StackPanel();
            horizontalPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

            // Füge die TextBox und die RadioButtons zum horizontalen StackPanel hinzu
            horizontalPanel.Children.Add(newTextBox);
            horizontalPanel.Children.Add(radioButton1);
            horizontalPanel.Children.Add(radioButton2);

            // Füge das horizontale StackPanel in das vertikale StackPanel (TextBoxContainer) ein
            TextBoxContainer.Children.Add(horizontalPanel);

            // Speichere das StackPanel, die TextBox und das RadioButton-Paar
            panelList.Add(horizontalPanel); // Das gesamte Panel speichern
            textBoxList.Add(newTextBox);
            radioButtonGroups.Add((radioButton1, radioButton2));

            textBoxCount++;
        }

        /// <summary>
        /// Löscht das zuletzt hinzugefügte Textfeld und die zugehörigen Radiobuttons
        /// </summary>
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (panelList.Count > 0)
            {
                // Entferne das letzte StackPanel (das die TextBox und die RadioButtons enthält)
                System.Windows.Controls.StackPanel lastPanel = panelList.Last();
                TextBoxContainer.Children.Remove(lastPanel); // Entferne das Panel aus dem UI
                panelList.RemoveAt(panelList.Count - 1); // Entferne das Panel aus der Liste

                // Entferne das letzte TextBox-Element und die zugehörigen Radiobuttons aus den Listen
                textBoxList.RemoveAt(textBoxList.Count - 1);
                radioButtonGroups.RemoveAt(radioButtonGroups.Count - 1);

                // Zähler verringern
                textBoxCount--;
            }
            else
            {
                System.Windows.MessageBox.Show("Es gibt keine weiteren Elemente zum Löschen.");
            }
        }


        /// <summary>
        /// Verarbeitet die eingaben der Textfelder und fügt zu Zum Objekt Task hinzu
        /// </summary>
        private void ProcessInputs(Models.Task task)
        {
            string TaskName=task.getName();
            for (int i = 0; i < textBoxList.Count; i++)
            {
                
                string textBoxValue = textBoxList[i].Text;
                MCAnswer Answer = new MCAnswer(TaskName, textBoxValue, i + 1, 0);
                
                var (radioButton1, radioButton2) = radioButtonGroups[i];

                if (radioButton1.IsChecked == true)// Richtig
                {
                    Answer.AnswerFlag = 1;
                }
                else if (radioButton2.IsChecked == true)//Falsch
                {
                    Answer.AnswerFlag = 0;
                }
                else
                {
                    //System.Windows.MessageBox.Show("Bitte auswählen, ob die Antworten Richttig oder Falsch sind");
                }
                //task.MCAnswers.set(Answer);
            }
            task.TaskContent = QuestionText.Text;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            ProcessInputs(task);
            NavigationService.Navigate(new ToolsPage());
            
        }

        private void FrageText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
