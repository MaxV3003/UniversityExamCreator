using System;
using System.Data.SQLite;
using UniversityExamCreator.Models;

public class DatabaseManager
{
    private string connectionString;

    public DatabaseManager(string databasePath)
    {
        // Setze die Verbindungszeichenfolge korrekt
        connectionString = databasePath;
    }

    public void CreateTables(string connection)
    {
        string connectionDataSource = $"Data Source=" + connection + ";Version=3;";
        using (SQLiteConnection conn = new SQLiteConnection(connectionDataSource))
        {
            conn.Open();

            string createTaskTable = @"
            CREATE TABLE IF NOT EXISTS task (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            topic TEXT NOT NULL,
            type TEXT NOT NULL,
            difficulty VARCHAR(50) NOT NULL,
            points INTEGER NOT NULL,
            name TEXT NOT NULL,
            content TEXT NOT NULL,
            date_created DATE NOT NULL,
            author TEXT 
            );";

            string createExamTable = @"
            CREATE TABLE IF NOT EXISTS exam (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            course VARCHAR(100) NOT NULL,
            examiner VARCHAR(100) NOT NULL,
            date DATE NOT NULL
            );";

            string createExamTaskTable = @"
            CREATE TABLE IF NOT EXISTS exam_task (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            exam_id INTEGER NOT NULL,
            task_id INTEGER NOT NULL,
            FOREIGN KEY (exam_id) REFERENCES exam(id),
            FOREIGN KEY (task_id) REFERENCES task(id)
            );";

            string createUserTable = @"
            CREATE TABLE IF NOT EXISTS user (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            username VARCHAR(50) NOT NULL,
            password VARCHAR(20) NOT NULL
            );";

            string createAnswerTable = @"
            CREATE TABLE IF NOT EXISTS answer (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            task_id INTEGER NOT NULL,
            answer_content TEXT NOT NULL,
            FOREIGN KEY (task_id) REFERENCES task(id)
            );";

            string createExamConfigTable = @"
            CREATE TABLE IF NOT EXISTS exam_config (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            exam_id INTEGER NOT NULL,
            user_id INTEGER NOT NULL,
            FOREIGN KEY (exam_id) REFERENCES exam(id),
            FOREIGN KEY (user_id) REFERENCES user(id)
            );";

            string createTaskAnswerTable = @"
            CREATE TABLE IF NOT EXISTS task_answer (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            answer_id INTEGER NOT NULL,
            task_id INTEGER NOT NULL,
            FOREIGN KEY (answer_id) REFERENCES answer(id),
            FOREIGN KEY (task_id) REFERENCES task(id)
            );";

            string createModuleTable = @"
            CREATE TABLE IF NOT EXISTS module (
            moduleID INTEGER PRIMARY KEY,
            name TEXT NOT NULL
            );";

            string createTempexamTable = @"
            CREATE TABLE IF NOT EXISTS tempexam (
            exam_id INTEGER,
            task_id INTEGER,
            FOREIGN KEY(exam_id) REFERENCES exam(id),
            FOREIGN KEY(task_id) REFERENCES task(id),
            PRIMARY KEY (exam_id, task_id)
            );";

            string createHintTable = @"
            CREATE TABLE IF NOT EXISTS hint (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            name VARCHAR(50) NOT NULL,
            content TEXT NOT NULL
            );";


            ExecuteNonQuery(conn, createTaskTable);
            ExecuteNonQuery(conn, createExamTable);
            ExecuteNonQuery(conn, createExamTaskTable);
            ExecuteNonQuery(conn, createUserTable);
            ExecuteNonQuery(conn, createAnswerTable);
            ExecuteNonQuery(conn, createExamConfigTable);
            ExecuteNonQuery(conn, createTaskAnswerTable);
            ExecuteNonQuery(conn, createModuleTable);
            ExecuteNonQuery(conn, createTempexamTable);
            ExecuteNonQuery(conn, createHintTable);
        }
    }

    private void ExecuteNonQuery(SQLiteConnection conn, string sql)
    {
        using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
        {
            //cmd.ExecuteNonQuery();
        }
    }
}






