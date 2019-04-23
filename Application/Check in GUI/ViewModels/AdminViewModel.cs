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
        MainViewModel _mainViewModel;

        public AdminViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

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
    }
}
