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
    /// Interaktionslogik für ModulCreate.xaml
    /// </summary>
    public partial class ModulCreate : Page
    {
        public ModulCreate()
        {
            InitializeComponent();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ToolsPage());
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            if (CheckID(ModulIDText)==true)
            {
                MessageBox.write("Modul erstellt");
            }
            else
            {
                MessageBox.write("Modul breits vorhanden");
            }

            //Datenbankeintrag erstellen mit der Madul ID und dem Modulnamen

            NavigationService.Navigate(new ToolsPage());
        }

        private bool CheckID(int ID)
        {
            if ("Datenbankabfrage zur ID ==ID")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
