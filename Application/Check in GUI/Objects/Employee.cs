using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    public class Employee
    {
        private Random rng;
        static int _employeeIdCounter = 123456;
        public int EmployeeNr { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string JobId { get; private set; }
        public string Password { get; private set; }
        public Employee(string firstName, string lastName, string jobId)
        {
            FirstName = firstName;
            LastName = lastName;
            rng = new Random();
            EmployeeNr = _employeeIdCounter;
            JobId = jobId;
            _employeeIdCounter++;
        }
        public Employee(string firstName, string lastName, string jobId, string password, int employeeNr)
        {
            FirstName = firstName;
            LastName = lastName;
            JobId = jobId;
            Password = password;
            EmployeeNr = employeeNr;
        }
    }
}
