using EventManager.Models;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
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
                
                Dm.UserName = name;
                int numb = Convert.ToInt32(Dm.EmployeeNumber);

                string job = dh.GetEmployee(numb).JobId;
                if(job.Length == 1)
                {
                    Dm.ShowBackButton = false;
                    switch (job)
                    {
                        case "i":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.CheckIn);
                            break;
                        case "e":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Status);
                            break;
                        case "o":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.CheckOut);
                            break;
                        case "s":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Shop);
                            break;
                        case "c":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Camping);
                            break;
                        case "l":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.LoanStand);
                            break;
                        case "v":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Converter);
                            break;
                        case "a":
                            _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Admin);
                            break;
                    }
                }
                else if(job.Length > 1)
                {
                    Dm.ShowBackButton = true;
                    if (job.Contains("i"))
                    {
                        Dm.ShowCheckin = true;
                    }
                    if (job.Contains("e"))
                    {
                        Dm.ShowStatus = true;
                    }
                    if (job.Contains("o"))
                    {
                        Dm.ShowCheckout = true;
                    }
                    if (job.Contains("s"))
                    {
                        Dm.ShowShop = true;
                    }
                    if (job.Contains("c"))
                    {
                        Dm.ShowCamping = true;
                    }
                    if (job.Contains("l"))
                    {
                        Dm.ShowLoan = true;
                    }
                    if (job.Contains("v"))
                    {
                        Dm.ShowConverter = true;
                    }
                    if (job.Contains("a"))
                    {
                        Dm.ShowAdmin = true;
                    }
                    _mainViewModel.PlaySound(Properties.Resources.correct);
                    _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Apps);
                }
                

            }
            else
            {
                _mainViewModel.PlaySound(Properties.Resources.error);
                pwBox.Password = "";
            }
        }
        public LoginViewModel(MainViewModel mainViewModel)
        {

            _mainViewModel = mainViewModel;
            dh = new DataHelper();
            Dm = _mainViewModel.dataModel;
        }

        public void Start()
        {
            Dm = _mainViewModel.dataModel;
        }


    }
}