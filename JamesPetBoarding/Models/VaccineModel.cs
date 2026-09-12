using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class VaccineModel
    {

        [Key]
        public Guid VaccineId { get; set; }

        public VaccineModel() 
        {
            VaccineId = Guid.NewGuid();
        }

        [Required, MaxLength(200)]
        public string VaccineName { get; set; }

        [Required]
        public SpeciesEnum Species { get; set; }

        [Required]
        public bool RequiredFlag { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<PetVaccineModel> PetVaccines { get; set; }

    }
}