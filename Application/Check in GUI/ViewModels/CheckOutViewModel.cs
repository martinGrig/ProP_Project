using EventManager.Models;
using EventManager.Objects;
using EventManager.ViewModels.Commands;
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
        public Visitor Visitor
        {
            get
            {
                return _visitor;
            }
            set
            {
                _visitor = value;
                OnPropertyChanged("Visitor");
            }
        }
        public DataModel Dm { get; set; }
        DataHelper dh;
        public MainViewModel _mainViewModel { get; set; }
        public CheckOutViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Dm = _mainViewModel.dataModel;
            dh = new DataHelper();
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

        private bool _canSeeDisplay2;
        public bool CanSeeDisplay2
        {
            get
            {
                return _canSeeDisplay2;
            }
            set
            {
                _canSeeDisplay2 = value;
                OnPropertyChanged("CanSeeDisplay2");
            }
        }

        private Display _display2;
        public Display Display2
        {
            get
            {
                return _display2;
            }
            set
            {
                _display2 = value;
                OnPropertyChanged("Display2");
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

        private string _buttonTempText;
        public string ButtonTempText
        {
            get
            {
                return _buttonTempText;
            }
            set
            {
                _buttonTempText = value;
                OnPropertyChanged("ButtonTempText");
            }
        }

        private string _buttonInvalidText;
        public string ButtonInvalidText
        {
            get
            {
                return _buttonInvalidText;
            }
            set
            {
                _buttonInvalidText = value;
                OnPropertyChanged("ButtonInvalidText");
            }
        }

        private RelayCommand _clearVisitorCommand;
        public RelayCommand ClearVisitorCommand
        {
            get
            {
                if (_clearVisitorCommand == null) _clearVisitorCommand = new RelayCommand(new Action<object>(ClearEmployee));
                return _clearVisitorCommand;
            }
        }

        private void ClearEmployee(object obj)
        {
            Reset(null, null);
        }

        public void Start()
        {
            Dm = _mainViewModel.dataModel;
            Visitor = null;
            Loans = new List<Loan>();
            Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            try
            {
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);
                _mainViewModel._MyRFIDReader.Detach += new DetachEventHandler(DetachRfid);
                _mainViewModel._MyRFIDReader.Attach += new AttachEventHandler(AttachRfid);
                _mainViewModel._MyRFIDReader.Open();
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false); }
        }

        private void AttachRfid(object sender, AttachEventArgs e)
        {
            
                CanSeeDisplay2 = false;
                if (Visitor == null)
                {
                    _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                }
                else
                {
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                }

        }
        private void DetachRfid(object sender, DetachEventArgs e)
        {
            Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false);
            CanSeeDisplay2 = true;
        }

        public void GetLoans(object sender, RFIDTagEventArgs e)
        {
            try
            {
                Visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                try
                {
                    if (Visitor != null)
                    {
                        Loans = dh.GetLoans(_visitor);
                        //if the dont have any loanen items
                        if (Loans.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Visitor has no unreturned loaned items", "", false, false);
                            ButtonTempText = "Temporarly check visitor out";
                            ButtonInvalidText = "Permanently check visitor out";
                            _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        }
                        else if (Loans.Where(l => l.IsOverdue).Count() > 0)
                        {
                            int days;
                            double pricePerDay;
                            int daysOverdue;
                            double extra;
                            double totalExtra = 0;
                            foreach (Loan loan in Loans)
                            {
                                if (loan.IsOverdue)
                                {
                                    days = ((TimeSpan)(loan.EndDate - loan.StartDate)).Days;
                                    pricePerDay = (loan.Total / loan.Qauntity) / days;
                                    daysOverdue = ((TimeSpan)(DateTime.Now - loan.EndDate)).Days;
                                    extra = pricePerDay * daysOverdue*loan.Qauntity;
                                    totalExtra += extra;
                                }
                            }

                            if (Visitor.Balance >= totalExtra)
                            {
                                Display = new Display(Brushes.Orange, $"Visitor must return overdue items.\n Must pay:{totalExtra}", "", false, false);
                                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                                ButtonTempText = "Return items & Temporarly check visitor out";
                                ButtonInvalidText = "Return items & Permanently check visitor out";
                            }
                            else
                            {
                                //Start timer but increase the interval
                                Display = new Display(Brushes.Red, $"Visitor does not have enough funds to pay for overdue items.\n Must pay:{totalExtra}", "", false, false);
                                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                                ButtonTempText = "";
                                ButtonInvalidText = "";
                                _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 10);
                                _mainViewModel.ResetTimer.Start();
                            }

                        }
                        else
                        {
                            Display = new Display(Brushes.Black, "Visitor must return items before checking out", "", false, false);
                            ButtonTempText = "Return items & Temporarly check visitor out";
                            ButtonInvalidText = "Return items & Permanently check visitor out";
                        }
                    }
                    else
                    {
                        Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
                        _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 1);
                        _mainViewModel.ResetTimer.Start();
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

        private RelayCommand _checkOutTempCommand;
        public RelayCommand CheckOutTempCommand
        {

            get
            {
                if (_checkOutTempCommand == null) _checkOutTempCommand = new RelayCommand(new Action<object>(CheckOutTemp));
                return _checkOutTempCommand;
            }

        }

        private void CheckOutTemp(object obj)
        {
            if (ButtonTempText == "Temporarly check visitor out")
            {
                try
                {
                    dh.TemporaryCheckOut(Visitor.TicketNr);
                    Display = new Display(Brushes.Green, "Visitor succesfully temporarly checked out", "", false, false);
                }
                catch
                {
                    Display = new Display(Brushes.Red, "Visitor checkout unsuccesfully", "", false, false);
                }
                finally
                {
                    _mainViewModel.ResetTimer.Start();
                }
            }
            else if (ButtonTempText == "Return items & Temporarly check visitor out")
            {
                //TODO What happens when visitor has overdue items
                try
                {
                    dh.ReturnLoanedItems(Loans.ToList(), Visitor);
                    dh.TemporaryCheckOut(Visitor.TicketNr);
                    Visitor = _mainViewModel.dataHelper.GetVisitor(Visitor.TicketNr);
                    _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 10);
                    _mainViewModel.ResetTimer.Start();
                }
                catch
                {

                }
            }
        }

        private RelayCommand _checkOutMarkInvalidCommand;
        public RelayCommand CheckOutMarkInvalidCommand
        {

            get
            {
                if (_checkOutMarkInvalidCommand == null) _checkOutMarkInvalidCommand = new RelayCommand(new Action<object>(CheckOutMarkInvalid));
                return _checkOutMarkInvalidCommand;
            }

        }

        private void CheckOutMarkInvalid(object obj)
        {
            if (ButtonInvalidText == "Permanently check visitor out")
            {
                try
                {
                    dh.MarkVisitorAccountAsInvalid(Visitor.TicketNr);
                    Display = new Display(Brushes.Green, "Visitor succesfully Permanently checked out", "", false, false);
                }
                catch
                {
                    Display = new Display(Brushes.Red, "Visitor checkout unsuccesfully", "", false, false);
                }
                finally
                {
                    _mainViewModel.ResetTimer.Start();
                }
            }
            else if (ButtonInvalidText == "Return items & Permanently check visitor out")
            {
                //TODO What happens when visitor has overdue items
                try
                {
                    dh.ReturnLoanedItems(Loans.ToList(), Visitor);
                    dh.MarkVisitorAccountAsInvalid(Visitor.TicketNr);
                    Visitor = _mainViewModel.dataHelper.GetVisitor(Visitor.TicketNr);
                    Display = new Display(Brushes.Green, $"Visitor succesfully Permantly checked out,\nReturn amount:{Visitor.Balance}", "", false, false);
                    _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 10);
                    _mainViewModel.ResetTimer.Start();
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show("Test");
                }
               
            }
        }

        private void Reset(object sender, EventArgs e)
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID", "", true, false);
            Loans = new List<Loan>();
            Visitor = null;
            _mainViewModel.ResetTimer.Stop();
            if (_mainViewModel.ResetTimer.Interval != new TimeSpan(0, 0, 3))
            {
                _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            }
            try
            {
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false); }
        }
    }
}
