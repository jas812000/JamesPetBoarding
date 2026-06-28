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
    public class BoardingUtilizationReportFilterVM
    {
        [Required]
        public DateTime UtilizationStartDate { get; set; }

        [Required]
        public DateTime UtilizationEndDate { get; set; }

        public BoardingStatusEnum? Status { get; set; }

        public UnitTypeEnum? UnitType { get; set; }

        public UnitNameEnum? UnitName { get; set; }

        [Range(1, 10)]
        public int? UnitNumber { get; set; }

        public SpeciesAllowedEnum? SpeciesAllowed { get; set; }

        public SizeCategoryEnum? SizeCategory { get; set; }

        public ActiveStatusEnum? ActiveStatus { get; set; }

    }
}