using EventManager.Models;
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
                _selecetedJob = value;
                OnPropertyChanged("SelectedJob");
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
        }
    }
}
