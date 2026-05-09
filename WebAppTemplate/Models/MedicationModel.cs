using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class MedicationModel
    {

        [Key]
        public Guid MedicationId { get; set; }

        public MedicationModel() 
        {
            MedicationId = Guid.NewGuid();      
        }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required, MaxLength(50)]
        public string MedicationName { get; set; }

        [Required, MaxLength(20)]
        public string Dosage {  get; set; }

        [Required, MaxLength(20)]
        public string Route { get; set; }

        [Required, MaxLength(50)]
        public string Frequency { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}