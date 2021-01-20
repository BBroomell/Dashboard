using System;
using System.Collections.Generic;
using System.Data.SQLite;
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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        Database databaseObject = new Database();

        public LoginPage()
        {
            InitializeComponent();
            databaseObject.OpenConnection(databaseObject.dbConnection);

        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {

            String username;
            String password;

            username = usernameBox.Text;
            password = passwordBox.Password;

            if(databaseObject.LoginUser(username, password, databaseObject.dbConnection) == true)
            {
                ImportPage importpage = new ImportPage();
                this.NavigationService.Navigate(importpage);
            }

            else
            {
                MessageBox.Show("Wrong username or password try again");
            }
            /// this opens up new page
            /// TODO: implement secure login, currently just moving between pages without using username / password info
            /// Previous code from when it was all in MainWindow / Non-Navigation window
            /// ImportPage importpage = new ImportPage();
            ///this.Content = importpage;
            ///
            

        }

        private void createaccountButton_Click(object sender, RoutedEventArgs e)
        {
            CreateAccount createAccount = new CreateAccount();
            this.NavigationService.Navigate(createAccount);
        }

        
    }
}
