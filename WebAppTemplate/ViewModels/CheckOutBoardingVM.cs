using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CheckOutBoardingVM
    {
        public Guid BoardingId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        [Required]
        public DateTime? ActualCheckOutDateTime { get; set; }

        [Required]
        public Guid? CheckedOutByEmployeeId { get; set; }

        public string CheckedOutByEmployeeNameDisplay { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}