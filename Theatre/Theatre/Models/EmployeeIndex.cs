using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Theatre.Models
{ 
    public class EmployeeIndex
    {

        public Employee Employee { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public int CurrentRole { get; set; }
    }
}
