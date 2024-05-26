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
    /// Interaktionslogik für ExamConfig.xaml
    /// </summary>
    public partial class ExamConfig : Page
    {
        string SelectedItem=string.Empty; 

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
            ProcessSelectedItem();
        }

        /// <summary>
        /// Testfunktion, ob das richtige Item ausgegeben und gespeichert wird. 
        /// </summary>
        private void ProcessSelectedItem()
        {
            MessageBox.Show("Ausgewähltes Item für interne Speicherung: " + SelectedItem);
        }

        /// <summary>
        /// List for DropDown-Items for the ModuleDropDown. 
        /// </summary>
        /// <returns></returns>
        /*private List<string> getDropDownItems()
        {
            //add DB-Funcion heare. 
        }*/
    }
}
