using System;
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
        /// <summary>
        /// Die Klasse StringPair erstellt ein Objekt bestehend aus zwei Strings
        /// Diese werden gebraucht um der Liste die dem Service übergeben wird immer dem Content und den dazu passenden Namen zu schicken.
        /// </summary>
        public class StringPair
        {
            public string Name { get; set; }
            public string Content { get; set; }

            public StringPair(string name, string content)
            {
                Name = name;
                Content = content;
            }

            public override string ToString()
            {
                return Name + ", " + Content;
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFilled() == true)
            {
                CreateList();
                NextPath();
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

            MCCountDD.Items.Add("1");
            MCCountDD.Items.Add("2");
            MCCountDD.Items.Add("3");

            DifficultyDD.Items.Add("Leicht");
            DifficultyDD.Items.Add("Mittel");
            DifficultyDD.Items.Add("Schwer");

        }

        /// <summary>
        /// Die Methode CreateList erstellt je nach dem, ob eine MC oder eine OF Aufgabe erstellt wird eine Liste mit StringPairs
        /// </summary>
        /// <returns></returns>
        private List<StringPair> CreateList()
        {
            List<StringPair> contentList = new List<StringPair>();

            string selectedText = MCDD.SelectedItem.ToString();

            if (selectedText == "Multiple Choice")
            {
                return CreateListMC(contentList);
            }
            else
            {
                return CreateListOF(contentList);
            }


        }
        /// <summary>
        /// Die Methode CreateListMC erstellt die eigentliche StringPiar Liste mit den Eingaben die für MC gebraucht werden.
        /// </summary>
        /// <param name="contentListMC"></param>
        /// <returns></returns>
        private List<StringPair> CreateListMC(List<StringPair> contentListMC)
        {
            string selectedModulText = ModulDD.SelectedItem.ToString();
            StringPair modul = new StringPair("Modul", selectedModulText);

            StringPair theme = new StringPair("Theme", ThemeText.Text);

            string selectedMCCountText = MCCountDD.SelectedItem.ToString();
            StringPair mcCount = new StringPair("MCCOunt", selectedMCCountText);

            StringPair mcRules = new StringPair("MCRules", MCRulesText.Text);

            string selectedDifficultyText = DifficultyDD.SelectedItem.ToString();
            StringPair difficulty = new StringPair("Difficulty", selectedDifficultyText);

            StringPair points = new StringPair("Points", PointsText.Text);

            StringPair title = new StringPair("Title", TitleText.Text);

            contentListMC.Add(modul);
            contentListMC.Add(theme);
            contentListMC.Add(mcCount);
            contentListMC.Add(mcRules);
            contentListMC.Add(difficulty);
            contentListMC.Add(points);
            contentListMC.Add(title);

            return contentListMC;
        }
        /// <summary>
        /// Die Methode CreateListOF erstellt die eigentliche StringPiar Liste mit den Eingaben die für OF gebraucht werden.
        /// </summary>
        /// <param name="contentListOF"></param>
        /// <returns></returns>
        private List<StringPair> CreateListOF(List<StringPair> contentListOF)
        {
            string selectedModulText = ModulDD.SelectedItem.ToString();
            StringPair modul = new StringPair("Modul", selectedModulText);

            StringPair theme = new StringPair("Theme", ThemeText.Text);

            string selectedDifficultyText = DifficultyDD.SelectedItem.ToString();
            StringPair difficulty = new StringPair("Difficulty", selectedDifficultyText);

            StringPair points = new StringPair("Points", PointsText.Text);

            StringPair title = new StringPair("Title", TitleText.Text);

            contentListOF.Add(modul);
            contentListOF.Add(theme);
            contentListOF.Add(difficulty);
            contentListOF.Add(points);
            contentListOF.Add(title);

            return contentListOF;
        }


        /// <summary>
        /// NextPath entscheidet auf welche WPF-Seite der User als nächstes kommt, basierend auf den Eingaben auf der aktuellen Seite.
        /// </summary>
        private void NextPath()
        {
            if (MCDD.Text == "Multiple Choice")
            {
                NavigationService.Navigate(new TaskCreateMC());
            }
            else
            {
                NavigationService.Navigate(new TaskCreateOF());
            }
        }

        private string CheckEerythingFilled()
        {
            return "";
        }
        

        /// <summary>
        /// greift aud den eigegebenen Text zu.
        /// </summary>
        private void Text()
        {
            CreateList();
        }
        /// <summary>
        /// ZUm testen neuer Funktionen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Test_Click(object sender, RoutedEventArgs e)
        {
            //string[] test = new string[5];
            string test = "";
            for (int i = 0; i < CreateList().Count;  i++)
            {
                test = test + CreateList()[i].ToString();
            }
            MessageBox.Show(test);
            /*string testString = "";
            for (int j = 0; j < 5; j++)
            {
                testString = testString + test[j];
            }*/

            //Text();
        }

        private Boolean CheckFilled()
        {
            //string selectedModulText = ModulDD.SelectedItem.ToString();
            //string selectedMCCountText = MCCountDD.SelectedItem.ToString();
            //string selectedDifficultyText = DifficultyDD.SelectedItem.ToString();
            string selectedText = MCDD.SelectedItem.ToString();

            if (selectedText == "Multiple Choice")
            {
                if (ModulDD.SelectedItem == null)
                {
                    ModulDD.BorderBrush = Brushes.Red;
                    ModulDD.BorderThickness = new Thickness(2);
                    return false;
                }
                else if(MCCountDD.SelectedItem == null)
                {
                    MCCountDD.BorderBrush = Brushes.Red;
                    MCCountDD.BorderThickness = new Thickness(2);
                    return false;
                }
                else if (DifficultyDD.SelectedItem == null)
                {
                    DifficultyDD.BorderBrush = Brushes.Red;
                    DifficultyDD.BorderThickness = new Thickness(2);
                    return false;
                }
                else if (ThemeText.Text =="")
                {
                    ThemeText.BorderBrush = Brushes.Red;
                    ThemeText.BorderThickness = new Thickness(2);
                    return false;
                }
                else if (MCRulesText.Text == "")
                {
                    MCRulesText.BorderBrush = Brushes.Red;
                    MCRulesText.BorderThickness = new Thickness(2);
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
                else
                {
                    return true;
                }
            }
            else
            {
                if (ModulDD.SelectedItem == null)
                {
                    ModulDD.BorderBrush = Brushes.Red;
                    ModulDD.BorderThickness = new Thickness(2);
                    return false;
                }
                else if (DifficultyDD.SelectedItem == null)
                {
                    DifficultyDD.BorderBrush = Brushes.Red;
                    DifficultyDD.BorderThickness = new Thickness(2);
                    return false;
                }
                else if (ThemeText.Text == "")
                {
                    ThemeText.BorderBrush = Brushes.Red;
                    ThemeText.BorderThickness = new Thickness(2);
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
                else
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Die Felder MCCCountDD und MCRulesText werden erst benutzbar gemacht, wenn Multiple Choice als Art der Klausur ausgewählt wurde.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MCDD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedText = MCDD.SelectedItem.ToString();

            if (selectedText == "Multiple Choice")
            {
                MCCountDD.IsEnabled = true;
                MCRulesText.IsEnabled = true;
            }
            else
            {
                MCCountDD.IsEnabled = false;
                MCRulesText.IsEnabled = false;
            }
        }
    }
}
