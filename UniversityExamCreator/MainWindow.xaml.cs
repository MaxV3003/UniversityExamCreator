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
using System.Windows.Shapes;
using UniversityExamCreator.Models;
using UniversityExamCreator.Views;

namespace UniversityExamCreator
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new LogIn());
            PathFinder pathFinder = new PathFinder("Databases", "database.db");
            DatabaseManager databaseManager = new DatabaseManager(pathFinder.GetPath());
            databaseManager.CreateTables(pathFinder.GetPath());
        }

    }
}
