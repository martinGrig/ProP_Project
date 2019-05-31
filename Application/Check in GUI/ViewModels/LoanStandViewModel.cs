using EventManager.Models;
using EventManager.Objects;
using EventManager.ViewModels.Commands;
using Phidget22;
using Phidget22.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EventManager.ViewModels
{
    public class LoanStandViewModel : ObservableObject, IPageViewModel
    {

        #region Properties
        private List<LoanStandItem> _loanItems { get; set; }

        public IEnumerable<LoanStandItem> FilteredLoanItems
        {
            get
            {

                if (SearchText == null)
                {
                    return _loanItems;
                }
                return _loanItems.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()));

            }

        }
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
                OnPropertyChanged("FilteredLoanItems");
                OnPropertyChanged("_loanItems");
            }
        }

        public List<LoanStandItem> _selectedItems;

        public IEnumerable<LoanStandItem> SelectedItems
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

        private List<Loan> _loans;
        public IEnumerable<Loan> Loans
        {
            get
            {
                return _loans;
            }
            set
            {
                _loans = value.ToList();
                OnPropertyChanged("Loans");
            }
        }

        private Display _display;
        public Display Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged("Display");
            }
        }

        private List<LoanStand> _loanStands;
        public IEnumerable<LoanStand> LoanStands
        {
            get
            {
                return _loanStands;
            }
            set
            {
                _loanStands = value.ToList();
                OnPropertyChanged("LoanStands");
            }
        }

        private LoanStand _selectedLoanStand;
        public LoanStand SelectedLoanStand
        {
            get
            {
                return _selectedLoanStand;
            }
            set
            {
                if(_selectedLoanStand != value)
                {
                    _selectedLoanStand = value;
                    Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
                    _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
                    SearchText = "";
                }
                _selectedLoanStand = value;
                OnPropertyChanged("SelectedLoanStand");
            }
        }

        private bool _canChangeLoanStand;
        public bool CanChangeLoanStand
        {
            get
            {
                return _canChangeLoanStand;
            }
            set
            {
                _canChangeLoanStand = value;
                OnPropertyChanged("CanChangeLoanStand");
            }
        }
        #endregion


        Visitor _visitor;
        public Visitor Visitor
        {
            get
            {
                return _visitor;
            }
            set
            {
                _visitor = value;
                OnPropertyChanged("Visitor");
            }
        }
        public DataModel Dm { get; set; }
        DataHelper dh;
        public MainViewModel _mainViewModel { get; set; }
        private List<string> _receipt;

        public IEnumerable<string> Receipt
        {
            get
            {
                if (_receipt == null)
                {
                    return new List<string>();
                }
                return _receipt;
            }
            set
            {
                _receipt = value.ToList();
                OnPropertyChanged("Receipt");
            }
        }

        public LoanStandViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Dm = _mainViewModel.dataModel;
            dh = new DataHelper();
            _receipt = new List<string>();
            _loans = new List<Loan>();
            _selectedItems = new List<LoanStandItem>();
            _loanStands = new List<LoanStand>();

        }

        // Commands 
        #region Commands
        private RelayCommand _selectItem;
        public RelayCommand SelectItemCommand
        {
            get
            {
                if (_selectItem == null)
                {
                    _selectItem = new RelayCommand(new Action<object>(SelectItem));
                }
                return _selectItem;
            }
        }

        private RelayCommand _returnAllItem;
        public RelayCommand ReturnAllItem
        {
            get
            {
                if (_returnAllItem == null)
                {
                    _returnAllItem = new RelayCommand(new Action<object>(ReturnAll));
                }
                return _returnAllItem;
            }
        }
        private RelayCommand _returnSelectedItem;
        public RelayCommand ReturnSelectedItem
        {
            get
            {
                if (_returnSelectedItem == null)
                {
                    _returnSelectedItem = new RelayCommand(new Action<object>(ReturnSelected));
                }
                return _returnSelectedItem;
            }
        }
        #endregion

        private void ReturnAll(object obj)
        {
            try
            {
                dh.ReturnLoanedItems(Loans.ToList(), Visitor);
                if (SelectedItems.Count() > 0)
                {
                    _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LoanItems);
                }
                Reset();
                Display = new Display(Brushes.Green, "Items successfully returned", "check", false, true);
                _mainViewModel.ResetTimer.Start();
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                SearchText = "";
                Visitor = null;
            }
            catch
            {

            }
        }

        private RelayCommand _clearVisitorCommand;
        public RelayCommand ClearVisitorCommand
        {
            get
            {
                if (_clearVisitorCommand == null) _clearVisitorCommand = new RelayCommand(new Action<object>(ClearEmployee));
                return _clearVisitorCommand;
            }
        }

        private void ClearEmployee(object obj)
        {
            Reset(null, null);
        }

        private void ReturnSelected(object obj)
        {
            try
            {
                System.Collections.IList items = (System.Collections.IList)obj;
                var temp = items.Cast<Loan>().ToList();
                if (SelectedItems.Count() > 0)
                {
                    _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LoanItems);
                }
                dh.ReturnLoanedItems(temp, Visitor);
                Reset();
                Display = new Display(Brushes.Green, "Items successfully returned", "check", false, true);
                _mainViewModel.ResetTimer.Start();
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                SearchText = "";
                
                Visitor = null;
            }
            catch
            {

            }
        }

        public void Start()
        {
            CanChangeLoanStand = true;
            LoanStands = dh.GetEmployeeLoanStands(Convert.ToInt32(Dm.EmployeeNumber));
            SelectedLoanStand = _loanStands[0];
            Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            try
            {
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);

                _mainViewModel._MyRFIDReader.Open();
            }
            catch (PhidgetException) { System.Windows.Forms.MessageBox.Show("Please connect rfid reader"); }
        }

        private void SelectItem(object obj)
        {
            LoanStandItem item = ((LoanStandItem)obj);
            if (Visitor != null)
            {
                if (Loans.Where(l => l.IsOverdue).Count() == 0)
                {
                    if (SelectedItems.Count() == 0)
                    {
                        Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet to complete trasaction", "", true, false);
                        _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                        _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(LoanItems);
                    }
                    //Check if there is already a item with that name and  if there is select the item to update its properties
                    foreach (LoanStandItem it in SelectedItems)
                    {
                        if (it.Name == item.Name)
                        {
                            it.SelectItem();
                            SelectedItems = _selectedItems;
                            return;
                        }
                    }
                    if (item.Stock != 0)
                    {
                        item.SelectItem();
                        _selectedItems.Add(item);
                        SelectedItems = _selectedItems;
                    }

                }
            }



        }

        private RelayCommand _unselectItemCommand;
        public RelayCommand UnselectItemCommand
        {
            get
            {
                if (_unselectItemCommand == null)
                {
                    _unselectItemCommand = new RelayCommand(new Action<object>(UnselectItem));
                }
                return _unselectItemCommand;
            }
        }


        private void UnselectItem(object obj)
        {
            LoanStandItem item = ((LoanStandItem)obj);
            if (Visitor != null)
            {

                item.UnselectItem();
                SelectedItems = _selectedItems;
                if (item.Quantity == 0)
                {
                    _selectedItems.Remove(item);
                    SelectedItems = _selectedItems;
                }
                if (SelectedItems.Count() == 0)
                {
                    Display = new Display(Brushes.Black, "Select items", "", false, false);
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                    _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LoanItems);
                }

            }
        }

        private RelayCommand _increaseDaysCommand;
        public RelayCommand IncreaseDaysCommand
        {
            get
            {
                if (_increaseDaysCommand == null)
                {
                    _increaseDaysCommand = new RelayCommand(new Action<object>(IncreaseDays));
                }
                return _increaseDaysCommand;
            }
        }

        private void IncreaseDays(object obj)
        {
            LoanStandItem item = ((LoanStandItem)obj);
            if (_visitor != null)
            {

                item.IncreaseDays();
                SelectedItems = _selectedItems;

            }
        }

        private RelayCommand _decreaseDaysCommand;
        public RelayCommand DecreaseDaysCommand
        {
            get
            {
                if (_decreaseDaysCommand == null)
                {
                    _decreaseDaysCommand = new RelayCommand(new Action<object>(DecreaseDays));
                }
                return _decreaseDaysCommand;
            }
        }

        private void DecreaseDays(object obj)
        {
            LoanStandItem item = ((LoanStandItem)obj);
            if (Visitor != null)
            {

                item.DecreaseDays();
                SelectedItems = _selectedItems;

            }
        }

        public void LoanItems(object sender, RFIDTagEventArgs e)
        {
            try
            {

                if (Visitor.TicketNr == _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString()).TicketNr)
                {
                    try
                    {
                        _mainViewModel.dataHelper.StartLoan(Visitor, SelectedItems.ToList(), SelectedLoanStand.ID);
                        Display = new Display(Brushes.Green, "Transaction Complete", "check", false, true);
                        //_receipt.Add("Shop: blablabla");
                        //_receipt.Add($"Customer: {temp.FirstName} {temp.LastName}");
                        //_receipt.Add(DateTime.Now.ToShortDateString());
                        //_receipt.Add("-----------------------------");
                        //_receipt.Add("Food:");
                        //foreach (ShopItem shopItem in Dm.SelectedItems)
                        //{
                        //    if (shopItem.IsFood)
                        //    {
                        //        _receipt.Add($"Item: {shopItem.Name} Price: {shopItem.Price} Quantity: {shopItem.Quantity}");
                        //        _receipt.Add($"     Subtotal: {shopItem.SubTotal}");
                        //    }
                        //}
                        //_receipt.Add("Beverages:");
                        //foreach (ShopItem shopItem in Dm.SelectedItems)
                        //{
                        //    if (!shopItem.IsFood)
                        //    {
                        //        _receipt.Add($"Item {shopItem.Name} Price {shopItem.Price} Quantity {shopItem.Quantity}");
                        //        _receipt.Add($"     Subtotal: {shopItem.SubTotal}");
                        //    }
                        //}
                        //_receipt.Add($"Total {Dm.SelectedItems.Sum(x => x.SubTotal)}");
                        //Receipt = _receipt;
                        _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
                        _selectedItems.Clear();
                        SelectedItems = _selectedItems;
                        SearchText = "";
                        _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(LoanItems);
                        _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        _mainViewModel.ResetTimer.Start();
                        Visitor = null;

                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {
                    Display = new Display(Brushes.Red, "This is a differen visitor", "times", false, true);
                }


            }
            catch
            {
                Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
            }

        }

        public void GetLoans(object sender, RFIDTagEventArgs e)
        {
            try
            {
                Visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                try
                {
                    if (Visitor != null)
                    {
                        Loans = dh.GetLoans(Visitor, SelectedLoanStand.ID);
                        CanChangeLoanStand = false;
                        //if the dont have any loanen items
                        if (Loans.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Select items", "", false, false);
                            _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        }
                        else if (Loans.Where(l => l.IsOverdue).Count() > 0)
                        {
                            Display = new Display(Brushes.Black, "Visitor must return overdue items before loaning new items", "", false, false);
                            _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        }
                        else if (SelectedItems.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Select items", "", false, false);
                            _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                        }
                        _mainViewModel._MyRFIDReader.Tag -= new RFIDTagEventHandler(GetLoans);
                    }
                    else
                    {
                        Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
                    }
                }
                catch
                {
                    Display = new Display(Brushes.Red, "Something went wrong with loans", "times", false, true);
                }
                //The visitor is valid


            }
            catch
            {
                Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
            }




        }

        private void Reset()
        {
            //Loans = dh.GetLoans(_visitor, SelectedLoanStand.ID);
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
            _selectedItems.Clear();
            SelectedItems = _selectedItems;
        }
        private void Reset(object sender, EventArgs e)
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID", "", true, false);
            Loans = new List<Loan>();
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
            _selectedItems.Clear();
            SelectedItems = _selectedItems;
            _mainViewModel.ResetTimer.Stop();
            _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);
            _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            CanChangeLoanStand = true;
            Visitor = null;
        }
    }
}
