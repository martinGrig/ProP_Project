
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
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : SWWindow
    {
        public LoginWindow()
        {
            InitializeComponent();

        }
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CheckInWindow window1 = new CheckInWindow();
            window1.Show();
            //Actions
            Close();
        }

        private void getRidOfBorder(object sender, EventArgs e)
        {
            
                // Does not appear to have any effect:
                // the brush remains yellow.
            TempBack.Visibility = Visibility.Collapsed;
            Logo.Visibility = Visibility.Collapsed;
            
        }
    }
}
