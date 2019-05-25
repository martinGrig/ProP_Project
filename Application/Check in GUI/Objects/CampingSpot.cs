using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class CampingSpot
    {
        public string GroupLeader { get; private set; }
        public int Spot { get; private set; }
        public int AmountOfParticpants { get; private set; }
        public string Name { get; private set; }
        public bool PaymentStatus { get; private set; }

        public CampingSpot(string groupLeader, int spot, int amountPar, bool status, string name)
        {
            GroupLeader = groupLeader;
            Spot = spot;
            AmountOfParticpants = amountPar;
            PaymentStatus = status;
            Name = name;
        }
    }
}
