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

        }

        /// <summary>
        /// Button to get on the next Page. 
        /// </summary>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Examconfig.ExamType))
            {
                MessageBox.Show("Bitte wählen Sie eine Prüfungsart aus.");
                return;
            }

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
                var result = MessageBox.Show("Wollen Sie wirklich keine Punktzahl festlegen?", "Ungültige Eingabe", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
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
                Examconfig.ExamType = checkedRadioButton.Name.ToString();
            }
        }

        /// <summary>
        /// Loader for the Examconfig-Item. Especially if u switch the Pages. 
        /// </summary>
        /// <param name="examconfig"></param>
        internal void ConfigLoader(Examconfig examconfig)
        {
            if (!examconfig.isEmpty())
            {

                if (!string.IsNullOrEmpty(examconfig.ModuleID))
                {
                    Module.SelectedItem = examconfig.ModuleID;
                }

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

                NumTasks.Text = examconfig.TaskAmount.ToString();
                NumPoints.Text = examconfig.PointAmount.ToString();
                ExamTitle.Text = examconfig.ExamName;
            }
        }
    }
}
