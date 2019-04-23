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
        public int JobId { get; private set; }
        public int Password { get; private set; }
        public Employee(string firstName, string lastName, int jobId)
        {
            FirstName = firstName;
            LastName = lastName;
            rng = new Random();
            Password = rng.Next(10000);
            EmployeeNr = _employeeIdCounter;
            _employeeIdCounter++;
        }
        public Employee(string firstName, string lastName, int jobId, int password, int employeeNr)
        {
            FirstName = firstName;
            LastName = lastName;
            JobId = jobId;
            Password = password;
            EmployeeNr = employeeNr;
        }
    }
}
