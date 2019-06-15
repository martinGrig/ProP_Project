using EventManager.Models;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Phidget22;
using Phidget22.Events;
using EventManager.Objects;
using System.Windows.Media;

namespace EventManager.ViewModels
{
    public class ShopViewModel : ObservableObject, IPageViewModel
    {
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

        private Shop _selectedShop;
        public Shop SelectedShop
        {
            get
            {
                return _selectedShop;
            }
            set
            {
                if (value != null)
                {
                    if (_selectedShop != value)
                    {
                        _selectedShop = value;
                        Display = new Display(Brushes.Black, "Select items", "", false, false);
                        Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedShop.ID);
                        Dm.SearchText = "";
                    }
                    _selectedShop = value;
                    OnPropertyChanged("SelectedShop");
                }

            }
        }

        private bool _canChangeShop;
        public bool CanChangeShop
        {
            get
            {
                return _canChangeShop;
            }
            set
            {
                _canChangeShop = value;
                OnPropertyChanged("CanChangeShop");
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

        public ShopViewModel(MainViewModel mainViewModel)
        {
            _shops = new List<Shop>();
            _mainViewModel = mainViewModel;
            Dm = _mainViewModel.dataModel;
            dh = new DataHelper();
            _receipt = new List<string>();

        }

        // Commands 
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
        private void SelectItem(object obj)
        {
            ShopItem item = ((ShopItem)obj);
            if (Dm._selectedItems == null)
            {
                Dm._selectedItems = new List<ShopItem>();
            }
            if (Dm.SelectedItems.Count() == 0)
            {
                CanChangeShop = false;
                Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet to complete trasaction", "", true, false);
                _mainViewModel._MyRFIDReader.AntennaEnabled = true;
            }
            //Check if there is already a item with that name and  if there is select the item to update its properties
            foreach (ShopItem it in Dm.SelectedItems)
            {
                if (it.Name == item.Name)
                {
                    it.SelectItem();
                    Dm.SelectedItems = Dm._selectedItems;
                    return;
                }
            }
            if (item.Stock != 0)
            {
                item.SelectItem();
                Dm._selectedItems.Add(item);
                Dm.SelectedItems = Dm._selectedItems;
                OnPropertyChanged("FilteredItems");
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
            ShopItem item = ((ShopItem)obj);

            item.UnselectItem();
            Dm.SelectedItems = Dm._selectedItems;
            if (item.Quantity == 0)
            {
                Dm._selectedItems.Remove(item);
                Dm.SelectedItems = Dm._selectedItems;
            }
            if (Dm.SelectedItems.Count() == 0)
            {
                CanChangeShop = true;
                Display = new Display(Brushes.Black, "Select items", "", false, false);
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
            }

        }



        public void SellItems(object sender, RFIDTagEventArgs e)
        {
            Visitor temp = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
            if (temp != null)
            {
                try
                {
                    _mainViewModel.dataHelper.CreatePurchase(temp, Dm.SelectedItems.ToList(), SelectedShop.ID);
                    _receipt.Add($"Shop: {SelectedShop.Name}");
                    _receipt.Add($"Customer: {temp.FirstName} {temp.LastName}");
                    _receipt.Add(DateTime.Now.ToShortDateString());
                    _receipt.Add("-----------------------------");
                    _receipt.Add("Food:");
                    foreach (ShopItem shopItem in Dm.SelectedItems)
                    {
                        if (shopItem.IsFood)
                        {
                            _receipt.Add($"Item: {shopItem.Name} Price: {shopItem.Price} Quantity: {shopItem.Quantity}");
                            _receipt.Add($"Subtotal: {shopItem.SubTotal}");
                        }
                    }
                    _receipt.Add("Beverages:");
                    foreach (ShopItem shopItem in Dm.SelectedItems)
                    {
                        if (!shopItem.IsFood)
                        {
                            _receipt.Add($"Item {shopItem.Name} Price {shopItem.Price} Quantity {shopItem.Quantity}");
                            _receipt.Add($"Subtotal: {shopItem.SubTotal}");
                        }
                    }
                    _receipt.Add("-----------------------------");
                    _receipt.Add($"Total {Dm.SelectedItems.Sum(x => x.SubTotal)}");
                    Display = new Display(Brushes.Green, "Transaction Complete \nItems successfully purchased", "check", false, true);
                    Receipt = _receipt;
                    Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedShop.ID);
                    Dm._selectedItems.Clear();
                    Dm.SelectedItems = Dm._selectedItems;
                    Dm.SearchText = "";
                    _mainViewModel.ResetTimer.Start();
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                }
                catch (Exception ex)
                {
                    Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedShop.ID);
                    Dm._selectedItems.Clear();
                    Dm.SelectedItems = Dm._selectedItems;
                    Dm.SearchText = "";
                    Display = new Display(Brushes.Red, $"{ex.Message}", "times", false, true);
                    _mainViewModel.ResetTimer.Start();
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                }

            }
            else
            {
                Display = new Display(Brushes.Red, "Visitor not recognised, try again", "times", false, true);
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                Task.Delay(1000).ContinueWith(_ =>
                {
                    Display = new Display(Brushes.Black, "Scan vistors \nRFID Bracelet to complete trasaction", "", true, false);
                    _mainViewModel._MyRFIDReader.AntennaEnabled = true;
                });
            }

        }

        public void Start()
        {
            Dm = _mainViewModel.dataModel;
            CanSeeDisplay2 = false;
            try
            {

                _mainViewModel._MyRFIDReader.Detach += new DetachEventHandler(DetachRfid);
                _mainViewModel._MyRFIDReader.Attach += new AttachEventHandler(AttachRfid);
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(SellItems);
                _mainViewModel._MyRFIDReader.Open();
            }
            catch (PhidgetException) { }
            CanChangeShop = true;
            Shops = dh.GetEmployeeShops(Convert.ToInt32(Dm.EmployeeNumber));
            if (Shops.Count() == 1)
            {
                CanChangeShop = false;
            }
            if (Shops.Count() == 0)
            {
                CanChangeShop = false;
                Display2 = new Display(Brushes.Red, "Employee does not \nhave any Shops", "", false, false);
                CanSeeDisplay2 = true;
            }
            else
            {

                SelectedShop = _shops[0];
                Display = new Display(Brushes.Black, "Select items", "", false, false);
                Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber), SelectedShop.ID);
            }
            Dm._selectedItems = new List<ShopItem>();
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 4);
            try
            {
                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
            }
            catch (PhidgetException) { CanSeeDisplay2 = true; Display2 = new Display(Brushes.Red, "Please connect rfid reader", "", false, false); }
        }

        public void Reset(object sender, EventArgs e)
        {
            CanChangeShop = true;
            Display = new Display(Brushes.Black, "Select items", "", false, false);
            _receipt = new List<string>();
            Receipt = _receipt;
            _mainViewModel.ResetTimer.Stop();
        }

        private void AttachRfid(object sender, AttachEventArgs e)
        {
            if (!CanChangeShop && Shops.Count() == 0)
            {
                CanChangeShop = false;
                Display2 = new Display(Brushes.Red, "Employee does not \nhave any Shops", "", false, false);
                CanSeeDisplay2 = true;
            }
            else
            {
                CanSeeDisplay2 = false;
                if (Dm.SelectedItems != null &&  Dm.SelectedItems.Count() != 0)
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
