using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Text;
using System.Data.SQLite;
using Microsoft.VisualBasic.FileIO;
using Spire.Xls;

 class TempDB
    {
        // Parse imported CSV file and initiate transfer of data into SQLite DB
        // Takes an active SQLite connection and a string containing file location
        public static void ImportData(SQLiteConnection conn, String file)
        {
            ArrayList headers = new ArrayList(); // Holds column headers
            ArrayList rows = new ArrayList(); // Holds row stringw
            int i = 0;

            // If file is excel format convert to csv
            int fileFlag = 0;
            if (file.Contains(".xlsx"))
            {
                //System.Environment.Exit(1);
                fileFlag = 1;
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(file);
                Worksheet sheet = workbook.Worksheets[0];
                sheet.SaveToFile("temp.csv", ",", Encoding.UTF8);
                file = "temp.csv";
            }

            // Parse CSV using "," delimeter
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] parts = parser.ReadFields();

                    // Get column headers
                    if (i == 0)
                    {
                        foreach (string part in parts)
                            headers.Add(part);
                    }

                    // Break at end of row
                    if (parts == null)
                    {
                        break;
                    }
                    // Get row data after headers
                    if (i > 0)
                    {
                        foreach (string part in parts)
                            rows.Add(part);
                    }
                    i++;
                }
            }
            int numRows = i - 1; // Total number of rows minus header
            int numCols = headers.Count; // Total number of columns

            // Delete converted csv file
            if (fileFlag == 1)
                File.Delete(file);

            Console.WriteLine("Number of rows: " + numRows);
            Console.WriteLine("Number of cols: " + numCols);

            // Initiate transfer of data to SQLite DB
            TempTable(conn, headers, numCols);
            TempData(conn, rows, numRows, numCols);

        }

        // Create a new Temporary SQLite Connection in memory
        public static SQLiteConnection CreateConnection()
        {

            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source = :memory:;");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return sqlite_conn;
        }

        // Dynamically create temp SQL table command using file headers from imported file
        public static void TempTable(SQLiteConnection conn, ArrayList headers, int numCols)
        {
            String cmd = "CREATE TABLE TempTable (";
            for (int i = 0; i < numCols; i++)
            {
                cmd += headers[i];
                if (i == numCols - 1)
                    cmd += " TEXT";
                else
                    cmd += " TEXT, ";
            }
            cmd += ")";

            // Create the new table
            CreateTable(conn, cmd);
        }

        // Create a new SQLite Table
        // Takes in active SQLite connection and string containing SQL command
        public static void CreateTable(SQLiteConnection conn, String cmd)
        {
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.ExecuteNonQuery();
        }

        // Creates INSERT SQL command for imported rows
        private static void TempData(SQLiteConnection conn, ArrayList rows, int numRows, int numCols)
        {
            int line = 1; // keeps track of each line/row added to statement
            int cursor = 0; // keeps track of current position in rows

            // Iterates through each row
            for (int j = 0; j < numRows; j++)
            {
                // Creates single insert statement for each row/line
                String cmd = "INSERT INTO TempTable VALUES(";
                for (int i = cursor; i < (numCols * line); i++)
                {
                    cmd += ("\'" + rows[i] + "\'");
                    if (i == (numCols * line) - 1)
                    {
                        cursor++;
                        break;
                    }
                    else
                        cmd += ", ";
                    cursor++;
                }
                cmd += ")";
                line++;

                //Console.WriteLine(cmd);

                // Insert each row into DB
                InsertData(conn, cmd);
            }
        }

        // Insert data into SQLite DB
        // Takes in active SQLite connection and string containing SQL command
        public static void InsertData(SQLiteConnection conn, String cmd)
        {
            SQLiteCommand sqlite_cmd;

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = cmd;
            sqlite_cmd.ExecuteNonQuery();
        }

        // Read data from SQLite DB
        // Takes in active SQLite connection and string containing SQL command
        public static DataTable FetchData(SQLiteConnection conn, String cmd)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            DataTable table = new DataTable();

            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = cmd;
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            table.Load(sqlite_datareader); // Load query results into DataTable

        /*
            // Read all DataTable contents
            //Console.WriteLine();
            foreach (DataRow dataRow in table.Rows)
            {
                foreach (var item in dataRow.ItemArray)
                {
                    Console.WriteLine(item);
                }
            }
        */
            return table;
        }

        // Close SQLite DB connection
        // Takes in active SQLite connection
        public static void CloseConnection(SQLiteConnection conn)
        {
            conn.Close();
        }

    }

