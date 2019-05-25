using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class LoanStand
    {
        public int ID { get; private set; }
        public string Name { get; private set; }

        public LoanStand(int _id, string name)
        {
            ID = _id;
            Name = name;
        }
    }
}
