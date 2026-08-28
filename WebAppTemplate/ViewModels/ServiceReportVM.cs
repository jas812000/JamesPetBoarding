using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class ServiceReportVM
    {

        public ServiceReportFilterVM ServiceReportFilter { get; set; }

        public List<ServiceReportRowVM> ServiceReportRows { get; set; }

        public int TotalServiceCount { get; set; }

        public int TotalServiceUsageCount { get; set; }

        public string TotalServiceRevenueDisplay { get; set; }

    }
}