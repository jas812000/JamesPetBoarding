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
    public class BoardingCheckInVM
    {
        public Guid BoardingId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        public BoardingStatusEnum BoardingStatus { get; set; }

        public string StartDateTimeDisplay { get; set; }

        public string EndDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}