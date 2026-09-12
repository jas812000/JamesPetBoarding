using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class EmergencyContactDeactivateVM
    {
        public Guid EmergencyContactId { get; set; }

        public Guid CustomerId { get; set; }

        public string FullNameDisplay { get; set; }

        public string RelationshipDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityStateZipDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public bool IsActive { get; set; }

        public string StatusDisplay { get; set; }

        [Required]
        public InactivationReasonEnum InactivationReason { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }
    }
}