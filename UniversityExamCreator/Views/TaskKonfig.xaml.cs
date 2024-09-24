﻿using System;
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
using UniversityExamCreator.Models;
using System.Windows.Shapes;

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
        }        

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFilled() == true)
            {
                if(MCDD.Text == "Multiple Choice")
                {
                    
                    NextPath(CreateTaskMC());
                }
                else
                {
                    
                    NextPath(CreateTaskOF());
                }           
            }
            else
            {
                MessageBox.Show("Bitte alle Felder füllen");
                
            }
        }
        /// <summary>
        /// Erzeugt die Dropdown Optionen.
        /// </summary>
        private void InitializeDD()
        {
            ModulDD.Items.Add("Einfinf");
            ModulDD.Items.Add("Mathe 1");
            ModulDD.Items.Add("Mathe 2");

            MCDD.Items.Add("Multiple Choice");
            MCDD.Items.Add("Offene Frage");
            MCDD.SelectedIndex = 1;

            DifficultyDD.Items.Add("Leicht");
            DifficultyDD.Items.Add("Mittel");
            DifficultyDD.Items.Add("Schwer");

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

        /// <summary>
        /// NextPath entscheidet auf welche WPF-Seite der User als nächstes kommt, basierend auf den Eingaben auf der aktuellen Seite.
        /// </summary>
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

        private string CheckEerythingFilled()
        {
            return "";
        }
        


        /// <summary>
        /// ZUm testen neuer Funktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //string[] test = new string[5];
            
            /*string testString = "";
            for (int j = 0; j < 5; j++)
            {
                testString = testString + test[j];
            }*/

            //Text();
        }
        /// <summary>
        /// Checkt, ob alle Pflichfelder befüllt wurden und markiert alle rot, die nicht befüllt wurden.
        /// </summary>
        /// <returns></returns>
        private Boolean CheckFilled()// Kann man noch besser schreiben, sodass alle fehlenden Felder direkt markiert werden.
        {
            //string selectedText = MCDD.SelectedItem.ToString();

            if (ModulDD.SelectedItem == null)
            {
                ModulDD.BorderBrush = Brushes.Red;
                ModulDD.BorderThickness = new Thickness(2);
                return false;
            }
            else if (ThemeText.Text == "")
            {
                ThemeText.BorderBrush = Brushes.Red;
                ThemeText.BorderThickness = new Thickness(2);
                return false;
            }
            else if (DifficultyDD.SelectedItem == null)
            {
                DifficultyDD.BorderBrush = Brushes.Red;
                DifficultyDD.BorderThickness = new Thickness(2);
                return false;
            }
            else if (PointsText.Text == "")
            {
                PointsText.BorderBrush = Brushes.Red;
                PointsText.BorderThickness = new Thickness(2);
                return false;
            }
            else if (TitleText.Text == "")
            {
                TitleText.BorderBrush = Brushes.Red;
                TitleText.BorderThickness = new Thickness(2);
                return false;
            }

            if (MCDD.SelectedItem.ToString() == "Multiple Choice")
            {
                if (MCRules.Text == "")
                {
                    MCRulesText.BorderBrush = Brushes.Red;
                    MCRulesText.BorderThickness = new Thickness(2);
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Die Felder MCCCountDD und MCRulesText werden erst benutzbar gemacht, wenn Multiple Choice als Art der Klausur ausgewählt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MCDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string selectedText = MCDD.SelectedItem.ToString();

            if (MCDD.SelectedItem.ToString() == "Multiple Choice")
            {
                MCRulesText.Visibility = Visibility.Visible;
                MCRules.Visibility = Visibility.Visible;
            }
            else
            {
                MCRulesText.Visibility = Visibility.Hidden;
                MCRules.Visibility= Visibility.Hidden;  
            }
        }
    }
    
}
