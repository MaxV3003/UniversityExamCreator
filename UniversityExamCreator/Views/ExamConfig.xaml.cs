using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using UniversityExamCreator.Models;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ExamConfig.xaml
    /// </summary>
    public partial class ExamConfig : Page
    {
        //Item muss noch erstellt und befüllt werden!
        Examconfig Examconfig { get; set; }

        internal ExamConfig(Examconfig examconfig)
        {
            Examconfig = examconfig;
            InitializeComponent();

            //ComboBox-Content
            Module.Items.Add("EinfInf");
            Module.Items.Add("Mathe3");
            Module.Items.Add("AuD");

            ConfigLoader(examconfig);
            /*
             * Implement the Function to get the Items from the list
             * List<string> Items = getDropDownItems();
             */

            //Checkboxen

        }

        /// <summary>
        /// Button to get on the next Page. 
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(NumTasks.Text, out int taskAmount))
            {
                Examconfig.TaskAmount = taskAmount;
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige Zahl für die Anzahl der Aufgaben ein.");
                return;
            }

            if (double.TryParse(NumPoints.Text, out double pointAmount))
            {
                Examconfig.PointAmount = pointAmount;
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige Zahl für die Punktanzahl ein.");
                return;
            }

            Examconfig.ExamName = ExamTitle.Text;
            Examconfig.toString();
            NavigationService.Navigate(new ExamCreate(Examconfig));
        }

        /// <summary>
        /// Button to get to the last Page. 
        /// </summary>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());  
        }

        /// <summary>
        /// Dropdown-Menu to select the Module. 
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           Examconfig.ModuleID = Module.SelectedItem.ToString();
           
        }

        /// <summary>
        /// 3 Radiobuttons to check which Type of Exam it should be. 
        /// </summary>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRadioButton)
            {
                //SelectedRadioButton = checkedRadioButton;
                Examconfig.ExamType = checkedRadioButton.Name.ToString();
            }
        }

        internal void ConfigLoader(Examconfig examconfig)
        {
            if (!examconfig.isEmpty())
            {
                // Modul laden
                if (!string.IsNullOrEmpty(examconfig.ModuleID))
                {
                    Module.SelectedItem = examconfig.ModuleID;
                }

                // Prüfungsart laden
                switch (examconfig.ExamType)
                {
                    case "MC":
                        MC.IsChecked = true;
                        break;
                    case "OffeneFragen":
                        OffeneFragen.IsChecked = true;
                        break;
                    case "Mischform":
                        Mischform.IsChecked = true;
                        break;
                }

                // Aufgabenanzahl laden
                NumTasks.Text = examconfig.TaskAmount.ToString();

                // Punktanzahl laden
                NumPoints.Text = examconfig.PointAmount.ToString();

                // Klausurüberschrift laden
                ExamTitle.Text = examconfig.ExamName;
            }
        }
    }
}
