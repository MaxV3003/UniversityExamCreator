using System;
using System.Data.SQLite; // SQLite-Bibliothek importieren
using System.Windows;
using System.Windows.Controls;
using UniversityExamCreator.Models;
using UniversityExamCreator.Services;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für TaskKonfig.xaml
    /// </summary>
    public partial class TaskKonfig : Page
    {
        public TaskKonfig()
        {
            InitializeComponent();
            InitializeDD();
            LoadModulesFromDatabase(); 
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFilled() == true)
            {
                Models.Task task;
                string taskContent = ""; // Standardwert für taskContent

                if (MCDD.Text == "Multiple Choice")
                {
                    task = CreateTaskMC();
                    taskContent = MCRulesText.Text; 
                }
                else
                {
                    task = CreateTaskOF();
                }
                NextPath(task);
            }
            else
            {
                MessageBox.Show("Bitte alle Felder füllen");
            }
        }

        /// <summary>
        /// Initialisiert statische Dropdown-Optionen.
        /// </summary>
        private void InitializeDD()
        {
            MCDD.Items.Add("Multiple Choice");
            MCDD.Items.Add("Offene Frage");
            MCDD.SelectedIndex = 1;

            DifficultyDD.Items.Add("Leicht");
            DifficultyDD.Items.Add("Mittel");
            DifficultyDD.Items.Add("Schwer");
        }

        /// <summary>
        /// Lädt die Modulnamen aus der SQLite-Datenbank und fügt sie in die Modul-ComboBox ein.
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

                    string query = "SELECT Name FROM Module"; 
                    using (var command = new SQLiteCommand(query, connection))
                    {
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ModulDD.Items.Add(reader["name"].ToString());
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

        private Models.Task CreateTaskMC()
        {
            int points;
            int.TryParse(PointsText.Text, out points);
            Models.Task taskMC = new Models.Task(ModulDD.SelectedItem.ToString(), ThemeText.Text, MCDD.SelectedItem.ToString(), DifficultyDD.SelectedItem.ToString(), points, TitleText.Text);
            return taskMC;
        }

        private Models.Task CreateTaskOF()
        {
            int points;
            int.TryParse(PointsText.Text, out points);
            Models.Task taskOF = new Models.Task(ModulDD.SelectedItem.ToString(), ThemeText.Text, MCDD.SelectedItem.ToString(), DifficultyDD.SelectedItem.ToString(), points, TitleText.Text);
            return taskOF;
        }

        private void NextPath(Models.Task task)
        {
            if (MCDD.Text == "Multiple Choice")
            {
                NavigationService.Navigate(new TaskCreateMC(task));
            }
            else
            {
                NavigationService.Navigate(new TaskCreateOF(task));
            }
        }

        private Boolean CheckFilled()
        {
            if (ModulDD.SelectedItem == null)
            {
                ModulDD.BorderBrush = System.Windows.Media.Brushes.Red;
                ModulDD.BorderThickness = new Thickness(2);
                return false;
            }
            else if (ThemeText.Text == "")
            {
                ThemeText.BorderBrush = System.Windows.Media.Brushes.Red;
                ThemeText.BorderThickness = new Thickness(2);
                return false;
            }
            else if (DifficultyDD.SelectedItem == null)
            {
                DifficultyDD.BorderBrush = System.Windows.Media.Brushes.Red;
                DifficultyDD.BorderThickness = new Thickness(2);
                return false;
            }
            else if (PointsText.Text == "")
            {
                PointsText.BorderBrush = System.Windows.Media.Brushes.Red;
                PointsText.BorderThickness = new Thickness(2);
                return false;
            }
            else if (TitleText.Text == "")
            {
                TitleText.BorderBrush = System.Windows.Media.Brushes.Red;
                TitleText.BorderThickness = new Thickness(2);
                return false;
            }

            if (MCDD.SelectedItem.ToString() == "Multiple Choice")
            {
                if (MCRules.Text == "")
                {
                    MCRulesText.BorderBrush = System.Windows.Media.Brushes.Red;
                    MCRulesText.BorderThickness = new Thickness(2);
                    return false;
                }
            }
            return true;
        }

        private void MCDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MCDD.SelectedItem.ToString() == "Multiple Choice")
            {
                MCRulesText.Visibility = Visibility.Visible;
                MCRules.Visibility = Visibility.Visible;
            }
            else
            {
                MCRulesText.Visibility = Visibility.Hidden;
                MCRules.Visibility = Visibility.Hidden;
            }
        }

        private void ModulDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DifficultyDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TitleText_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}

