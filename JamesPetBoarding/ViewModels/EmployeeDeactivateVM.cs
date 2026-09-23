using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmployeeDeactivateVM
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeNameDisplay { get; set; }

        public string RoleDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public InactivationReasonEnum? InactivationReason { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }

        public string ProfileImagePath { get; set; }
    }
}