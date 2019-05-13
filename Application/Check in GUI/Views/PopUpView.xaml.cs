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
using System.Windows.Threading;

namespace EventManager.Views
{
    /// <summary>
    /// Interaction logic for PopUpView.xaml
    /// </summary>
    public partial class PopUpView : Window
    {
        DataHelper dataHelper;
        DispatcherTimer timer;
        int attemptCount;
        int timeTillRetry;
        public PopUpView(DataHelper dh)
        {
            InitializeComponent();
            timeTillRetry = 10;
            attemptCount = 0;
            dataHelper = dh;
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(UpdateTime);
            timer.Start();
        }


        private void CheckDatabaseConnection(object sender, EventArgs e)
        {
            bool check = dataHelper.IsServerConnected();
            if (check)
            {
                this.Close();
            }
            else
            {
                lbTime.Content = "Conection attempt fail";
            }
            
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            timeTillRetry -= 1;
            if (timeTillRetry <= 0)
            {
                CheckDatabaseConnection(null, null);
                if (timeTillRetry == -1)
                {
                    if (attemptCount == 0)
                    {
                        timeTillRetry = 10;
                    }
                    else if (attemptCount < 4)
                    {
                        timeTillRetry = 30;
                    }
                    else
                    {
                        timeTillRetry = 60;
                    }
                    attemptCount++;
                    lbTime.Content = timeTillRetry;
                    
                }

            }
            else
            {
                lbTime.Content = timeTillRetry;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckDatabaseConnection(null, null);
        }
    }
}
