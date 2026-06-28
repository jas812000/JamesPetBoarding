using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingUtilizationReportRowVM
    {

        public string FullUnitNameDisplay { get; set; }

        public string UnitTypeDisplay { get; set; }

        public string SpeciesAllowedDisplay { get; set; }

        public string SizeCategoryDisplay { get; set; }

        public string ActiveStatusDisplay { get; set; }

        public string FeatureSummaryDisplay { get; set; }

        public int TotalDaysInRange { get; set; }

        public int OccupiedDays { get; set; }

        public int AvailableDays { get; set; }

        public int UnavailableDays { get; set; }

        public string UtilizationPercentDisplay { get; set; }

        public int BoardingCount { get; set; }

        public int CancelledBoardingCount { get; set; }

    }
} 