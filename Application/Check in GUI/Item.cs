﻿using System;
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
        private int quanity;
        private double subTotal;

        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Quantity {
            get
            {
                return quanity;
            }
            set
            {
                if (quanity != value)
                {
                    quanity = value;
                    OnPropertyChanged("Quanity");
                    OnPropertyChanged("SubTotal");
                    OnPropertyChanged("FilteredItems");
                }
            }
        }
        public double SubTotal {
            get
            {
                return Quantity*Price;
            }
            set
            {
                if (subTotal != value)
                {
                    subTotal = value;
                    OnPropertyChanged("SubTotal");
                    OnPropertyChanged("FilteredItems");
                }
            }
        }
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
            SubTotal = 0;
            Quantity = 0;
        }

        public void SellItem(int amount)
        {
            if(Stock-amount >= 0)
            {
                Stock -= amount;
            }
        }
        
        public void SelectItem()
        {
            if(Quantity+1 <= Stock)
            {
                Quantity++;
            }
            
        }
        //public List<string> GetSelectedItemInfo()
        //{
        //    int quanity;
        //    foreach()
        //}
    }
}
