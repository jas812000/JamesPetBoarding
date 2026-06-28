using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingUnitFormVM
    {
        public Guid BoardingUnitId { get; set; }

        [Required]
        public UnitTypeEnum UnitType { get; set; }

        [Required]
        public UnitNameEnum UnitName { get; set; }

        [Required, Range(1, 10)]
        public int UnitNumber { get; set; }

        [Required]
        public SpeciesAllowedEnum SpeciesAllowed { get; set; }

        [Required]
        public SizeCategoryEnum SizeCategory { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}