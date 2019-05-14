using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Security.Cryptography;

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
                if (_mainViewModel.dataModel.SelectedEmployee != null)
                {
                    _mainViewModel.ChangePageCommand.Execute(_mainViewModel.Employee);
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("There is no employee with that name");
                }
                
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

                        if(_mainViewModel.dataHelper.AddEmployee(NewFirstName, NewLastName, SelectedJob.Id, GetRandomAlphanumericString(6)) != -1)
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

        public static string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                //"ABCDEFGHJKMNPQRSTUVWXYZ" +
                "abcdefghjkmnpqrstuvwxyz" +
                "23456789";
            return GetRandomString(length, alphanumericCharacters);
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
    }
}
