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
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        Database databaseObject = new Database();

        public CreateAccount()
        {
            InitializeComponent();
            databaseObject.OpenConnection(databaseObject.dbConnection);
        }

        private void createAccountButton_Click(object sender, RoutedEventArgs e)
        {
            String newUsername;
            String newName;
            String newPassword;
            String newPin;

            newUsername = newUsernameBox.Text;
            newPassword = newPasswordBox.Password;
            newName = newNameBox.Text;
            newPin = newPinBox.Password;

            if(databaseObject.IsExistingUser(newUsername, databaseObject.dbConnection) == false)
            {
                databaseObject.InsertNewUser(newUsername, newName, newPassword, newPin, databaseObject.dbConnection);
                MessageBox.Show("UserAccount Created!");
                LoginPage loginpage = new LoginPage();
                this.NavigationService.Navigate(loginpage);
            }
            else
            {
                MessageBox.Show("Username exists already. Please choose another username.");
            }
        }
    }
}
