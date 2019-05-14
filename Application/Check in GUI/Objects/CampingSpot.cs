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
        public string Spot { get; private set; }
        public int AmountOfParticpants { get; private set; }
        public bool PaymentStatus { get; private set; }

        public CampingSpot(string groupLeader, string spot, int amountPar, bool status)
        {
            GroupLeader = groupLeader;
            Spot = spot;
            AmountOfParticpants = amountPar;
            PaymentStatus = status;
        }
    }
}
