using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    class Visitor
    {
        //private string _firstName;
        //private string _lastName;
        private int _ticketNr;
        private string _email;
        private string _password;
        private string _bankAccountNr;
        private int _balance;
        private int _RFIDCode;

        private int _campingSpotId;
        private int _campSpotIt;
        private bool _isCampPayed;

        private bool _idScanned;
        private bool _isValid;
        private DateTime _whenScanned;

        public string FirstName { get; private set; }
        public string SecondName { get; private set; }
        public string Email { get; private set; }  //add regex for email
        public Visitor(string firstName, string lastName, string email, string password, string bankAccountNr, int balance)
        {

        }
        public Visitor(string firstName, string lastName, string email, string password, string bankAccountNr, int balance, int _campingSpotId)
        {

        }
    }
}
