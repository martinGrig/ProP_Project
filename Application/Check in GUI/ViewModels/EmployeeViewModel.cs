using EventManager.Models;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class EmployeeViewModel : IPageViewModel
    {
        public MainViewModel _mainViewModel { get; set; }

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

        private void RemoveEmployee(object obj)
        {
            _mainViewModel.dataHelper.DeleteEmployee(_mainViewModel.dataModel.SelectedEmployee.EmployeeNr);
            _mainViewModel.dataModel.SelectedEmployee = null;
            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Admin);
        }
    }
}
