using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class PetVaccineModel
    {

        [Key]
        public Guid PetVaccineId { get; set; }

        public PetVaccineModel() 
        {
            PetVaccineId = Guid.NewGuid();
        }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required]
        public Guid VaccineId { get; set; }

        [ForeignKey("VaccineId")]
        public VaccineModel Vaccine { get; set; }

        [Required]
        public DateTime DateGiven { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        [Required, MaxLength(200)]
        public string DocumentFilePath { get; set; }

        [MaxLength(2000)]
        public string Notes {  get; set; }

    }
}