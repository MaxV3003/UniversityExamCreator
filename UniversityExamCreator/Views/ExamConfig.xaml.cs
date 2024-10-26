using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.SQLite;
using UniversityExamCreator.Models;
using UniversityExamCreator.Services;

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
            LoadModulesFromDatabase();
            ConfigLoader();
        }

        /// <summary>
        /// Lädt die Module direkt aus der Datenbank und fügt sie in die ComboBox ein.
        /// </summary>
        private void LoadModulesFromDatabase()
        {
            try
            {
                PathFinder pathFinder = new PathFinder("Databases", "database.db");
                string databasePath = pathFinder.GetPath();

                using (var connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    // SQL-Abfrage zum Abrufen der Modulnamen
                    string query = "SELECT Name FROM Module";
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Füge den Modulnamen zur ComboBox hinzu
                                Module.Items.Add(reader.GetString(0));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Module: " + ex.Message);
            }
        }

        /// <summary>
        /// Button to get on the next Page. 
        /// </summary>
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            // Check for selected type of the exam.
            if (string.IsNullOrEmpty(Examconfig.ExamType))
            {
                MessageBox.Show("Bitte wählen Sie eine Prüfungsart aus.");
                MC.BorderBrush = Brushes.Red;
                MC.BorderThickness = new Thickness(2);
                OffeneFragen.BorderBrush = Brushes.Red;
                OffeneFragen.BorderThickness = new Thickness(2);
                Mischform.BorderBrush = Brushes.Red;
                Mischform.BorderThickness = new Thickness(2);
                return;
            }

            // Check for selected taskamount of the exam.
            if (int.TryParse(NumTasks.Text, out int taskAmount))
            {
                Examconfig.TaskAmount = taskAmount;
            }
            else
            {
                MessageBox.Show("Bitte geben Sie eine gültige Zahl für die Anzahl der Aufgaben ein.");
                NumTasks.BorderBrush = Brushes.Red;
                NumTasks.BorderThickness = new Thickness(2);
                return;
            }

            // Check for selected pointamount of the exam. 
            if (double.TryParse(NumPoints.Text, out double pointAmount))
            {
                Examconfig.PointAmount = pointAmount;
            }
            else
            {
                var result = MessageBox.Show("Wollen Sie wirklich keine Punktzahl festlegen?", "Ungültige Eingabe", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                NumPoints.BorderBrush = Brushes.Red;
                NumPoints.BorderThickness = new Thickness(2);
                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            Examconfig.ExamName = ExamTitle.Text;
            //Examconfig.toString();
            NavigationService.Navigate(new ExamCreate(Examconfig));
        }

        /// <summary>
        /// Button to get to the last Page. 
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
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
                Examconfig.ExamType = checkedRadioButton.Content.ToString();
            }
        }

        /// <summary>
        /// Loader for the Examconfig-Item. Especially if switching the Pages. 
        /// </summary>
        internal void ConfigLoader()
        {
            if (!Examconfig.isEmpty())
            {

                if (!string.IsNullOrEmpty(Examconfig.ModuleID))
                {
                    Module.SelectedItem = Examconfig.ModuleID;
                }

                switch (Examconfig.ExamType)
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

                NumTasks.Text = Examconfig.TaskAmount.ToString();
                NumPoints.Text = Examconfig.PointAmount.ToString();
                ExamTitle.Text = Examconfig.ExamName;
            }
        }

        /// <summary>
        /// Change-Event for the taskamount-selection.
        /// </summary>
        private void NumTasks_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(NumTasks.Text, out int taskAmount))
            {
                Examconfig.TaskAmount = taskAmount;
            }
        }

        /// <summary>
        /// Change-Event for the pointamount-selection.
        /// </summary>
        private void NumPoints_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(NumPoints.Text, out double taskPoints))
            {
                Examconfig.PointAmount = taskPoints;
            }
        }
    }
}

