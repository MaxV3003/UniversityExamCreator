using System.Data.SQLite;
using System;

public class DataService
{
    private string connection;

    public DataService(string connectionString)
    {
        connection = connectionString; 
    }


    // Insert Task
        public void InsertTask(string topic, string taskType, string difficulty, int points, string taskName, string taskContent, DateTime dateCreated, string author)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string insertQuery = @"
                    INSERT INTO task (topic, type, difficulty, points, name, content, date_created, author) 
                    VALUES (@topic, @type, @difficulty, @points, @name, @content, @date_created, @author)";

                using (SQLiteCommand command = new SQLiteCommand(insertQuery, conn))
                {
                    command.Parameters.AddWithValue("@topic", topic);
                    command.Parameters.AddWithValue("@type", taskType);
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    command.Parameters.AddWithValue("@points", points);
                    command.Parameters.AddWithValue("@name", taskName);
                    command.Parameters.AddWithValue("@content", taskContent);
                    command.Parameters.AddWithValue("@date_created", dateCreated);
                    command.Parameters.AddWithValue("@author", author);

                command.ExecuteNonQuery();
                }
            }
        }
        // Insert Exam
        public void InsertExam(string course, string examiner, DateTime date)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string insertquery = @"
                    INSERT INTO exam (course, examiner, date) 
                    VALUES (@course, @examiner, @date)";

                using (SQLiteCommand command = new SQLiteCommand(insertquery, conn))
                {
                    command.Parameters.AddWithValue("@course", course);
                    command.Parameters.AddWithValue("@examiner", examiner);
                    command.Parameters.AddWithValue("@date", date);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert Exam Task
        public void InsertExamTask(int exam_id, int task_id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO exam_task (exam_id, task_id) 
                    VALUES (@exam_id, @task_id)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@exam_id", exam_id);
                    command.Parameters.AddWithValue("@task_id", task_id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert User
        public void InsertUser(string username, string password)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO user (username, password) 
                    VALUES (@username, @password)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert Answer
        public void InsertAnswer(int task_id, string answer_content, string username)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO answer (task_id, answer_content, username) 
                    VALUES (@task_id, @answer_content, @username)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@task_id", task_id);
                    command.Parameters.AddWithValue("@answer_content", answer_content);
                command.Parameters.AddWithValue("@username", username);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert Exam Config
        public void InsertExamConfig(int exam_id, int user_id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO exam_config (exam_id, user_id) 
                    VALUES (@exam_id, @user_id)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@exam_id", exam_id);
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert Task Answer
        public void InsertTaskAnswer(int answer_id, int task_id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO task_answer (answer_id, task_id) 
                    VALUES (@answer_id, @task_id)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@answer_id", answer_id);
                    command.Parameters.AddWithValue("@task_id", task_id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Insert Module
        public void InsertModule(string moduleID, string name)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = @"
                    INSERT INTO module (modulID, name) 
                    VALUES (@modulID, @name)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@moduleID", moduleID);
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertTempExam(int examID, int taskID)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();

                string query = @"
                    INSERT INTO tempexam (exam_id, task_id) 
                    VALUES (@examID, @taskID)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@examID", examID);
                    command.Parameters.AddWithValue("@taskID", taskID);
                    command.ExecuteNonQuery();
                }
            }
        }
        public void InsertHint(string name, string content)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();

                string query = @"
                        INSERT INTO tempexam (name, content) 
                        VALUES (@name, @content)";

                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@content", content);
                    command.ExecuteNonQuery();
                }
            }
        }


        //----------------------------------- 
        // Delete Section
        //-----------------------------------

        // Delete Task
        public void DeleteTask(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM task WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Task erfolgreich gelöscht.");
                }
            }
        }

        // Delete Exam
        public void DeleteExam(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM exam WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Exam erfolgreich gelöscht.");
                }
            }
        }

        // Delete Exam Task
        public void DeleteExamTask(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM exam_task WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete User
        public void DeleteUser(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM user WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("User erfolgreich gelöscht.");
                }
            }
        }

        // Delete Answer
        public void DeleteAnswer(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM answer WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Answer erfolgreich gelöscht.");
                }
            }
        }

        // Delete Exam Config
        public void DeleteExamConfig(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM exam_config WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Exam Config erfolgreich gelöscht.");
                }
            }
        }

        // Delete Task Answer
        public void DeleteTaskAnswer(int id)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM task_answer WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        // Delete Module
        public void DeleteModule(string modulID)
        {
            using (SQLiteConnection conn = new SQLiteConnection(connection))
            {
                conn.Open();
                string query = "DELETE FROM module WHERE modulID = @modulID";
                using (SQLiteCommand command = new SQLiteCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@modulID", modulID);
                    command.ExecuteNonQuery();
                    Console.WriteLine("Modul erfolgreich gelöscht.");
                }
            }
        }


        //----------------------------------- 
        // If-Exists Section
        //-----------------------------------

        // Check if Task Exists
        public bool IfTaskExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM task WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Task: {ex.Message}");
                return false;
            }
        }

        // Check if Exam Exists
        public bool IfExamExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM exam WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Exam: {ex.Message}");
                return false;
            }
        }

        // Check if Exam Task Exists
        public bool IfExamTaskExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM exam_task WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Exam Task: {ex.Message}");
                return false;
            }
        }

        // Check if User Exists
    public bool IfUserExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM user WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für User: {ex.Message}");
                return false;
            }
        }

        // Check if Answer Exists
        public bool IfAnswerExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM answer WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Answer: {ex.Message}");
                return false;
            }
        }

        // Check if Exam Config Exists
        public bool IfExamConfigExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM exam_config WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Exam Config: {ex.Message}");
                return false;
            }
        }

        // Check if Task Answer Exists
        public bool IfTaskAnswerExists(int id)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM task_answer WHERE id = @id";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Task Answer: {ex.Message}");
                return false;
            }
        }

        // Check if Module Exists
        public bool IfModuleExists(string modulID)
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connection))
                {
                    conn.Open();
                    string query = "SELECT COUNT(1) FROM module WHERE modulID = @modulID";
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@modulID", modulID);
                        return Convert.ToInt32(command.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der Ausführung der Abfrage für Module: {ex.Message}");
                return false;
            }
        }
    }




