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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        public MainWindow()
        {
            /// Want Full Size Window, Single Border with close, minimize options
            WindowStyle = WindowStyle.SingleBorderWindow;
            WindowState = WindowState.Maximized;
            ///ImportPage importpage = new ImportPage();
            ///_mainFrame.NavigationService.Navigate(importpage);
            InitializeComponent();
        }



    }
}
