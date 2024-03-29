﻿using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class StatusViewModel : ObservableObject, IPageViewModel
    {
        public MainViewModel _mainViewModel { get; private set; }
        private string _visitorStatus;
        private string _totalVisitors;
        private string _totalBalance;
        private string _spentMoney;
        private string _bookedCampingSpots;
        private string _freeCampingSpots;
        private string _amountEarnedPerShop;
        private string _amountEarnedPerItem;
        private string _amountEarnedPerLoanStand;
        private string _amountEarnedPerLanable;

        private Visitor _visitor;
        private List<string> _transactions;

        public Visitor Visitor { get { return _visitor; } private set { _visitor = value; OnPropertyChanged("VisitorStatus"); OnPropertyChanged("Visitor"); } }
        public string VisitorStatus
        {
            get
            {
                return _visitorStatus;
            }
            set
            {
                if (Visitor != null)
                {
                    if (_visitor.IsScanned == true)
                    {
                        _visitorStatus = "Checked In";
                    }
                    else
                    {
                        _visitorStatus = "Not checked in";
                    }
                    OnPropertyChanged("VisitorStatus");
                }

            }
        }
        public IEnumerable<string> Transactions
        {
            get
            {
                return _transactions;
            }
            private set
            {
                _transactions = value.ToList();
                OnPropertyChanged("Transactions");
            }
        }
        private int SortByDay(string first,string second)
        {
            return first.CompareTo(second);
        }
        public string TotalVisitors
        {
            get
            {
                return _totalVisitors;
            }
            private set
            {
                _totalVisitors = value;
                OnPropertyChanged("TotalVisitors");
            }
        }
        public string TotalBalance
        {
            get
            {
                return _totalBalance;
            }
            private set
            {
                _totalBalance = value;
                OnPropertyChanged("TotalBalance");
            }
        }
        public string SpentMoney
        {
            get
            {
                return _spentMoney;
            }
            private set
            {
                _spentMoney = value;
                OnPropertyChanged("SpentMoney");
            }
        }
        public string BookedCampingSpots
        {
            get
            {
                return _bookedCampingSpots;
            }
            private set
            {
                _bookedCampingSpots = value;
                OnPropertyChanged("BookedCampingSpots");
            }
        }
        public string FreeCampingSpots
        {
            get
            {
                return _freeCampingSpots;
            }
            private set
            {
                _freeCampingSpots = value;
                OnPropertyChanged("FreeCampingSpots");
            }
        }
        public string AmountEarnedPerShop
        {
            get
            {
                return _amountEarnedPerShop;
            }
            private set
            {
                _amountEarnedPerShop = value;
                OnPropertyChanged("AmountEarnedPerShop");
            }
        }
        public string AmountEarnedPerLoanStand
        {
            get
            {
                return _amountEarnedPerLoanStand;
            }
            private set
            {
                _amountEarnedPerLoanStand = value;
                OnPropertyChanged("AmountEarnedPerLoanStand");
            }
        }
        public string AmountEarnedPerItem
        {
            get
            {
                return _amountEarnedPerItem;
            }
            private set
            {
                _amountEarnedPerItem = value;
                OnPropertyChanged("AmountEarnedPerItem");
            }
        }
        public string AmountEarnedPerLoanable
        {
            get
            {
                return _amountEarnedPerLanable;
            }
            private set
            {
                _amountEarnedPerLanable = value;
                OnPropertyChanged("AmountEarnedPerLoanable");
            }
        }

        public StatusViewModel(MainViewModel model)
        {
            _mainViewModel = model;
        }
        private RelayCommand _FindVisitorCommand;
        public RelayCommand FindVisitorCommand
        {

            get
            {
                if (_FindVisitorCommand == null) _FindVisitorCommand = new RelayCommand(new Action<object>(FindVisitor));
                return _FindVisitorCommand;
            }

        }
        private RelayCommand _GetShopEarningsCommand;
        public RelayCommand GetShopEarningsCommand
        {

            get
            {
                if (_GetShopEarningsCommand == null) _GetShopEarningsCommand = new RelayCommand(new Action<object>(FindShop));
                return _GetShopEarningsCommand;
            }

        }
        private RelayCommand _GetLoanStandEarningsCommand;
        public RelayCommand GetLoanStandEarningsCommand
        {

            get
            {
                if (_GetLoanStandEarningsCommand == null) _GetLoanStandEarningsCommand = new RelayCommand(new Action<object>(FindLoanStand));
                return _GetLoanStandEarningsCommand;
            }

        }
        private RelayCommand _GetItemEarningsCommand;
        public RelayCommand GetItemEarningsCommand
        {

            get
            {
                if (_GetItemEarningsCommand == null) _GetItemEarningsCommand = new RelayCommand(new Action<object>(FindItem));
                return _GetItemEarningsCommand;
            }

        }
        private RelayCommand _GetLoanableEarningsCommand;
        public RelayCommand GetLoanableEarningsCommand
        {

            get
            {
                if (_GetLoanableEarningsCommand == null) _GetLoanableEarningsCommand = new RelayCommand(new Action<object>(FindLoanable));
                return _GetLoanableEarningsCommand;
            }

        }
        private void FindVisitor(object o)
        {
            try
            {
                Visitor = _mainViewModel.dataHelper.GetVisitor(Convert.ToInt32(o));
                Transactions = _mainViewModel.dataHelper.GetTransactions(Visitor.TicketNr);
                OnPropertyChanged("VisitorStatus");
                VisitorStatus = "";
            }
            catch
            {

            }

        }
        private void FindShop(object o)
        {
            try
            {
                AmountEarnedPerShop = _mainViewModel.dataHelper.AmountEarnedPerShop(Convert.ToInt32(o)).ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }

        }
        private void FindLoanStand(object o)
        {
            try
            {
                AmountEarnedPerLoanStand = _mainViewModel.dataHelper.AmountEarnedPerLoanStand(Convert.ToInt32(o)).ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }
        }
        private void FindItem(object o)
        {
            try
            {
                AmountEarnedPerItem = _mainViewModel.dataHelper.AmountEarnedPerItem(Convert.ToInt32(o)).ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }
        }
        private void FindLoanable(object o)
        {
            try
            {
                AmountEarnedPerLoanable = _mainViewModel.dataHelper.AmountEarnedPerLoanable(Convert.ToInt32(o)).ToString();
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }
        }
        public void Start()
        {
            _mainViewModel.ResetTimer.Start();
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 30);
            TotalVisitors = _mainViewModel.dataHelper.GetAllVisitors().ToString();
            TotalBalance = _mainViewModel.dataHelper.SumOfAllVisitorBalance().ToString();
            SpentMoney = _mainViewModel.dataHelper.TotalMoneySpentByVisitor().ToString();
            BookedCampingSpots = _mainViewModel.dataHelper.AmountOfBookedCampingSpots().ToString();
            FreeCampingSpots = _mainViewModel.dataHelper.AmountOfFreeCampSpaces().ToString();
            Visitor = null;
            AmountEarnedPerItem = null;
            AmountEarnedPerLoanable = null;
            AmountEarnedPerLoanStand = null;
            AmountEarnedPerShop = null;
            VisitorStatus = "";
            Transactions = new List<string>();

        }
        private void Reset(object sender, EventArgs eventArgs)
        {
            if (_mainViewModel.isConnected)
            {
                TotalVisitors = _mainViewModel.dataHelper.GetAllVisitors().ToString();
                TotalBalance = _mainViewModel.dataHelper.SumOfAllVisitorBalance().ToString();
                SpentMoney = _mainViewModel.dataHelper.TotalMoneySpentByVisitor().ToString();
                BookedCampingSpots = _mainViewModel.dataHelper.AmountOfBookedCampingSpots().ToString();
                FreeCampingSpots = _mainViewModel.dataHelper.AmountOfFreeCampSpaces().ToString();
            }

        }
    }
}
