using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingUnitDetailsVM
    {
        public Guid BoardingUnitId { get; set; }

        public string UnitTypeDisplay { get; set; }

        public string FullUnitNameDisplay { get; set; }

        public string SpeciesAllowedDisplay { get; set; }

        public string SizeCategoryDisplay { get; set; }

        public bool IsActive { get; set; }

        public string StatusDisplay { get; set; }

        public string Notes { get; set; }
    }
}