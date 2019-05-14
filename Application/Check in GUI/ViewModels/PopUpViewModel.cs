using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace EventManager.ViewModels
{
    public class PopUpViewModel
    {
        DataHelper dataHelper;
        DispatcherTimer databaseChecker;
        DispatcherTimer timer;
        int attemptCount;
        int timeTillRetry;

        public PopUpViewModel(DataHelper dh)
        {
            attemptCount = 0;
            dataHelper = dh;
            databaseChecker = new DispatcherTimer();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            //databaseChecker.Interval = new TimeSpan(0, 0, 10);
            databaseChecker.Tick += new EventHandler(CheckDatabaseConnection);
        }

        private void CheckDatabaseConnection(object sender, EventArgs e)
        {
            bool check = dataHelper.IsServerConnected();
            if (!check)
            {
                //MessageBox.Show("Cannot connect to database");
            }
        }

        private void UpdateTime(object sender, EventArgs e)
        {

        }


    }
}
