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
                empNr = Convert.ToInt32(((TextBox)obj).Text);
                _mainViewModel.dataModel.SelectedEmployee = _mainViewModel.dataHelper.GetEmployee(empNr);
                _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Employee);
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

                        if(_mainViewModel.dataHelper.AddEmployee(NewFirstName, NewLastName, SelectedJob.Id, "1111") != -1)
                        {
                            NewFirstName = null;
                            NewLastName = null;
                            SelectedJob = null;
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
    }
}
