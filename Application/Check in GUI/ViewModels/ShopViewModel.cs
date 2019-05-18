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

namespace EventManager.ViewModels
{
    public class ShopViewModel : ObservableObject, IPageViewModel
    {
        public DataModel Dm { get; set; }
        DataHelper dh;
        private RFID myRFIDReader;
        public MainViewModel _mainViewModel { get; set; }

        public ShopViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            Dm = _mainViewModel.dataModel;
            dh = new DataHelper();


            try
            {
                myRFIDReader = new RFID();
                myRFIDReader.Tag += new RFIDTagEventHandler(SellItems);
                myRFIDReader.Open();
            }
            catch (PhidgetException) { }
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
                myRFIDReader.AntennaEnabled = true;
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

        private RelayCommand _unSelectItem;
        //public RelayCommand UnSelectItemCommand
        //{

        //}

        private void UnSelectItem(object obj)
        {

        }


        public void SellItems(object sender, RFIDTagEventArgs e)
        {
            Visitor temp = _mainViewModel.dataHelper.GetVisitor(e.Tag.ToString());
            if (temp != null)
            {
                try
                {
                    _mainViewModel.dataHelper.CreatePurchase(temp, Dm.SelectedItems.ToList(), 1234);
                    Dm.Items = dh.GetItems(Convert.ToInt32(Dm.EmployeeNumber));
                    Dm._selectedItems.Clear();
                    Dm.SelectedItems = Dm._selectedItems;
                    Dm.SearchText = "";
                    myRFIDReader.AntennaEnabled = false;
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.Message);
                }

            }

        }
    }

}
