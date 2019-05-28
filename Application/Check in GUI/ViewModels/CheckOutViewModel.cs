﻿using EventManager.Models;
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


        public void Start()
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
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
                            ButtonInvalidText = "Permantly check visitor out";
                            myRFIDReader.AntennaEnabled = false;
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
                                    extra = pricePerDay * daysOverdue;
                                    totalExtra += extra;
                                }
                            }

                            if (Visitor.Balance >= totalExtra)
                            {
                                Display = new Display(Brushes.Orange, "Visitor must return overdue items", "", false, false);
                                myRFIDReader.AntennaEnabled = false;
                                ButtonTempText = "Return items & Temporarly check visitor out";
                                ButtonInvalidText = "Return items & Permantly check visitor out";
                            }
                            else
                            {
                                //Start timer but increase the interval
                                Display = new Display(Brushes.Red, "Visitor does not have enough funds to pay for overdue items", "", false, false);
                                myRFIDReader.AntennaEnabled = false;
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
                            ButtonInvalidText = "Return items & Permantly check visitor out";
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
                    Display = new Display(Brushes.Green, $"Visitor succesfully Permantly checked out, Return amount:{Visitor.Balance}", "", false, false);
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
            if (ButtonInvalidText == "Permantly check visitor out")
            {
                try
                {
                    dh.MarkVisitorAccountAsInvalid(Visitor.TicketNr);
                    Display = new Display(Brushes.Green, "Visitor succesfully Permantly checked out", "", false, false);
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
            else if (ButtonTempText == "Return items & Permantly check visitor out")
            {
                //TODO What happens when visitor has overdue items
                try
                {
                    dh.ReturnLoanedItems(Loans.ToList(), Visitor);
                    dh.MarkVisitorAccountAsInvalid(Visitor.TicketNr);
                    Visitor = _mainViewModel.dataHelper.GetVisitor(Visitor.TicketNr);
                    Display = new Display(Brushes.Green, $"Visitor succesfully Permantly checked out, Return amount:{Visitor.Balance}", "", false, false);
                    _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 10);
                    _mainViewModel.ResetTimer.Start();
                }
                catch
                {

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
            myRFIDReader.AntennaEnabled = true;
        }
    }
}
