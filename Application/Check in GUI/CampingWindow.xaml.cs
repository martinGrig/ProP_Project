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
    /// Interaction logic for Camping.xaml
    /// </summary>
    public partial class CampingWindow : Window
    {
        bool isPaid;
        public CampingWindow()
        {
            InitializeComponent();
            isPaid = false;
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            LogfileConverterWindow window2 = new LogfileConverterWindow();
            window2.Show();
            //Actions
            Close();
        }

        private void PaidOrUnpaidButton_Click(object sender, RoutedEventArgs e)
        {
            isPaid = !isPaid;
            if (isPaid)
            {
                PayForCampingSpotButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                PayForCampingSpotButton.Visibility = Visibility.Visible;
            }
        }
    }
}
