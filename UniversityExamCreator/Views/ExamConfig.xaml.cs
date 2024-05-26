using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UniversityExamCreator.Views
{
    /// <summary>
    /// Interaktionslogik für ExamConfig.xaml
    /// </summary>
    public partial class ExamConfig : Page
    {
        //The Item which was selected in the Module-DD.
        string SelectedItem=string.Empty;

        //The Item which was selected from ExamType.
        private RadioButton SelectedRadioButton;


        public ExamConfig()
        {
            InitializeComponent();

            //ComboBox-Content
            Module.Items.Add("EinfInf");
            Module.Items.Add("Mathe3");
            Module.Items.Add("AuD");
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
            //hier muss noch das ConfigItem erstellt werden. 

            NavigationService.Navigate(new ExamCreate());
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
            SelectedItem= Module.SelectedItem.ToString();
            //ProcessSelectedItem();
        }

        /// <summary>
        /// Testfunktion, ob das richtige Item ausgegeben und gespeichert wird. 
        /// </summary>
        /*private void ProcessSelectedItem()
        {
            MessageBox.Show("Ausgewähltes Item für interne Speicherung: " + SelectedItem);
        }
        */

        /// <summary>
        /// 3 Radiobuttons to check which Type of Exam it should be. 
        /// </summary>
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton checkedRadioButton)
            {
                // Den neuen ausgewählten RadioButton speichern
                SelectedRadioButton = checkedRadioButton;
            }
            //ProcessSelectedRadioButton();
        }

        /// <summary>
        /// Testfunktion, ob der richtige Radiobutton übergeben wird.
        /// </summary>
        /*private void ProcessSelectedRadioButton()
        {
            MessageBox.Show("Ausgewählter RadioButton: " + SelectedRadioButton.Content.ToString());
        }
        */




        /// <summary>
        /// List for DropDown-Items for the ModuleDropDown. 
        /// </summary>
        /// <returns></returns>
        /*private List<string> getDropDownItems()
        {
            //add DB-Funcion heare. 
        }*/
    }

    //ToDo: Freitextfelder implementieren und Werte speichern
}
