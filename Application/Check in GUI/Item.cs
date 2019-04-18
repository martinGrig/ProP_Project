using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    public class Item : ObservableObject
    {
        //private string name;
        //private double price;
        private int stock;

        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Stock
        {
            get
            {
                return stock;
            }
            set
            {
                if (stock != value)
                {
                    stock = value;
                    OnPropertyChanged("Stock");
                    OnPropertyChanged("FilteredItems");
                }
            }
        }
        public string _FileName { get; private set; }


        public Item(string _name, double _price, int _stock, string fileName)
        {
            Name = _name;
            Price = _price;
            Stock = _stock;
            _FileName = fileName;
        }

        public void SellItem(int amount)
        {
            if(Stock-amount >= 0)
            {
                Stock -= amount;
            }
        }
    }
}
