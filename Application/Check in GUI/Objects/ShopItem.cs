using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class ShopItem : Item
    {
        public bool IsFood { get; private set; }

        public ShopItem(string _name, double _price, int _stock, string fileName, int _id, bool _isFood) : base(_name, _price, _stock, fileName, _id)
        {
            IsFood = _isFood;
        }
    }
}
