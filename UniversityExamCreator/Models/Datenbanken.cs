using System;
using System.Data.SQLite;

public class DatabaseManager
{
    private string connectionString;

    public DatabaseManager(string databasePath)
    {
        connectionString = databasePath;
        
    }

    public void CreateTables(string connection)
    {
        string connectionDataSource = $"Data Source=" + connection + ";Version=3;";
        using (SQLiteConnection conn = new SQLiteConnection(connectionDataSource))
        {
            conn.Open();

            string createAufgabeTable = @"
            CREATE TABLE IF NOT EXISTS aufgabe (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                topic TEXT NOT NULL,
                type TEXT NOT NULL,
                difficulty VARCHAR(50) NOT NULL,
                points INTEGER NOT NULL,
                name TEXT NOT NULL,
                content TEXT NOT NULL,
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
            string createModulTable = @"
            CREATE TABLE IF NOT EXISTS nutzer (
                id INTEGER PRIMARY KEY,
                name VARCHAR(50) NOT NULL,
                faculty VARCHAR(50) NOT NULL
            );";
            ExecuteNonQuery(conn, createAufgabeTable);
            ExecuteNonQuery(conn, createKlausurTable);
            ExecuteNonQuery(conn, createKlausurAufgabeTable);
            ExecuteNonQuery(conn, createNutzerTable);
            ExecuteNonQuery(conn, createAntwortTable);
            ExecuteNonQuery(conn, createKlausurConfigTable);
            ExecuteNonQuery(conn, createAufgabeAntwortTable);
            ExecuteNonQuery(conn, createModulTable);
        }
    
    }
    private void ExecuteNonQuery(SQLiteConnection conn, string sql)
    {
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
            cmd.ExecuteNonQuery();
        }
    }
}


