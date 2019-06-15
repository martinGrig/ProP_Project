using EventManager.Objects;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EventManager.ViewModels
{
    public class AdminViewModel : ObservableObject, IPageViewModel
    {
        public MainViewModel _mainViewModel { get; set; }

        public AdminViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            
        }

        #region Properties
        private string _newFirstName;
        public string NewFirstName
        {
            get
            {
                return _newFirstName;
            }
            set
            {
                _newFirstName = value;
                OnPropertyChanged("NewFirstName");
            }
        }
        private string _newLastName;
        public string NewLastName
        {
            get
            {
                return _newLastName;
            }
            set
            {
                _newLastName = value;
                OnPropertyChanged("NewLastName");
            }
        }

        private string _searchNr;
        public string SearchNr
        {
            get
            {
                return _searchNr;
            }
            set
            {
                _searchNr = value;
                OnPropertyChanged("SearchNr");
                OnPropertyChanged("FilteredEmployees");
            }
        }
        private Employee _focusedEmployee;
        public Employee FocusedEmployee
        {
            get
            {
                return _focusedEmployee;
            }
            set
            {
                if(value != _focusedEmployee)
                {
                    if(value == null)
                    {
                    _focusedEmployee = null;
                    }
                    else
                    {
                        _focusedEmployee = value;
                        SearchNr = _focusedEmployee.EmployeeNr.ToString();
                    }
                }
                _focusedEmployee = value;
            }
        }
        private Job _selecetedJob;
        public Job SelectedJob
        {
            get
            {
                return _selecetedJob;
            }
            set
            {
                _selecetedJob = value;
                OnPropertyChanged("SelectedJob");
            }
        }

        private List<Employee> _employees;
        public IEnumerable<Employee> FilteredEmployees
        {
            get
            {
                if (SearchNr == null)
                {
                    return _employees;
                }
                if(_employees.Where(x => x.FirstName.ToUpper() == SearchNr.ToUpper() || x.LastName.ToUpper() == SearchNr.ToUpper() || x.EmployeeNr.ToString() == SearchNr.ToUpper() || x.JobDescription.ToUpper() == SearchNr.ToUpper()).Count() == 1)
                {
                    CanInspect = true;
                }
                else
                {
                    CanInspect = false;
                }
                return _employees.Where(x => x.FirstName.ToUpper().Contains(SearchNr.ToUpper()) || x.LastName.ToUpper().Contains(SearchNr.ToUpper())|| x.JobDescription.ToUpper().Contains(SearchNr.ToUpper()) || x.EmployeeNr.ToString().Contains(SearchNr.ToUpper()));
            }
        }

        private bool _canInspect;
        public bool CanInspect
        {
            get
            {
                return _canInspect;
            }
            set
            {
                _canInspect = value;
                OnPropertyChanged("CanInspect");
            }
        }

        private Shop _selectedShop;
        public Shop SelectedShop
        {
            get
            {
                return _selectedShop;
            }
            set
            {
                _selectedShop = value;
                OnPropertyChanged("SelectedShop");
            }
        }

        private LoanStand _selectedLoanStand;
        public LoanStand SelectedLoanStand
        {
            get
            {
                return _selectedLoanStand;
            }
            set
            {
                _selectedLoanStand = value;
                OnPropertyChanged("SelectedLoanStand");
            }
        }
        #endregion Properties
        private RelayCommand _inspectEmployeeCommand;
        public RelayCommand InspectEmployeeCommand
        {
            get
            {
                if (_inspectEmployeeCommand == null) _inspectEmployeeCommand = new RelayCommand(new Action<object>(Inspect));
                return _inspectEmployeeCommand;
            }
        }
        public void Inspect(object obj)
        {
            int empNr;
            try
            {
                _mainViewModel.dataModel.SelectedEmployee = _employees.Where(x => x.FirstName.ToUpper() == SearchNr.ToUpper() || x.LastName.ToUpper() == SearchNr.ToUpper() || x.EmployeeNr.ToString() == SearchNr.ToUpper() || x.JobDescription.ToUpper() == SearchNr.ToUpper()).ToList()[0];
                
                    _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Employee);
                    SearchNr = null;
              
                
            }
            catch (FormatException)
            {
                
            }
            
        }

        private RelayCommand _addEmployeeCommand;
        public RelayCommand AddEmployeeCommand
        {
            get
            {
                if (_addEmployeeCommand == null) _addEmployeeCommand = new RelayCommand(new Action<object>(AddEmployee));
                return _addEmployeeCommand;
            }
        }

        public void AddEmployee(object obj)
        {
            if(NewFirstName != null)
            {
                if (NewLastName != null)
                {
                    if (obj != null)
                    {

                        if(_mainViewModel.dataHelper.AddEmployee(NewFirstName, NewLastName, SelectedJob.Id, SelectedShop,SelectedLoanStand) != -1)
                        {
                            _employees = _mainViewModel.dataHelper.GetEmployees();
                            SearchNr = null;

                            NewFirstName = null;
                            NewLastName = null;
                            SelectedJob = null;
                            SelectedLoanStand = null;
                            SelectedShop = null;
                            _mainViewModel.PlaySound(Properties.Resources.correct);
                        }
                        else
                        {
                            //something went wrong
                            System.Windows.Forms.MessageBox.Show("Something went wrong with database");
                        }
                    }
                    else
                    {
                        //please select job
                        System.Windows.Forms.MessageBox.Show("no job selected");
                    }
                }
                else
                {
                    //pleas fill in last name
                    System.Windows.Forms.MessageBox.Show("Last name not selected");
                }
            }
            else
            {
                //please fill in first name
                System.Windows.Forms.MessageBox.Show("First name not selected");
            }
        }

        public void GetDatabaseStuff()
        {
            NewFirstName = null;
            NewLastName = null;
            _employees = _mainViewModel.dataHelper.GetEmployees();
            _mainViewModel.dataModel.LoanStands = _mainViewModel.dataHelper.GetLoanStands();
            _mainViewModel.dataModel.Shops = _mainViewModel.dataHelper.GetShops();
            _mainViewModel.dataModel.Jobs = _mainViewModel.dataHelper.GetJobs();
            SelectedJob = _mainViewModel.dataModel.Jobs.ToList()[0];
            CanInspect = true;
            SearchNr = "";
        }

        
    }
}
