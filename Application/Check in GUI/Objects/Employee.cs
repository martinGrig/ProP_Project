using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManager
{
    public class Employee
    {
        public int EmployeeNr { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string JobId { get; private set; }
        public string JobDescription { get; private set; }
        public string Password { get; private set; }

        public Employee(string firstName, string lastName, string jobId, string password, int employeeNr, string jobdesc)
        {
            FirstName = firstName;
            LastName = lastName;
            JobId = jobId;
            Password = password;
            EmployeeNr = employeeNr;
            JobDescription = jobdesc;
        }
    }
}
