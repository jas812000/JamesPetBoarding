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

        public string EmployeeNameDisplay { get; set; }

        public string RoleDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public bool IsActive { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string InactivationReasonDisplay { get; set; }

        public string InactivationDateDisplay { get; set; }

        public string InactivationNotesDisplay { get; set; }

        public string ReactivationDateDisplay { get; set; }

        public string ReactivationNotesDisplay { get; set; }

        public string NotesDisplay { get; set; }

    }
}