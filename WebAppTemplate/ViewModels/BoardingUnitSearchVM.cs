using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingUnitSearchVM
    {
        public BoardingUnitSearchVM() 
        {
            BoardingUnitResults = new List<BoardingUnitSummaryVM>();
        }

        public UnitTypeEnum? UnitType { get; set; }

        public UnitNameEnum? UnitName { get; set; }

        [Range(1, 10)]
        public int? UnitNumber { get; set; }

        public SpeciesAllowedEnum? SpeciesAllowed { get; set; }

        public SizeCategoryEnum? SizeCategory { get; set; }

        public bool? IsActive { get; set; }

        public List<BoardingUnitSummaryVM> BoardingUnitResults { get; set; }
    }
}