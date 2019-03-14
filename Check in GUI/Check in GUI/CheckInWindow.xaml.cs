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

namespace EventManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CheckInWindow : Window
    {
        public CheckInWindow()
        {
            InitializeComponent();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CampingWindow window2 = new CampingWindow();
            window2.Show();
            //Actions
            Close();
        }
    }
}
