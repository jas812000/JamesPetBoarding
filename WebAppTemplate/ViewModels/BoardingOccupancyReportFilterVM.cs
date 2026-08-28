using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingOccupancyReportFilterVM
    {
        public DateTime? ReportDate { get; set; }

        public UnitTypeEnum? UnitType { get; set; }

        public SpeciesAllowedEnum? SpeciesAllowed { get; set; }

        public SizeCategoryEnum? SizeCategory { get; set; }

    }
}