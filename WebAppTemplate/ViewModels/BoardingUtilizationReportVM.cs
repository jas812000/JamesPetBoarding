using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingUtilizationReportVM
    {

        public BoardingUtilizationReportFilterVM BoardingUtilizationReportFilter { get; set; }

        public List<BoardingUtilizationReportRowVM> BoardingUtilizationReportRows { get; set; }

        public int TotalUnits { get; set; }

        public int TotalOccupiedDays { get; set; }

        public int TotalAvailableDays { get; set; }

        public int TotalUnavailableDays { get; set; }

        public string AverageUtilizationPercentDisplay { get; set; }

        public string MostUsedUnitDisplay { get; set; }

        public string MostUsedAreaDisplay { get; set; }

    }
}