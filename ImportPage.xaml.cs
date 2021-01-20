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

namespace Dashboard
{
    /// <summary>
    /// Interaction logic for ImportPage.xaml
    /// </summary>
    public partial class ImportPage : Page
    {
        public ImportPage()
        {

            InitializeComponent();
        }

        private void openfile_Click(object sender, RoutedEventArgs e)
        {
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
           // dlg.FileName = ""; // Default file name
           // dlg.DefaultExt = ".xlsx"; // Default file extension
            //dlg.Filter = "Excel documents (.xlsx)|*.xlsx"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                //string filename = dlg.FileName;
                // want to use this value in dashboard page
                (App.Current as App).FileImported = dlg.FileName;
            }
        }

        private void gotoDashboard_Click(object sender, RoutedEventArgs e)
        {

            DashboardPage dash = new DashboardPage();
            this.NavigationService.Navigate(dash);

        }
    }
}
