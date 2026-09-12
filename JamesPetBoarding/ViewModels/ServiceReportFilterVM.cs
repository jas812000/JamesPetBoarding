using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceReportFilterVM
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public Guid? ServiceId { get; set; }

        public SpeciesEnum? Species { get; set; }

        public PricingTypeEnum? PricingType { get; set; }

    }
}