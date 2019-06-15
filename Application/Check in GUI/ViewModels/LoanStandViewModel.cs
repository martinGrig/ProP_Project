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
                OnPropertyChanged("Total");
            }
        }

        public string Total
        {
            get
            {
                return SelectedItems.Sum(x => x.SubTotal).ToString();
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
                if (value != null)
                {
                    if (_selectedLoanStand != value)
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
        private bool _canSeeDisplay2;
        public bool CanSeeDisplay2
        {
            get
            {
                return _canSeeDisplay2;
            }
            set
            {
                _canSeeDisplay2 = value;
                OnPropertyChanged("CanSeeDisplay2");
            }
        }

        private Display _display2;
        public Display Display2
        {
            get
            {
                return _display2;
            }
            set
            {
                _display2 = value;
                OnPropertyChanged("Display2");
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
            Dm = _mainViewModel.dataModel;
            CanChangeLoanStand = true;
            LoanStands = dh.GetEmployeeLoanStands(Convert.ToInt32(Dm.EmployeeNumber));
            Loans = new List<Loan>();
            if (LoanStands.Count() == 1)
            {
                CanChangeLoanStand = false;
            }
            if (LoanStands.Count() == 0)
            {
                CanChangeLoanStand = false;
                Display2 = new Display(Brushes.Red, "Employee does not \nhave any LoanStand", "", false, false);
                CanSeeDisplay2 = true;
            }
            else
            {
                SelectedLoanStand = _loanStands[0];
                Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet", "", true, false);
                _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
            }
            _selectedItems = new List<LoanStandItem>();
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 3);
            try
            {
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);
                _mainViewModel._MyRFIDReader.Detach += new DetachEventHandler(DetachRfid);
                _mainViewModel._MyRFIDReader.Attach += new AttachEventHandler(AttachRfid);
                _mainViewModel._MyRFIDReader.Open();
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false); }
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
                        _receipt.Add($"Loan Stand: {SelectedLoanStand.Name}");
                        _receipt.Add($"Customer: {Visitor.FirstName} {Visitor.LastName}");
                        _receipt.Add(DateTime.Now.ToShortDateString());
                        _receipt.Add("-----------------------------");
                        _receipt.Add("Loaned Items:");
                        foreach (LoanStandItem lsi in SelectedItems)
                        {
                                _receipt.Add($"Item: {lsi.Name} Price: {lsi.Price} Quantity: {lsi.Quantity}");
                                _receipt.Add($"Subtotal: {lsi.SubTotal}");
                        }
                        _receipt.Add("-----------------------------");
                        _receipt.Add($"Total {Total}");
                        Receipt = _receipt;
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
                        Display = new Display(Brushes.Red, $"{ex.Message}", "times", false, true);
                        _mainViewModel.ResetTimer.Start();
                        _mainViewModel._MyRFIDReader.AntennaEnabled = false;
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
                            int days;
                            double pricePerDay;
                            int daysOverdue;
                            double extra;
                            double totalExtra = 0;
                            foreach (Loan loan in Loans)
                            {
                                if (loan.IsOverdue)
                                {
                                    days = ((TimeSpan)(loan.EndDate - loan.StartDate)).Days;
                                    pricePerDay = (loan.Total / loan.Qauntity) / days;
                                    daysOverdue = ((TimeSpan)(DateTime.Now - loan.EndDate)).Days;
                                    extra = pricePerDay * daysOverdue * loan.Qauntity;
                                    totalExtra += extra;
                                }
                            }

                            if (Visitor.Balance >= totalExtra)
                            {
                                Display = new Display(Brushes.Orange, $"Visitor must return overdue items.\n Must pay:{totalExtra}", "", false, false);
                                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                            }
                            else
                            {
                                //Start timer but increase the interval
                                Display = new Display(Brushes.Red, $"Visitor does not have enough funds to pay for overdue items.\n Must pay:{totalExtra}", "", false, false);
                                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                                _mainViewModel.ResetTimer.Start();
                            }

                            //Display = new Display(Brushes.Black, "Visitor must return overdue items before loaning new items", "", false, false);
                            //_mainViewModel._MyRFIDReader.AntennaEnabled = false;
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
        public void Reset(object sender, EventArgs e)
        {
            Display = new Display(Brushes.Black, "Scan vistors \nRFID", "", true, false);
            _receipt = new List<string>();
            Receipt = _receipt;
            Loans = new List<Loan>();
            _loanItems = dh.GetLoanStandItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedLoanStand.ID);
            _selectedItems.Clear();
            SelectedItems = _selectedItems;
            SearchText = "";
            _mainViewModel.ResetTimer.Stop();
            _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(GetLoans);
            try
            {
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false); }
            CanChangeLoanStand = true;
            Visitor = null;
        }

        private void AttachRfid(object sender, AttachEventArgs e)
        {
            if (!CanChangeLoanStand && LoanStands.Count() == 0)
            {
                CanChangeLoanStand = false;
                Display2 = new Display(Brushes.Red, "Employee does not \nhave any LoanStands", "", false, false);
                CanSeeDisplay2 = true;
            }
            else
            {
                CanSeeDisplay2 = false;
                if (SelectedItems.Count() != 0 || _visitor == null)
                {
                    _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                }
                else
                {
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                }
            }

        }
        private void DetachRfid(object sender, DetachEventArgs e)
        {
            Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false);
            CanSeeDisplay2 = true;
        }
    }
}
