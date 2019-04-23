using EventManager.Models;
using EventManager.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EventManager.ViewModels
{
    public class ShopViewModel : ObservableObject, IPageViewModel
    {
        public DataModel Dm { get;  set; }
        DataHelper dh;

        public ShopViewModel(DataModel dataModel)
        {
            Dm = dataModel;
            dh = new DataHelper();
           
            OnPropertyChanged("FilteredItems");
            OnPropertyChanged("Items");
            //_items = new List<Item>();
            //_selectedItems = new List<Item>();
            //_selectedItems.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            //_items.Add(new Item("ddddd", 1.5, 8, "Images/camp.png"));
            //_items.Add(new Item("tter", 1.5, 50, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 8, "Images/camp.png"));
            //_items.Add(new Item("Hamburger", 1.5, 50, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 8, "Images/camp.png"));
            //_items.Add(new Item("Hamburger", 1.5, 50, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            //_items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
        }

        // Commands 
        private RelayCommand _selectItem;
        public RelayCommand SelectItemCommand
        {
            get
            {
                if (_selectItem == null)
                {
                    _selectItem  = new RelayCommand(new Action<object>(SelectItem));
                }
                return _selectItem;
            }
        }
        private void SelectItem(object obj)
        {
            Item item = ((Item)obj);
            if(Dm._selectedItems == null)
            {
                Dm._selectedItems = new List<Item>();
            }
            foreach(Item it in Dm.SelectedItems)
            {
                if(it.Name == item.Name)
                {
                    it.SelectItem();
                    Dm.SelectedItems = Dm._selectedItems;
                    return;
                }
            }
            
            Dm.SelectedEmployee = dh.GetEmployee(Convert.ToInt32(Dm.EmployeeNumber));
            MessageBox.Show(Dm.SelectedEmployee.FirstName);
            item.SelectItem();
            Dm._selectedItems.Add(item);
            Dm.SelectedItems = Dm._selectedItems;
        }
    }

}
