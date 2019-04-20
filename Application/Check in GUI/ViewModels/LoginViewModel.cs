using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EventManager.ViewModels
{
    public class LoginViewModel : ObservableObject, IPageViewModel
    {
        MainViewModel _mainViewModel;
        private string _employeeNumber;

        private string realEmpNr = "real";
        private string realPassword = "pass";

        //
        private string _password;
        //

        //
        public string Password
        {
            get
            {
                if (_password == null) _password = "";
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        //
        public string EmployeeNumber
        {
            get
            {
                if (string.IsNullOrEmpty(_employeeNumber))
                {
                    return "";
                }

                return _employeeNumber;
            }

            set
            {
                _employeeNumber = value;
                OnPropertyChanged("EmployeeNumber");
            }
        }
        private RelayCommand _click_LoginCommand;
        public RelayCommand Click_LoginCommand
        {

            get
            {
                if (_click_LoginCommand == null) _click_LoginCommand = new RelayCommand(new Action<object>(Login));
                return _click_LoginCommand;
            }

        }

        private void Login(object parameter)
        {

            PasswordBox pwBox = (PasswordBox)parameter;
            Password = pwBox.Password.ToString();
            
            if(Password == realPassword && EmployeeNumber == realEmpNr)
            {
                _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Apps);
            }
            else
            {
                MessageBox.Show("Wrong usernam or password");
            }
        }
        public LoginViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
    }
}