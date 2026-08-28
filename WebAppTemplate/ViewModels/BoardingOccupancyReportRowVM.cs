using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class BoardingOccupancyReportRowVM
    {
        public Guid BoardingUnitId { get; set; }

        public string UnitNameDisplay { get; set; }

        public string UnitNumberDisplay { get; set; }

        public string UnitTypeDisplay { get; set; }

        public string SpeciesAllowedDisplay { get; set; }

        public string SizeCategoryDisplay { get; set; }

        public string OccupancyStatusDisplay { get; set; }

        public Guid? BoardingId { get; set; }

        public Guid? PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid? CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string CheckInDateDisplay { get; set; }

        public string CheckOutDateDisplay { get; set; }

    }
}