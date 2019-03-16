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
    /// Interaction logic for Logfile_Converter.xaml
    /// </summary>
    public partial class LogfileConverterWindow : Window
    {
        public LogfileConverterWindow()
        {
            InitializeComponent();
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            CheckOutWindow window2 = new CheckOutWindow();
            window2.Show();
            //Actions
            Close();
        }
        
    }
}
