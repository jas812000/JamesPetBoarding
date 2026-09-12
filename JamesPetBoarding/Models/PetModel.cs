using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class PetModel
    {
        [Key]
        public Guid PetId { get; set; }

        public PetModel()
        {
            PetId = Guid.NewGuid();

            Diets = new List<DietModel>();
            Medications  = new List<MedicationModel>();
            CustomerPets = new List<CustomerPetModel>();
            PetVaccines = new List<PetVaccineModel>();
            Boardings = new List<BoardingModel>();
        }

        public Guid? VetId { get; set; }

        [ForeignKey("VetId")]
        public VeterinarianModel Veterinarian { get; set; }

        [Required, MaxLength(50)]
        public string PetName { get; set; }

        [Required]
        public SpeciesEnum Species { get; set; }

        [Required, MaxLength(50)]
        public string Breed { get; set; }

        [Required]
        public SexEnum Sex { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        [Range(0.01, 9999.99)]
        public decimal Weight { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public InactivationReasonEnum? InactivationReason { get; set; }

        public DateTime? InactivationDate { get; set; }

        [MaxLength(500)]
        public string InactivationNotes { get; set; }

        public DateTime? ReactivationDate { get; set; }

        [MaxLength(500)]
        public string ReactivationNotes { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<DietModel> Diets { get; set; }

        public List<MedicationModel> Medications { get; set; }

        public List<CustomerPetModel> CustomerPets { get; set; }

        public List<PetVaccineModel> PetVaccines { get; set; }

        public List<BoardingModel> Boardings { get; set; }
    }
}