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
    public class PetFormVM
    {
        public Guid PetId { get; set; }

        public Guid? VetId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public SpeciesEnum Species { get; set; }

        [Required, MaxLength(50)]
        public string Breed { get; set; }

        [Required]
        public SexEnum Sex { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required, Range(0.01, 9999.99)]
        public decimal Weight { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}