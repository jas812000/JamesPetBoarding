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
    public class PetVaccineFormVM
    {
        public PetVaccineFormVM() 
        { 
            VaccineOptions = new List<SelectListItem>(); 
        }

        public Guid PetVaccineId { get; set; }

        [Required]
        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        [Required]
        public Guid VaccineId { get; set; }

        public List<SelectListItem> VaccineOptions { get; set; }

        [Required]
        public DateTime DateGiven { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required, MaxLength(200)]
        public string DocumentFilePath { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}