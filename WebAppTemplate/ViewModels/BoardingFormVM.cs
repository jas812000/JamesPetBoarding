using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingFormVM
    {
        public Guid BoardingId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public Guid PetId { get; set; }

        [Required]
        public Guid BoardingUnitId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}