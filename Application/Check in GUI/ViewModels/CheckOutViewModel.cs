using EventManager.Models;
using EventManager.Objects;
using Phidget22;
using Phidget22.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EventManager.ViewModels
{
    public class CheckOutViewModel : ObservableObject, IPageViewModel
    {
        Visitor _visitor;
        public DataModel Dm { get; set; }
        DataHelper dh;
        public MainViewModel _mainViewModel { get; set; }
        private RFID myRFIDReader;
        public CheckOutViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Dm = _mainViewModel.dataModel;
            dh = new DataHelper();
            myRFIDReader = _mainViewModel._MyRFIDReader;
            _loans = new List<Loan>();
        }

        private Display _display;
        public Display Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }

        private List<Loan> _loans;
        public IEnumerable<Loan> Loans
        {
            get
            {
                return _loans;
            }
            set
            {
                _loans = value.ToList();
                OnPropertyChanged("Loans");
            }
        }

        public void Start()
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
            //_mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            //_mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            try
            {
                myRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);

                myRFIDReader.Open();
            }
            catch (PhidgetException) { System.Windows.Forms.MessageBox.Show("Please connect rfid reader"); }
        }

        public void GetLoans(object sender, RFIDTagEventArgs e)
        {
            try
            {
                _visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                try
                {
                    if (_visitor != null)
                    {
                        Loans = dh.GetLoans(_visitor);
                        //if the dont have any loanen items
                        if (Loans.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Visitor has no unreturned loaned items", "", false, false);
                            myRFIDReader.AntennaEnabled = false;
                        }
                        else if (Loans.Where(l => l.IsOverdue).Count() > 0)
                        {
                            Display = new Display(Brushes.Orange, "Visitor must return overdue items", "", false, false);
                            myRFIDReader.AntennaEnabled = false;
                        }
                        else
                        {
                            Display = new Display(Brushes.Black, "Visitor must return items before checking out", "", false, false);
                        }
                        myRFIDReader.Tag -= new RFIDTagEventHandler(GetLoans);
                    }
                    else
                    {
                        Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
                    }
                }
                catch
                {
                    Display = new Display(Brushes.Red, "Something went wrong with loans", "times", false, true);
                }
                //The visitor is valid


            }
            catch
            {
                Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
            }




        }
    }
}
