using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerDeactivateVM
    {
        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityStateZipDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public bool IsActive { get; set; }

        public string StatusDisplay { get; set; }

        [Required(ErrorMessage = "Select an inactivation reason.")]
        public InactivationReasonEnum? InactivationReason { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }

    }
}