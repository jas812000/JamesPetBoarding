using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerDeleteVM
    {
        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string AddressDisplay { get; set; }

        public string CityStateZipDisplay { get; set; }

        public string PhoneDisplay { get; set; }

        public string EmailDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public InactiveReasonEnum? InactiveReason { get; set; }

        [MaxLength(500)]
        public string InactiveNotes { get; set; }

    }
}