using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consid.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public decimal Salary { get; set; }
        public bool IsCEO { get; set; }
        public bool IsManager { get; set; }
        public virtual Employee Manager { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public int? ManagerId { get; set; }

    }
}
