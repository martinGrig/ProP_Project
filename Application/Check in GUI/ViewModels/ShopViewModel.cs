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
        //These properties need to go into a seperate Model clase 
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
            }
        }

        private List<Item> _items;

        public IEnumerable<Item> FilteredItems
        {
            get
            {

                    if (SearchText == null)
                    {
                        return _items;
                    }
                return _items.Where(x => x.Name.ToUpper().Contains(SearchText.ToUpper()));

            }
            
        }
        public ShopViewModel()
        {
            _items = new List<Item>();
            _items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            _items.Add(new Item("ddddd", 1.5, 8, "Images/camp.png"));
            _items.Add(new Item("tter", 1.5, 50, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 8, "Images/camp.png"));
            _items.Add(new Item("Hamburger", 1.5, 50, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 880, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 8, "Images/camp.png"));
            _items.Add(new Item("Hamburger", 1.5, 50, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 30, "Images/burger.ico"));
            _items.Add(new Item("Hamburger", 1.5, 540, "Images/burger.ico"));
        }

        // Commands 
        private RelayCommand _selectItem;
        public RelayCommand SelectItemCommand
        {
            get
            {
                if (_selectItem == null)
                {
                    _selectItem = new RelayCommand(SelectItem);
                }
                return _selectItem;
            }
        }
        private void SelectItem(object obj)
        {
            SearchText = ((Item)obj).Name;
        }
    }

}
