using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Consid.ViewModels
{
    public class EmployeeViewModel
    {

        public int Id { get; set; }

        [DisplayName("First name")]
        public string FirstName { get; set; }

        [DisplayName("Last name")]
        public string LastName { get; set; }

        public decimal SalaryCoefficient { get; set; }

        public decimal Salary { get; set; }

        [DisplayName("CEO?")]
        public bool IsCEO { get; set; }

        [DisplayName("Manager?")]
        public bool IsManager { get; set; }

        public bool IsManaging { get; set; }

        [DisplayName("Rank 1-10")]
        [Range(1, 10)]
        public int Rank { get; set; }

        [DisplayName("Role")]
        public string Type { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }

        public string ManagerLastName { get; set; }

        public string ManagerFirstName { get; set; }

        [DisplayName("Manager")]
        public int? ManagerId { get; set; }
        public IEnumerable<SelectListItem> Managers { get; set; }


    }
}
