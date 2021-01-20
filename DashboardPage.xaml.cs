using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.IO;
using System.Data.SQLite;
using Spire.Xls;


using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
//using DocumentFormat.OpenXml.Spreadsheet;

//using DocumentFormat.OpenXml.Office.CustomUI;

//using S = DocumentFormat.OpenXml.Spreadsheet.Sheets;
//using E = DocumentFormat.OpenXml.OpenXmlElement;
//using A = DocumentFormat.OpenXml.OpenXmlAttribute;


// DataSet
using System.Data;

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for DashboardPage.xaml
    /// </summary>
    /// System.Windows.Controls.Page
    
    public partial class DashboardPage : System.Windows.Controls.Page
    {
     /*   public static Sheets GetAllWorksheets(string fileName)
        {
            Sheets mySheets = null;
            // retrieving sheets
            using (SpreadsheetDocument document =
                    SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart wbPart = document.WorkbookPart;
                mySheets = wbPart.Workbook.Sheets;
            }
            return mySheets;
        }

        // If column name ends in spaces, it will have binding issues later on
        // So, getting rid of any extra spaces before doing anything with the name
        public System.String eliminateExtraSpaces(string text)
        {
            char[] trimSpaces = { ' ' };
            // start at end of string and work backwards, dont care about any other spaces
            text = text.Trim(trimSpaces);

            return text;
        }

        public System.Data.DataTable createColumn(DataTable table, string text)
        {
            DataColumn col = new DataColumn();

            col.ColumnName = text;
            table.Columns.Add(col);
            return table;
        }

        public System.Data.DataTable fillDataTable(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            System.Data.DataTable myTable = new System.Data.DataTable();
            OpenXmlReader reader = OpenXmlReader.Create(worksheetPart);
            Boolean isHeader = true;
            LinkedList<string> headers = new LinkedList<string>();
            LinkedListNode<string> current = null;
            string first = null;
            int numCols = 0;

            // reads along rows, left to right
            while (reader.Read())
            {
                if (reader.ElementType == typeof(Row))
                {
                    // create corresponding row in datatable
                    DataRow myRow = myTable.NewRow();

                    reader.ReadFirstChild();
                    do
                    {
                        if (reader.ElementType == typeof(Cell))
                        {
                            Cell c = (Cell)reader.LoadCurrentElement();

                            string cellValue;

                            if (c.DataType != null && c.DataType == CellValues.SharedString)
                            {
                                SharedStringItem ssi = workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(int.Parse(c.CellValue.InnerText));

                                cellValue = eliminateExtraSpaces(ssi.Text.Text);
                            }
                            else
                            {

                                if (c.CellValue == null)
                                {
                                    // account for when cell is intentionally left blank
                                    cellValue = "";
                                }
                                else
                                {
                                    cellValue = eliminateExtraSpaces(c.CellValue.InnerText);
                                }
                            }

                            // set isHeader = true when it's on the first row with headers, else false
                            if (isHeader)
                            {
                                // add column headers to a linked list for reference later on
                                if (current == null)
                                {

                                    headers.AddFirst(cellValue);
                                    current = headers.Find(cellValue);
                                    // keep track of name of first column 
                                    first = cellValue;
                                    myTable = createColumn(myTable, cellValue);
                                }
                                else
                                {
                                    // add this header to the linked list, after the node called 'current'
                                    headers.AddAfter(current, cellValue);
                                    // re-assign 'current' node to be this cell
                                    current = headers.Find(cellValue);
                                    myTable = createColumn(myTable, cellValue);
                                }
                                // each time you enter isHeader, you've encountered a new column,
                                // so incrementing numCols 
                                numCols++;

                            }
                            else
                            {

                                myRow[current.Value] = cellValue;
                                // move current to next value in linked list for next loop
                                current = current.Next;
                            }
                            // parsing values correctly
                            // System.Diagnostics.Debug.WriteLine("{0}: {1} ", c.CellReference, cellValue);

                        }
                    } while (reader.ReadNextSibling());

                    // add completed row to table
                    if (!isHeader)
                    {
                        myTable.Rows.Add(myRow);

                    }

                    // need to add headers to datatable before moving on
                    isHeader = false;
                    // reset current to first column header
                    current = headers.Find(first);
                }
            }



            return myTable;
        }

        public async void openWorkbook(string filename)
        {
            System.Data.DataTable td = new System.Data.DataTable();
            DataSet ds = new DataSet();

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(filename, false))
            {
                // SAX approach for large data files
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;

                DataSet myData = new DataSet();

                // search through list of sheets
                var mySheets = GetAllWorksheets(filename);


                // going through list of sheets
                foreach (Sheet item in mySheets)
                {

                    // get reference to worksheet part
                    WorksheetPart worksheetPart = (WorksheetPart)(workbookPart.GetPartById(item.Id));
                    System.Data.DataTable table = new System.Data.DataTable();

                    table = await Task.Run(() => fillDataTable(worksheetPart, workbookPart));

                    // add datatable to dataset
                    myData.Tables.Add(table);

                }
                // easier for testing if we can manipulate the dataview separately
                // TODO: change back to one line of code
                DataView view = myData.Tables[0].DefaultView;
                // send to DataGrid
                mydatagrid.ItemsSource = view;

            }

        }*/

        public DashboardPage()
        {
            InitializeComponent();

            // Ask user to select a file to import
            string filename = (App.Current as App).FileImported;

            //openWorkbook(filename);

            // Create SQLite Temp Database Connection and import file to DB
            SQLiteConnection sqlite_conn;
            sqlite_conn = TempDB.CreateConnection();
            TempDB.ImportData(sqlite_conn, filename);

            // Query Temp Database - Currently returns all columns for students who retook a course and failed
            string cmd = "SELECT DISTINCT * FROM TempTable WHERE Instat = 'R' AND Outstat = 'F'";
            DataTable table = TempDB.FetchData(sqlite_conn, cmd);

            DataView view = table.DefaultView;
            // send to DataGrid
            mydatagrid.ItemsSource = view;

            // Close SQLite Temp Database Connection
            TempDB.CloseConnection(sqlite_conn);
        }
    }
}
