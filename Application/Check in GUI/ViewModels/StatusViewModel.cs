using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class StatusViewModel : ObservableObject, IPageViewModel
    {
        MainViewModel _mainViewModel;
        private string _visitorStatus;
        private string _totalVisitors;
        private string _totalBalance;
        private string _spentMoney;
        private Visitor _visitor;
        public Visitor Visitor { get { return _visitor; }private set { _visitor = value; OnPropertyChanged("VisitorStatus"); OnPropertyChanged("Visitor"); } }
        public string VisitorStatus
        {
            get
            {
                return _visitorStatus;
            }
            set
            {
                if(_visitor.IsScanned == true)
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
        private void FindVisitor(object o)
        {
            Visitor = _mainViewModel.dataHelper.GetVisitor(Convert.ToInt32(o)); ;
            OnPropertyChanged("VisitorStatus");
            VisitorStatus = "";
        }
        public void Start()
        {
            TotalVisitors = _mainViewModel.dataHelper.GetAllVisitors().ToString();
            TotalBalance = _mainViewModel.dataHelper.SumOfAllVisitorBalance().ToString();
            SpentMoney = _mainViewModel.dataHelper.TotalMoneySpentByVisitor().ToString();
        }
    }
}
