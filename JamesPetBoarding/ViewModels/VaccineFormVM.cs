using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineFormVM
    {
        public Guid VaccineId { get; set; }

        [Required, MaxLength(200)]
        public string VaccineName { get; set; }

        [Required]
        public SpeciesEnum? Species { get; set; }

        [Required]
        public bool? RequiredFlag { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}