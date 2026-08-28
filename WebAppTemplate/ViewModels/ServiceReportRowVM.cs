using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceReportRowVM
    {
        public Guid ServiceId { get; set; }

        public string ServiceNameDisplay { get; set; }

        public string SpeciesDisplay { get; set; }

        public string PricingTypeDisplay { get; set; }

        public string BasePriceDisplay { get; set; }

        public int UsageCount { get; set; }

        public string TotalRevenueDisplay { get; set; }

        public string AverageRevenuePerUsageDisplay { get; set; }

    }
}