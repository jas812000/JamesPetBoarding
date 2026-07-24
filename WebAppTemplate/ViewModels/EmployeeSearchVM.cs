using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeSearchVM
    {
        public EmployeeSearchVM() 
        { 
            EmployeeSearchResults = new List<EmployeeSummaryVM>();
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public EmployeeRoleEnum? Role { get; set; }

        public bool? IsActive { get; set; }

        public List<EmployeeSummaryVM> EmployeeSearchResults { get; set; }
    }
}