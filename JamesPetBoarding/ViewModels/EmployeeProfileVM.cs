using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeProfileVM
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeNameDisplay { get; set; }

        public string RoleDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string ProfileImagePath { get; set; }

        public bool IsAdmin { get; set; }

    }
}