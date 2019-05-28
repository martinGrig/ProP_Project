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
                if(_receipt == null)
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

        public ShopViewModel(MainViewModel mainViewModel)
        {
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
            if(Dm.SelectedItems.Count() == 0)
            {
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
                    _mainViewModel.dataHelper.CreatePurchase(temp, Dm.SelectedItems.ToList(), 1234);
                    _receipt.Add("Shop: blablabla");
                    _receipt.Add($"Customer: {temp.FirstName} {temp.LastName}");
                    _receipt.Add(DateTime.Now.ToShortDateString());
                    _receipt.Add("-----------------------------");
                    _receipt.Add("Food:");
                    foreach(ShopItem shopItem in Dm.SelectedItems)
                    {
                        if (shopItem.IsFood)
                        {
                            _receipt.Add($"Item: {shopItem.Name} Price: {shopItem.Price} Quantity: {shopItem.Quantity}");
                            _receipt.Add($"     Subtotal: {shopItem.SubTotal}");
                        }
                    }
                    _receipt.Add("Beverages:");
                    foreach (ShopItem shopItem in Dm.SelectedItems)
                    {
                        if (!shopItem.IsFood)
                        {
                            _receipt.Add($"Item {shopItem.Name} Price {shopItem.Price} Quantity {shopItem.Quantity}");
                            _receipt.Add($"     Subtotal: {shopItem.SubTotal}");
                        }
                    }
                    Display = new Display(Brushes.Green, "Transaction Complete \nItems successfully purchased", "check", false, true);
                    _receipt.Add($"Total {Dm.SelectedItems.Sum(x => x.SubTotal)}");
                    Receipt = _receipt;
                    Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber));
                    Dm._selectedItems.Clear();
                    Dm.SelectedItems = Dm._selectedItems;
                    Dm.SearchText = "";
                    _mainViewModel.ResetTimer.Start();
                    _mainViewModel._MyRFIDReader.AntennaEnabled = false;
                }
                catch (Exception ex)
                {
                    Display = new Display(Brushes.Red, $"{ex.Message}", "times", false, true);
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
            Display = new Display(Brushes.Black, "Select items", "", false, false);
            Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber));
            _mainViewModel.ResetTimer.Tick += new EventHandler(Reset);
            _mainViewModel.ResetTimer.Interval = new TimeSpan(0, 0, 4);
            try
            {
                _mainViewModel._MyRFIDReader.Open();
                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(SellItems);

                _mainViewModel._MyRFIDReader.Tag += new RFIDTagEventHandler(test);

                _mainViewModel._MyRFIDReader.AntennaEnabled = false;
            }
            catch (PhidgetException) { /*System.Windows.Forms.MessageBox.Show("Please connect rfid reader");*/ }
        }

        private void Reset(object sender, EventArgs e)
        {
            Display = new Display(Brushes.Black, "Select items", "", false, false);
            _receipt = new List<string>();
            Receipt = _receipt;
        }

        private void test(object sender, RFIDTagEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Shop");
        }
    }

}
