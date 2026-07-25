using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceSearchVM
    {
        public ServiceSearchVM()
        {
            ServiceSummaryResults = new List<ServiceSummaryVM>();
        }
        public ServiceNameEnum? ServiceName { get; set; }

        public SpeciesEnum? Species { get; set; }

        public PricingTypeEnum? PricingType { get; set; }

        public List<ServiceSummaryVM> ServiceSummaryResults { get; set; }
    }
}