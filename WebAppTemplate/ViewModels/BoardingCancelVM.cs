using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingCancelVM
    {
        public Guid BoardingId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PetNameDisplay { get; set; }

        public string BoardingUnitDisplay { get; set; }

        public BoardingStatusEnum BoardingStatus { get; set; }

        public string StartDateTimeDisplay { get; set; }

        public string EndDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }

        [Required]
        [MaxLength(1000)]
        public string CancelledReason { get; set; }
    }
    
}