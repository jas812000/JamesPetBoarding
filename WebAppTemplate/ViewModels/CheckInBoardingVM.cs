using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CheckInBoardingVM
    {
        public Guid BoardingId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        [Required]
        public DateTime? ActualCheckInDateTime { get; set; }

        [Required]
        public Guid? CheckedInByEmployeeId { get; set; }

        public string CheckedInByEmployeeNameDisplay { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}