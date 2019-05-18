using EventManager.Objects;
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
                OnPropertyChanged("FilteredFood");
                OnPropertyChanged("FilteredDrink");
                OnPropertyChanged("FilteredItems");
                OnPropertyChanged("Items");
            }
        }

        private int _selectedTabIndex;
        public int SelectedTabIndex
        {
            get
            {
                return _selectedTabIndex;
            }
            set
            {
                if (_selectedTabIndex != value)
                {
                    _selectedTabIndex = value;
                    OnPropertyChanged("SelectedTabIndex");
                }
                
            }
        }

        //Item list. it returns items based on wheter they contain the search text
        public List<ShopItem> Items { get; set; }

        public IEnumerable<ShopItem> FilteredItems
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

        public IEnumerable<ShopItem> FilteredFood
        {
            get
            {

                if (SearchText == null)
                {
                    return Items.Where(x => x.IsFood);
                }
                if(Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && x.IsFood).Count() == 0  && Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && !x.IsFood).Count() > 0)
                {
                    SelectedTabIndex = 1;
                }
                return Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && x.IsFood);

            }
        }

        public IEnumerable<ShopItem> FilteredDrink
        {
            get
            {

                if (SearchText == null)
                {
                    return Items.Where(x => !x.IsFood);
                }
                if (Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && !x.IsFood).Count() == 0 && Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && x.IsFood).Count() > 0)
                {
                    SelectedTabIndex = 0;
                }
                return Items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()) && !x.IsFood);

            }
        }

        private List<Shop> _shops;
        public IEnumerable<Shop> Shops
        {
            get
            {
                return _shops;
            }

            set
            {
                _shops = value.ToList();
                OnPropertyChanged("Shops");
            }
        }

        //Selected items
        public List<ShopItem> _selectedItems;

        public IEnumerable<ShopItem> SelectedItems
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

        public List<Job> _jobs;

        public IEnumerable<Job> Jobs
        {
            get
            {
                return _jobs;
            }

            set
            {
                _jobs = value.ToList();
                OnPropertyChanged("Jobs");
            }
        }

        private Employee _selectedEmployee;

        public Employee SelectedEmployee
        {
            get
            {
                if (_selectedEmployee == null)
                {
                    return _selectedEmployee;
                }
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        private Visitor _selectedVisitor;
        public Visitor SelectedVisitor
        {
            get
            {
                return _selectedVisitor;
            }

            set
            {
                _selectedVisitor = value;
                OnPropertyChanged("SelectedVisitor");
            }
        }

        private CampingSpot _selectedCampingSpot;
        public CampingSpot SelectedCampingSpot
        {
            get
            {
                return _selectedCampingSpot;
            }
            set
            {
                _selectedCampingSpot = value;
                OnPropertyChanged("SelectedCampingSpot");
            }
        }
        public List<LogLine> _logFileLines;
        public IEnumerable<LogLine> LogFileLines
        {
            get
            {
                return _logFileLines;
            }

            set
            {
                _logFileLines = value.ToList();
                OnPropertyChanged("LogFileLines");
            }
        }
        public List<LogLine> _logFileLinesInfo;
        public IEnumerable<LogLine> LogFileLinesInfo
        {
            get
            {
                return _logFileLinesInfo;
            }

            set
            {
                _logFileLinesInfo = value.ToList();
                OnPropertyChanged("LogFileLinesInfo");
            }
        }

        private string _userName;

        public string UserName
        {
            get
            {
                if (_userName == null)
                {
                    return "Log-in screen";
                }
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private bool _showCheckIn;
        public bool ShowCheckin
        {
            get
            {

                return _showCheckIn;
            }
            set
            {
                _showCheckIn = value;
                OnPropertyChanged("ShowCheckIn");
            }
        }

        private bool _showCamping;
        public bool ShowCamping
        {
            get
            {
                return _showCamping;
            }
            set
            {
                _showCamping = value;
                OnPropertyChanged("ShowCamping");
            }
        }

        private bool _showStatus;
        public bool ShowStatus
        {
            get
            {
                return _showStatus;
            }
            set
            {
                _showStatus = value;
                OnPropertyChanged("ShowStatus");
            }
        }

        private bool _showLoan;
        public bool ShowLoan
        {
            get
            {
                return _showLoan;
            }
            set
            {
                _showLoan = value;
                OnPropertyChanged("ShowLoan");
            }
        }
        private bool _showCheckout;
        public bool ShowCheckout
        {
            get
            {
                return _showCheckout;
            }
            set
            {
                _showCheckout = value;
                OnPropertyChanged("ShowCheckout");
            }
        }
        private bool _showConverter;
        public bool ShowConverter
        {
            get
            {
                return _showConverter;
            }
            set
            {
                _showConverter = value;
                OnPropertyChanged("ShowConverter");
            }
        }
        private bool _showShop;
        public bool ShowShop
        {
            get
            {
                return _showShop;
            }
            set
            {
                _showShop = value;
                OnPropertyChanged("ShowShop");
            }
        }
        private bool _showAdmin;
        public bool ShowAdmin
        {
            get
            {
                return _showAdmin;
            }
            set
            {
                _showAdmin = value;
                OnPropertyChanged("ShowAdmin");
            }
        }

        private bool _showScanQrCode;
        public bool ShowScanQrCode
        {
            get
            {
                return _showScanQrCode;
            }
            set
            {
                _showScanQrCode = value;
                OnPropertyChanged("ShowScanQrCode");
            }
        }

        private bool _showBackButton;
        public bool ShowBackButton
        {
            get
            {
                return _showBackButton;
            }
            set
            {
                _showBackButton = value;
                OnPropertyChanged("ShowBackButton");
            }
        }





    }
}
