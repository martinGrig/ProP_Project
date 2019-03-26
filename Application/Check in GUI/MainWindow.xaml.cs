using EventManager.ViewModels;
using SourceWeave.Controls;
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
using System.Windows.Shapes;

namespace EventManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : SWWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new LoginViewModel();
            this.Title = "Log-in screen";
        }

        private void LoginView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new LoginViewModel();
            this.Title = "Log-in screen";
        }

        private void AppsView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new AppsViewModel();
            this.Title = "Apps screen";
        }

        private void CheckInView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new CheckinViewModel();
            this.Title = "Check in screen";
        }

        private void ShopView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ShopViewModel();
            this.Title = "Shop screen";
        }
        

        private void CampingView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new CampingViewModel();
            this.Title = "Camping screen";
        }

        private void CheckOutView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new CheckOutViewModel();
            this.Title = "Check out screen";
        }

        private void ConverterView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new ConverterViewModel();
            this.Title = "Converter screen";
        }

        private void StatusView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new StatusViewModel();
            this.Title = "Status screen";
        }
        private void LoanStandView_Clicked(object sender, RoutedEventArgs e)
        {
            DataContext = new LoanStandViewModel();
            this.Title = "Loan stand screen";
        }
    }
}
