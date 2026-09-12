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
    public class DietFormVM
    {
        public Guid DietId { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

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