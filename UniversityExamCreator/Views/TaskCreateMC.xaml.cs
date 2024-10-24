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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

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
            newTextBox.BorderBrush = new SolidColorBrush(Colors.Blue);

            System.Windows.Controls.RadioButton radioButton1 = new System.Windows.Controls.RadioButton();
            radioButton1.Content = "Richtig";
            radioButton1.GroupName = $"Group{textBoxCount}";
            radioButton1.Margin = new Thickness(5, 0, 0, 0);

            System.Windows.Controls.RadioButton radioButton2 = new System.Windows.Controls.RadioButton();
            radioButton2.Content = "Falsch";
            radioButton2.GroupName = $"Group{textBoxCount}";
            radioButton2.Margin = new Thickness(5, 0, 0, 0);

            System.Windows.Controls.StackPanel horizontalPanel = new System.Windows.Controls.StackPanel();
            horizontalPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;

            horizontalPanel.Children.Add(newTextBox);
            horizontalPanel.Children.Add(radioButton1);
            horizontalPanel.Children.Add(radioButton2);

            TextBoxContainer.Children.Add(horizontalPanel);

            panelList.Add(horizontalPanel); 
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
                
                System.Windows.Controls.StackPanel lastPanel = panelList.Last();
                TextBoxContainer.Children.Remove(lastPanel); 
                panelList.RemoveAt(panelList.Count - 1); 

               
                textBoxList.RemoveAt(textBoxList.Count - 1);
                radioButtonGroups.RemoveAt(radioButtonGroups.Count - 1);

                
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
                MCAnswer answer = new MCAnswer(textBoxValue, i + 1, 0);
                
                var (radioButton1, radioButton2) = radioButtonGroups[i];

                if (radioButton1.IsChecked == true)
                {
                    answer.AnswerFlag = 1;
                }
                else if(radioButton2.IsChecked == true)
                {
                    answer.AnswerFlag = 0;
                }
                
                task.addMCAnswer(answer);
            }
            task.TaskContent = QuestionText.Text;
            
        }

        private bool CheckFilled()
        {
            string TaskName = task.getName();

            for (int i = 0; i < textBoxList.Count; i++)
            {
                string textBoxValue = textBoxList[i].Text;
                var (radioButton1, radioButton2) = radioButtonGroups[i];

                // Überprüfung: Wurde keiner der beiden Radiobuttons ausgewählt?
                if (radioButton1.IsChecked == false && radioButton2.IsChecked == false)
                {
                    // Rot umrandete Radiobuttons als visuelles Feedback
                    radioButton1.BorderBrush = System.Windows.Media.Brushes.Red;
                    radioButton1.BorderThickness = new Thickness(2);
                    radioButton2.BorderBrush = System.Windows.Media.Brushes.Red;
                    radioButton2.BorderThickness = new Thickness(2);

                    // Fehlermeldung anzeigen
                    System.Windows.MessageBox.Show($"Bitte auswählen, ob Antwort {i + 1} Richtig oder Falsch ist");
                    return false;
                }

                // Überprüfung: Ist das Textfeld leer?
                else if (string.IsNullOrEmpty(textBoxList[i].Text))
                {
                    textBoxList[i].BorderBrush = System.Windows.Media.Brushes.Red;
                    textBoxList[i].BorderThickness = new Thickness(2);
                    System.Windows.MessageBox.Show($"Bitte einen Antworttext bei Antwort {i + 1} angeben");
                    return false;
                }

                // Überprüfung: Ist die Frage leer?
                else if (string.IsNullOrEmpty(QuestionText.Text))
                {
                    QuestionText.BorderBrush = System.Windows.Media.Brushes.Red;
                    QuestionText.BorderThickness = new Thickness(2);
                    System.Windows.MessageBox.Show("Bitte einen Fragetext angeben");
                    return false;
                }
            }

            return true;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new TaskKonfig());
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string username = username_text.Text;
            if (string.IsNullOrEmpty(username))
            {
                username = ""; // Fallback für leeren Benutzernamen
            }

            if (CheckFilled() == true)
            {
                if (string.IsNullOrEmpty(task.Author))
                {
                    task.Author = "Unbekannter Autor";
                }

                System.Windows.MessageBox.Show("Aufgabe wurde gespeichert");

                ProcessInputs(task);

                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();
                string connectionString = $"Data Source={databasePath};Version=3;";
                DataService dataService = new DataService(connectionString);

                task.Id = dataService.InsertTask(
                    topic: task.Topic,
                    taskType: task.TaskType,
                    difficulty: task.Difficulty,
                    points: task.Points,
                    taskName: task.TaskName,
                    taskContent: task.getTaskContent(),
                    dateCreated: DateTime.Now,
                    author: task.Author
                );
                foreach (var answer in task.MCAnswers)
                {
                    dataService.InsertMCAnswer(task.Id, answer.Content, answer.AnswerFlag);
                }
                NavigationService.Navigate(new ToolsPage());
            }
        }

    }
}
