using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingFormVM
    {
        public BoardingFormVM()
        {
            CustomerSelectList = new List<SelectListItem>();
            PetSelectList = new List<SelectListItem>();
            BoardingUnitSelectList = new List<SelectListItem>();
        }

        public Guid BoardingId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        [Required]
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        [Required]
        public Guid BoardingUnitId { get; set; }

        public string BoardingUnitDisplay { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<SelectListItem> CustomerSelectList { get; set; }

        public List<SelectListItem> PetSelectList { get; set; }

        public List<SelectListItem> BoardingUnitSelectList { get; set; }

    }
}