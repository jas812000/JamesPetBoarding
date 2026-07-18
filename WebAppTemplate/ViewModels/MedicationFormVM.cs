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
    public class MedicationFormVM
    {
        public Guid MedicationId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        [Required, MaxLength(50)]
        public string MedicationName { get; set; }

        [Required, MaxLength(20)]
        public string Dosage { get; set; }

        [Required]
        public MedicationRouteEnum Route { get; set; }

        [Required]
        public FrequencyEnum Frequency { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}