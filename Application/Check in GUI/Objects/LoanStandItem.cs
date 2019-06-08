using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class LoanStandItem : Item
    {
        public override double SubTotal
        {
            get { return base.Quantity * base.Price * Days; }
            set
            {
                base.SubTotal = value;
            }
        }
        private int _days;
        public int Days
        {
            get
            {
                return _days;
            }
            set
            {
                if (_days != value)
                {
                    _days = value;
                    OnPropertyChanged("Days");
                    OnPropertyChanged("SubTotal");
                    OnPropertyChanged("FilteredItems");
                }
            }
        }
        public LoanStandItem(string _name, double _price, int _stock, string fileName, int _id) : base(_name, _price, _stock, fileName, _id)
        {
            Days = 1;
        }

        public void IncreaseDays()
        {
            Days++;
        }

        public void DecreaseDays()
        {
            if (Days - 1 >= 1)
            {
                Days--;
            }
        }
    }
}
