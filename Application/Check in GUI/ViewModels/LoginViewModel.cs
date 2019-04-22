using EventManager.Models;
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
        DataHelper dh;
        public DataModel Dm{ get;  set; }
        MainViewModel _mainViewModel;
        
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
            Dm.Password = pwBox.Password.ToString();
            string name = dh.Login(Convert.ToInt32(Dm.EmployeeNumber), Dm.Password);
            if( name != null)
            {
                MessageBox.Show("Welcome" + name);
                int numb = Convert.ToInt32(Dm.EmployeeNumber);
                Dm.Items = dh.GetItems(numb);
                _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Shop);
            }
            else
            {
                MessageBox.Show("Wrong usernam or password");
            }
        }
        public LoginViewModel(MainViewModel mainViewModel, DataModel dataModel)
        {

            _mainViewModel = mainViewModel;
            dh = new DataHelper();
            Dm = dataModel;
        }
    }
}