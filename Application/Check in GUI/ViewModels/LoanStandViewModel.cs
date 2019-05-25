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
        #endregion


        Visitor _visitor;
        public DataModel Dm { get; set; }
        DataHelper dh;
        public MainViewModel _mainViewModel { get; set; }
        private RFID myRFIDReader;
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
            myRFIDReader = _mainViewModel._MyRFIDReader;
            _selectedItems = new List<LoanStandItem>();

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
                dh.ReturnLoanedItems(Loans.ToList(), _visitor);
                Reset();
                Display = new Display(Brushes.Green, "Items successfully returned", "check", false, true);
                _mainViewModel.ResetTimer.Start();
            }
            catch
            {

            }
        }

        private void ReturnSelected(object obj)
        {
            try
            {
                System.Collections.IList items = (System.Collections.IList)obj;
                var temp = items.Cast<Loan>().ToList();
                dh.ReturnLoanedItems(temp, _visitor);
                Reset();
                Display = new Display(Brushes.Green, "Items successfully returned", "check", false, true);
                _mainViewModel.ResetTimer.Start();
            }
            catch
            {

            }
        }

        public void Start()
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), 1);
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            try
            {
                myRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);

                myRFIDReader.Open();
            }
            catch (PhidgetException) { System.Windows.Forms.MessageBox.Show("Please connect rfid reader"); }
        }

        private void SelectItem(object obj)
        {
            LoanStandItem item = ((LoanStandItem)obj);
            if (_visitor != null)
            {
                if (Loans.Where(l => l.IsOverdue).Count() == 0)
                {
                    if (SelectedItems.Count() == 0)
                    {
                        Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet to complete trasaction", "", true, false);
                        myRFIDReader.AntennaEnabled = true;
                        myRFIDReader.Tag += new RFIDTagEventHandler(LoanItems);
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

        public void LoanItems(object sender, RFIDTagEventArgs e)
        {
            try
            {

                if (_visitor.TicketNr == _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString()).TicketNr)
                {
                    try
                    {
                        _mainViewModel.dataHelper.StartLoan(_visitor, SelectedItems.ToList(), 1, 2);
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
                        _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), 1);
                        _selectedItems.Clear();
                        SelectedItems = _selectedItems;
                        SearchText = "";
                        myRFIDReader.Tag -= new RFIDTagEventHandler(LoanItems);
                        myRFIDReader.AntennaEnabled = false;
                        _mainViewModel.ResetTimer.Start();
                        _visitor = null;
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
                _visitor = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
                try
                {
                    if (_visitor != null)
                    {
                        Loans = dh.GetLoans(_visitor, 1);
                        //if the dont have any loanen items
                        if (Loans.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Select items", "", false, false);
                            myRFIDReader.AntennaEnabled = false;
                        }
                        else if (Loans.Where(l => l.IsOverdue).Count() > 0)
                        {
                            Display = new Display(Brushes.Black, "Visitor must return overdue items before loaning new items", "", false, false);
                            myRFIDReader.AntennaEnabled = false;
                        }
                        else if (SelectedItems.Count() == 0)
                        {
                            Display = new Display(Brushes.Black, "Select items", "", false, false);
                            myRFIDReader.AntennaEnabled = false;
                        }
                        myRFIDReader.Tag -= new RFIDTagEventHandler(GetLoans);
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
            Loans = dh.GetLoans(_visitor, 1);
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), 1);
            _selectedItems.Clear();
            SelectedItems = _selectedItems;
        }
        private void Reset(object sender, EventArgs e)
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID", "", true, false);
            Loans = new List<Loan>();
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), 1);
            _selectedItems.Clear();
            SelectedItems = _selectedItems;
            _mainViewModel.ResetTimer.Stop();
            myRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);
            myRFIDReader.AntennaEnabled = true;
        }
    }
}
