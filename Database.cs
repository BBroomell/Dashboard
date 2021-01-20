using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;

namespace Dashboard
{
    class Database
    {
        public SQLiteConnection dbConnection;

        public Database()
        {
            dbConnection = new SQLiteConnection("Data Source=database.sqlite3");

            if (!File.Exists("./database.sqlite3"))
            {
                //Creates the table schema for the useraccounts to initilize the database
                string createTable = @"CREATE TABLE IF NOT EXISTS [Users] (
                [ID] INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                [Username] TEXT NOT NULL UNIQUE,
                [Name] TEXT NOT NULL,
                [Password] TEXT NOT NULL,
                [Pin] TEXT NOT NULL )";
                SQLiteConnection.CreateFile("database.sqlite3");

                using (SQLiteCommand myCommand = new SQLiteCommand(dbConnection))
                {
                    OpenConnection(dbConnection);
                    myCommand.CommandText = createTable;
                    myCommand.ExecuteNonQuery();
                    CloseConnection(dbConnection);
                }
                System.Console.WriteLine("Database created");
            }
        }

        //Opens the connection to the database.
        public void OpenConnection(SQLiteConnection dbConnection)
        {
            if (dbConnection.State != System.Data.ConnectionState.Open)
            {
                dbConnection.Open();
            }
        }

        //Closes the connection to the database.
        public void CloseConnection(SQLiteConnection dbConnection)
        {
            if (this.dbConnection.State != System.Data.ConnectionState.Closed)
            {
                this.dbConnection.Close();
            }
        }

        public bool LoginUser(string username, string password, SQLiteConnection dbConnection)
        {
            SQLiteCommand comm = new SQLiteCommand(dbConnection);
            comm.CommandText = "SELECT COUNT(*) FROM Users WHERE Username=(@param1) AND Password=(@param2)";
            comm.CommandType = System.Data.CommandType.Text;
            comm.Parameters.Add(new SQLiteParameter("@param1", username));
            comm.Parameters.Add(new SQLiteParameter("@param2", password));

            Int32 counter = 0;
            counter = Convert.ToInt32(comm.ExecuteScalar());

            if (counter == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public void InsertNewUser(String newUsername,String newName, String newPassword, String newPin, SQLiteConnection dbConnection)
        {
                    String query = "INSERT INTO Users ('Username', 'Name', 'Password', 'Pin') VALUES (@Username, @Name, @Password, @Pin)";

                    SQLiteCommand myCommand = new SQLiteCommand(query, dbConnection);
                    myCommand.Parameters.AddWithValue("@Username", newUsername);
                    myCommand.Parameters.AddWithValue("@Password", newPassword);
                    myCommand.Parameters.AddWithValue("@Name", newName);
                    myCommand.Parameters.AddWithValue("@Pin", newPin);
                    var result = myCommand.ExecuteNonQuery();
        }

        //Checks database for an existing username, returns true if found, false if not found.
        public bool IsExistingUser(string username, SQLiteConnection dbConnection)
        {
            SQLiteCommand comm = new SQLiteCommand(dbConnection);
            comm.CommandText = "SELECT COUNT(*)  FROM Users WHERE Username=(@param1)";
            comm.CommandType = System.Data.CommandType.Text;
            comm.Parameters.Add(new SQLiteParameter("@param1", username));
            Int32 counter = 0;
            counter = Convert.ToInt32(comm.ExecuteScalar());

            if (counter == 0)
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
