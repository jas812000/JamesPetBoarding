using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeSummaryVM
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeNameDisplay { get; set; }

        public string RoleDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public bool IsActive { get; set; }

        public string ActiveStatusDisplay { get; set; }
    }
}