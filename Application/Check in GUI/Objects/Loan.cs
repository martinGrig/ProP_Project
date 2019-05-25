using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager.Objects
{
    public class Loan
    {
        public int ID { get; private set; }
        public int LoanStandID { get; private set; }
        public string Name { get; private set; }
        public string LoanstandName { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public double Total { get; private set; }
        public int Qauntity { get; private set; }
        public bool IsOverdue { get; private set; } 
        public int ItemId { get; private set; }

        public Loan(int id, int loanstandId, string name, string loanstandName, DateTime start, DateTime end, double total, int qauntity, int itemId)
        {
            ID = id;
            LoanStandID = loanstandId;
            Name = name;
            LoanstandName = loanstandName;
            StartDate = start;
            EndDate = end;
            Total = total;
            Qauntity = qauntity;
            ItemId = itemId;
            if(DateTime.Compare(DateTime.Now, EndDate) > 0)
            {
                IsOverdue = true;
            }
            else
            {
                IsOverdue = false;
            }

        }

    }
}
