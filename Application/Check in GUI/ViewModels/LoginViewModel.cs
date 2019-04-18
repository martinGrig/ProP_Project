using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventManager.ViewModels
{
    public class LoginViewModel : ObservableObject, IPageViewModel
    {
        private string _employeeNumber;

        private string realEmpNr = "real";
        private string realPassword = "pass";
        
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

    }
}
