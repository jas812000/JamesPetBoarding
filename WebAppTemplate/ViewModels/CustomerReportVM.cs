using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerReportVM
    {

        public CustomerReportFilterVM CustomerReportFilter { get; set; }

        public List<CustomerReportRowVM> CustomerReportRows { get; set; }

        public int TotalCount { get; set; }

        public int ActiveCount { get; set; }

        public int InactiveCount { get; set; }
    }
}