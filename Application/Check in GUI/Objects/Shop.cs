using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class Shop
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public Shop(int _id, string name)
        {
            ID = _id;
            Name = name;
        }
    }
}
