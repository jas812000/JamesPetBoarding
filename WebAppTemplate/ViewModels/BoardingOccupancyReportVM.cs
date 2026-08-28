using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingOccupancyReportVM
    {
        public BoardingOccupancyReportFilterVM BoardingOccupancyReportFilter { get; set; }

        public List<BoardingOccupancyReportRowVM> BoardingOccupancyReportRows { get; set; }

        public bool HasSearched { get; set; }

        public int TotalUnitCount { get; set; }

        public int OccupiedUnitCount { get; set; }

        public int AvailableUnitCount { get; set; }

        public decimal OccupancyPercentage { get; set; }

    }
}