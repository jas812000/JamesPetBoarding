using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeDetailsVM
    {
        public Guid EmployeeId { get; set; }

        public string FullName { get; set; }

        public string RoleDisplay { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }

        public string StatusDisplay { get; set; }
    }
}