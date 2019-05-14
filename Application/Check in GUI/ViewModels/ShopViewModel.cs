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

namespace EventManager.ViewModels
{
    public class ShopViewModel : ObservableObject, IPageViewModel
    {
        public DataModel Dm { get; set; }
        DataHelper dh;
        private RFID myRFIDReader;

        public ShopViewModel(DataModel dataModel)
        {
            Dm = dataModel;
            dh = new DataHelper();


            try
            {
                myRFIDReader = new RFID();
                myRFIDReader.Tag += new RFIDTagEventHandler(SellItems);
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
            Item item = ((Item)obj);
            if (Dm._selectedItems == null)
            {
                Dm._selectedItems = new List<Item>();
                myRFIDReader.Open();
            }
            //Check if there is already a item with that name and  if there is select the item to update its properties
            foreach (Item it in Dm.SelectedItems)
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
            MessageBox.Show(e.Tag);
            foreach (Item it in Dm.SelectedItems)
            {
                it.SellItem();
            }
            Dm.SelectedItems = new List<Item>();
        }
    }

}
