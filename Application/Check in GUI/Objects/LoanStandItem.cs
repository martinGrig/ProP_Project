using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class LoanStandItem : Item
    {

        public LoanStandItem (string _name, double _price, int _stock, string fileName, int _id) : base(_name, _price, _stock, fileName, _id)
        {
        }
    }
}
