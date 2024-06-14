﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

using System;
using System.Data.SQLite;

/*public class DatabaseManager
{
    private string connectionString;

    public DatabaseManager(string databasePath)
    {
        connectionString = $"Data Source={databasePath};Version=3;";
    }

    public void CreateTables()
    {
        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        {
            conn.Open();

            string createAufgabeTable = @"
                CREATE TABLE IF NOT EXISTS aufgabe (
                    id INTEGER PRIMARY KEY,
                    task_content TEXT NOT NULL,
                    points INTEGER NOT NULL,
                    difficulty VARCHAR(50) NOT NULL,
                    subject VARCHAR(100) NOT NULL,
                    date_created DATE NOT NULL,
                    author VARCHAR(100) NOT NULL
                );";

            string createKlausurTable = @"
                CREATE TABLE IF NOT EXISTS klausur (
                    id INTEGER PRIMARY KEY,
                    course VARCHAR(100) NOT NULL,
                    examiner VARCHAR(100) NOT NULL,
                    date DATE NOT NULL
                );";

            string createKlausurAufgabeTable = @"
                CREATE TABLE IF NOT EXISTS klausur_aufgabe (
                    id INTEGER PRIMARY KEY,
                    klausur_id INTEGER NOT NULL,
                    aufgabe_id INTEGER NOT NULL,
                    FOREIGN KEY (klausur_id) REFERENCES klausur(id),
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";

            string createNutzerTable = @"
                CREATE TABLE IF NOT EXISTS nutzer (
                    id INTEGER PRIMARY KEY,
                    username VARCHAR(50) NOT NULL,
                    password VARCHAR(20) NOT NULL
                );";

            string createAntwortTable = @"
                CREATE TABLE IF NOT EXISTS antwort (
                    id INTEGER PRIMARY KEY,
                    aufgabe_id INTEGER NOT NULL,
                    answer_content TEXT NOT NULL,
                    username VARCHAR(50) NOT NULL,
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";
            string createKlausurConfigTable = @"
                CREATE TABLE IF NOT EXISTS nutzer (
                    id INTEGER PRIMARY KEY,
                    FOREIGN KEY (klausur_id) REFERENCES klausur(id),
                    FOREIGN KEY (Nutzer_id) REFERENCES Nutzer(id)
                );";
            string createAufgabeAntwortTable = @"
                CREATE TABLE IF NOT EXISTS klausur_aufgabe (
                    id INTEGER PRIMARY KEY,
                    antwort_id INTEGER NOT NULL,
                    aufgabe_id INTEGER NOT NULL,
                    FOREIGN KEY (antwort_id) REFERENCES antwort(id),
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";
            ExecuteNonQuery(conn, createAufgabeTable);
            ExecuteNonQuery(conn, createKlausurTable);
            ExecuteNonQuery(conn, createKlausurAufgabeTable);
            ExecuteNonQuery(conn, createNutzerTable);
            ExecuteNonQuery(conn, createAntwortTable);
            ExecuteNonQuery(conn, createKlausurConfigTable);
            ExecuteNonQuery(conn, createAufgabeAntwortTable);
        }
    }
    private void ExecuteNonQuery(SQLiteConnection conn, string sql)
    {
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
            cmd.ExecuteNonQuery();
        }
    }
}*/
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DatenbankDropdown
{
    public partial class MainForm : Form
    {
        private string connectionString = "DeineVerbindungszeichenfolge"; // Ersetze durch deine Verbindungszeichenfolge

        public MainForm()
        {
            InitializeComponent();
            ErstelleTabellen();
            LadeDatenInComboBox();
        }

        private void InitializeComponent()
        {
            throw new NotImplementedException();
        }

        private void ErstelleTabellen()
        {
            string createAufgabeTable = @"
                CREATE TABLE IF NOT EXISTS aufgabe (
                    id INTEGER PRIMARY KEY,
                    task_content TEXT NOT NULL,
                    points INTEGER NOT NULL,
                    difficulty VARCHAR(50) NOT NULL,
                    subject VARCHAR(100) NOT NULL,
                    date_created DATE NOT NULL,
                    author VARCHAR(100) NOT NULL
                );";

            string createKlausurTable = @"
                CREATE TABLE IF NOT EXISTS klausur (
                    id INTEGER PRIMARY KEY,
                    course VARCHAR(100) NOT NULL,
                    examiner VARCHAR(100) NOT NULL,
                    date DATE NOT NULL
                );";

            string createKlausurAufgabeTable = @"
                CREATE TABLE IF NOT EXISTS klausur_aufgabe (
                    id INTEGER PRIMARY KEY,
                    klausur_id INTEGER NOT NULL,
                    aufgabe_id INTEGER NOT NULL,
                    FOREIGN KEY (klausur_id) REFERENCES klausur(id),
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";

            string createNutzerTable = @"
                CREATE TABLE IF NOT EXISTS nutzer (
                    id INTEGER PRIMARY KEY,
                    username VARCHAR(50) NOT NULL,
                    password VARCHAR(20) NOT NULL
                );";

            string createAntwortTable = @"
                CREATE TABLE IF NOT EXISTS antwort (
                    id INTEGER PRIMARY KEY,
                    aufgabe_id INTEGER NOT NULL,
                    answer_content TEXT NOT NULL,
                    username VARCHAR(50) NOT NULL,
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";

            string createKlausurConfigTable = @"
                CREATE TABLE IF NOT EXISTS klausur_config (
                    id INTEGER PRIMARY KEY,
                    klausur_id INTEGER NOT NULL,
                    nutzer_id INTEGER NOT NULL,
                    FOREIGN KEY (klausur_id) REFERENCES klausur(id),
                    FOREIGN KEY (nutzer_id) REFERENCES nutzer(id)
                );";

            string createAufgabeAntwortTable = @"
                CREATE TABLE IF NOT EXISTS aufgabe_antwort (
                    id INTEGER PRIMARY KEY,
                    antwort_id INTEGER NOT NULL,
                    aufgabe_id INTEGER NOT NULL,
                    FOREIGN KEY (antwort_id) REFERENCES antwort(id),
                    FOREIGN KEY (aufgabe_id) REFERENCES aufgabe(id)
                );";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    command.CommandText = createAufgabeTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createKlausurTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createKlausurAufgabeTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createNutzerTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createAntwortTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createKlausurConfigTable;
                    command.ExecuteNonQuery();

                    command.CommandText = createAufgabeAntwortTable;
                    command.ExecuteNonQuery();

                    MessageBox.Show("Tabellen wurden erfolgreich erstellt.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Erstellen der Tabellen: " + ex.Message);
                }
            }
        }

        private void LadeDatenInComboBox()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT task_content FROM aufgabe"; // Beispielabfrage
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        comboBox1.Items.Add(reader["task_content"].ToString());
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fehler beim Abrufen der Daten: " + ex.Message);
                }
            }
        }
    }
}


