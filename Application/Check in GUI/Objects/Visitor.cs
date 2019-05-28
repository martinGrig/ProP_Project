using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    public class Visitor : ObservableObject
    {
        private string _email;
        private string _rfidCode;
        private int _campingSpotId;
        private int _campSpotIt;
        private bool _isCampPayed;

        private bool _idScanned;
        private bool _isValid;
        private DateTime _whenScanned;

        public string RfidCode
        {
            get
            {
                return _rfidCode;
            }
            set
            {
                _rfidCode = value;
                OnPropertyChanged("RfidCode");
                OnPropertyChanged("SelectedVisitor");
            }
        }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int Balance { get; private set; }
        public int TicketNr { get; private set; }
        public string Email { get; private set; }  //add regex for email
        public bool IsScanned { get; private set; }
        public bool IsValid { get; private set; }
        public Visitor(string firstName, string lastName, int ticketNumber, string email, int balance, bool isScanned, bool isValid)
        {
            FirstName = firstName;
            LastName = lastName;
            TicketNr = ticketNumber;
            Balance = balance;
            Email = email;
            IsScanned = isScanned;
            IsValid = isValid;
        }


    }
}
