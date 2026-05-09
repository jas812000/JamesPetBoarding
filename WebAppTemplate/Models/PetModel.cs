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
        }

        public Guid? VetId { get; set; }

        [ForeignKey("VetId")]
        public VeterinarianModel Veterinarian { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(20)]
        public string Species { get; set; }

        [Required, MaxLength(50)]
        public string Breed { get; set; }

        [Required, MaxLength(10)]
        public string Sex { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        public int Age { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Weight { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<DietModel> Diets { get; set; }

        public List<MedicationModel> Medications { get; set; }

        public List<CustomerPetModel> CustomerPets { get; set; }

        public List<PetVaccineModel> PetVaccines { get; set; }

        public List<BoardingModel> Boardings { get; set; }
    }
}