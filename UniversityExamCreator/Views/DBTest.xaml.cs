using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace UniversityExamCreator.Views
{
    public partial class DBTest : Page
    {
        private string dbConnectionString;

        public DBTest(string connectionString)
        {
            dbConnectionString = connectionString;
            InitializeComponent();
            LoadTableNames();
        }

        private void LoadTableNames()
        {
            // Definiere die Tabellen mit ihren Abfragen
            tableQueries = new Dictionary<string, string>
            {
                { "task", "SELECT * FROM task" },
                { "user", "SELECT * FROM user" },
                { "answer", "SELECT * FROM answer" },
                { "module", "SELECT * FROM module" },
                { "hint", "SELECT * FROM hint" },
                { "mcanswer", "SELECT * FROM mcanswer" }
            };

            // Lade Tabellennamen in die ComboBox
            foreach (var tableName in tableQueries.Keys)
            {
                comboBoxTables.Items.Add(tableName);
            }
        }

        private Dictionary<string, string[]> tableFields = new Dictionary<string, string[]>
        {
            { "task", new[] { "module", "topic", "type", "difficulty", "points", "name", "content", "date_created", "author" } },
            { "user", new[] { "username", "password" } },
            { "answer", new[] { "task_id", "answer_content", "username" } },
            { "module", new[] { "moduleID", "name" } },
            { "hint", new[] { "name", "content" } },
            { "mcanswer", new[] { "task_id", "content", "is_correct" } }
        };

        private void ComboBoxTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTables.SelectedItem != null)
            {
                string selectedTable = comboBoxTables.SelectedItem.ToString();
                GenerateDynamicFields(selectedTable);
                LoadTableData(selectedTable);
            }
        }

        private void GenerateDynamicFields(string tableName)
        {
            DynamicFieldsPanel.Children.Clear();

            if (tableFields.ContainsKey(tableName))
            {
                foreach (var field in tableFields[tableName])
                {
                    // Erstelle Label und TextBox für jedes Feld
                    TextBlock label = new TextBlock { Text = field, Margin = new Thickness(0, 5, 0, 0) };
                    TextBox textBox = new TextBox { Name = "txt" + field, Margin = new Thickness(0, 0, 0, 10) };

                    // Füge das Label und die TextBox zum StackPanel hinzu
                    DynamicFieldsPanel.Children.Add(label);
                    DynamicFieldsPanel.Children.Add(textBox);
                }
            }
        }

        private void LoadTableData(string tableName)
        {
            try
            {
                string query = tableQueries[tableName];

                using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            dataGrid.Columns.Clear();
                            dataGrid.ItemsSource = dataTable.DefaultView;
                            dataGrid.Items.Refresh();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Laden der Daten: " + ex.Message);
            }
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxTables.SelectedItem != null)
            {
                string selectedTable = comboBoxTables.SelectedItem.ToString();
                AddRecordToDatabase(selectedTable);
                LoadTableData(selectedTable);
            }
            else
            {
                MessageBox.Show("Bitte wähle eine Tabelle aus.");
            }
        }

        private void AddRecordToDatabase(string tableName)
        {
            if (tableFields.ContainsKey(tableName))
            {
                string fields = string.Join(", ", tableFields[tableName]);
                string values = string.Join(", ", tableFields[tableName].Select(f => $"@{f}"));
                string insertQuery = $"INSERT INTO {tableName} ({fields}) VALUES ({values})";

                using (SQLiteConnection connection = new SQLiteConnection(dbConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        foreach (var field in tableFields[tableName])
                        {
                            var textBox = DynamicFieldsPanel.Children.OfType<TextBox>().FirstOrDefault(tb => tb.Name == "txt" + field);
                            command.Parameters.AddWithValue($"@{field}", string.IsNullOrEmpty(textBox?.Text) ? (object)DBNull.Value : textBox.Text);
                        }
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private Dictionary<string, string> tableQueries;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DevTools());
        }
    }
}

