using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class AdminViewModel : ObservableObject, IPageViewModel
    {
        private string _firstName;

        public string FirstName
        {
            get
            {
                if (string.IsNullOrEmpty(_firstName))
                {
                    return "";
                }

                return _firstName;
            }

            set
            {
                _firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
    }
}
