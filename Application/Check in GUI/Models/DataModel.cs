using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Models
{
    public class DataModel : ObservableObject
    {
        //Employee Number of selected user
        private string _employeeNumber;

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

        //Password entered by the user
        private string _password;
        
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

        //Text entered into the search bar of shop window when this text changes the filtered item list is updated
        private string _searchText;

        public string SearchText
        {
            get
            {
                if (_searchText == null)
                {
                    return _searchText;
                }

                return _searchText;
            }

            set
            {
                _searchText = value;
                OnPropertyChanged("SearchText");
                OnPropertyChanged("FilteredItems");
                OnPropertyChanged("Items");
            }
        }

        //Item list. it returns items based on wheter they contain the search text
        public List<Item> Items { get; set; }

        public IEnumerable<Item> FilteredItems
        {
            get
            {

                if (SearchText == null)
                {
                    return Items;
                }
                return Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()));

            }

        }

        //Selected items
        public List<Item> _selectedItems;

        public IEnumerable<Item> SelectedItems
        {
            get
            {
                if (_selectedItems == null)
                {
                    return _selectedItems;
                }
                return _selectedItems;
            }
            set
            {
                _selectedItems = value.ToList();
                OnPropertyChanged("SelectedItems");
            }
        }



    }
}
