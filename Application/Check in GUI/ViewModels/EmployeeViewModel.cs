using EventManager.Models;
using EventManager.Objects;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class EmployeeViewModel : ObservableObject, IPageViewModel
    {
        public MainViewModel _mainViewModel { get; set; }

        private Job _selecetedJob;
        public Job SelectedJob
        {
            get
            {
                return _selecetedJob;
            }
            set
            {
                if(_selecetedJob != value)
                {
                    _selecetedJob = value;
                    if (SelectedJob.Id != _mainViewModel.dataModel.SelectedEmployee.JobId)
                    {
                        CanChangeJob = true;
                    }
                    else
                    {
                        CanChangeJob = false;
                    }
                }
                _selecetedJob = value;
                OnPropertyChanged("SelectedJob");
            }
        }

        private List<Shop> _empShops;
        public IEnumerable<Shop> EmpShops
        {
            get
            {
                List<Shop> temp = _mainViewModel.dataModel.Shops.ToList();
                foreach (Shop s in _mainViewModel.dataModel.Shops)
                {
                    foreach (Shop es in _empShops)
                    {
                        if (s.ID == es.ID)
                        {
                            temp.Remove(s);
                        }
                    }
                }
                return temp;
            }
            set
            {
                _empShops = value.ToList();
                OnPropertyChanged("EmpShops");
                OnPropertyChanged("TheShops");
            }
        }
        public IEnumerable<Shop> TheShops
        {
            get
            {
                return _empShops;
            }
        }

        private List<LoanStand> _empLoanStands;
        public IEnumerable<LoanStand> EmpLoanStands
        {
            get
            {
                List<LoanStand> temp = _mainViewModel.dataModel.LoanStands.ToList();
                foreach (LoanStand l in _mainViewModel.dataModel.LoanStands)
                {
                    foreach (LoanStand el in _empLoanStands)
                    {
                        if (l.ID == el.ID)
                        {
                            temp.Remove(l);
                        }
                    }
                }
                return temp;
            }
            set
            {
                _empLoanStands = value.ToList();
                OnPropertyChanged("EmpLoanStands");
                OnPropertyChanged("TheLoanStands");
            }
        }
        public IEnumerable<LoanStand> TheLoanStands
        {
            get
            {
                return _empLoanStands;
            }
        }

        private bool _canChangeJob;
        public bool CanChangeJob
        {
            get
            {
                return _canChangeJob;
            }
            set
            {
                _canChangeJob = value;
                OnPropertyChanged("CanChangeJob");
            }
        }
        public EmployeeViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        private RelayCommand _removeEmployeeCommand;
        public RelayCommand RemoveEmployeeCommand
        {

            get
            {
                if (_removeEmployeeCommand == null) _removeEmployeeCommand = new RelayCommand(new Action<object>(RemoveEmployee));
                return _removeEmployeeCommand;
            }

        }

        private RelayCommand _removeShopCommand;
        public RelayCommand RemoveShopCommand
        {

            get
            {
                if (_removeShopCommand == null) _removeShopCommand = new RelayCommand(new Action<object>(RemoveShop));
                return _removeShopCommand;
            }

        }

        private RelayCommand _addShopCommand;
        public RelayCommand AddShopCommand
        {
            get
            {
                if (_addShopCommand == null) _addShopCommand = new RelayCommand(new Action<object>(AddShop));
                return _addShopCommand;
            }
        }

        private void RemoveShop(object obj)
        {
            Shop temp = (Shop)obj;
            _mainViewModel.dataHelper.RemoveShopFromEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr, temp.ID);
            EmpShops = _mainViewModel.dataHelper.GetEmployeeShops(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);

        }

        private void AddShop(object obj)
        {
            Shop temp = (Shop)obj;
            _mainViewModel.dataHelper.AddShopToEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr, temp.ID);
            EmpShops = _mainViewModel.dataHelper.GetEmployeeShops(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
        }


        private RelayCommand _removeLoanStandCommand;
        public RelayCommand RemoveLoanStandCommand
        {

            get
            {
                if (_removeLoanStandCommand == null) _removeLoanStandCommand = new RelayCommand(new Action<object>(RemoveLoanStand));
                return _removeLoanStandCommand;
            }

        }

        private RelayCommand _addLoanStandCommand;
        public RelayCommand AddLoanStandCommand
        {
            get
            {
                if (_addLoanStandCommand == null) _addLoanStandCommand = new RelayCommand(new Action<object>(AddLoanStand));
                return _addLoanStandCommand;
            }
        }

        private void RemoveLoanStand(object obj)
        {
            LoanStand temp = (LoanStand)obj;
            _mainViewModel.dataHelper.RemoveLoanstandFromEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr, temp.ID);
            EmpLoanStands = _mainViewModel.dataHelper.GetEmployeeLoanStands(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);

        }

        private void AddLoanStand(object obj)
        {
            LoanStand temp = (LoanStand)obj;
            _mainViewModel.dataHelper.AddLoanStandToEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr, temp.ID);
            EmpLoanStands = _mainViewModel.dataHelper.GetEmployeeLoanStands(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
        }


        private RelayCommand _changeJobIdCommand;
        public RelayCommand ChangeJobIdCommand
        {

            get
            {
                if (_changeJobIdCommand == null) _changeJobIdCommand = new RelayCommand(new Action<object>(ChangeJobId));
                return _changeJobIdCommand;
            }

        }

        private void RemoveEmployee(object obj)
        {
            _mainViewModel.dataHelper.DeleteEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
            _mainViewModel.dataModel.SelectedEmployee = null;
            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Admin);
        }

        private void ChangeJobId(object obj)
        {
            
            try
            {
                _mainViewModel.dataHelper.ChangeJobId(SelectedJob, _mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
                _mainViewModel.dataModel.SelectedEmployee = _mainViewModel.dataHelper.GetEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Test");
            }
            
        }

        public void Start()
        {
            foreach(Job j in _mainViewModel.dataModel.Jobs)
            {
                if(j.Id == _mainViewModel.dataModel.SelectedEmployee.JobId)
                {
                    SelectedJob = j;
                }
            }
            CanChangeJob = false;
            if (_mainViewModel.dataModel.SelectedEmployee.JobId.Contains("s"))
            {
                EmpShops = _mainViewModel.dataHelper.GetEmployeeShops(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
            }
            else
            {
                _empShops = new List<Shop>();
            }
            if (_mainViewModel.dataModel.SelectedEmployee.JobId.Contains("l"))
            {
                EmpLoanStands = _mainViewModel.dataHelper.GetEmployeeLoanStands(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
            }
            else
            {
                _empLoanStands = new List<LoanStand>();
            }
        }
    }
}
