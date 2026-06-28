using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Models
{
    public class DietModel
    {

        [Key]
        public Guid DietId { get; set; }

        public DietModel() 
        {
            DietId = Guid.NewGuid();
        }

        [Required]
        public Guid PetId { get; set; }

        [ForeignKey("PetId")]
        public PetModel Pet { get; set; }

        [Required, MaxLength(50)]
        public string FoodName { get; set; }

        [Required, MaxLength(20)]
        public string Amount { get; set; }

        [Required]
        public FrequencyEnum Frequency { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; } 

    }
}