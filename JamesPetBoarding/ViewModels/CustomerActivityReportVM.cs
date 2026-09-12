using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class CustomerActivityReportVM
    {
        public CustomerActivityReportFilterVM CustomerActivityReportFilter { get; set; }

        public List<CustomerActivityReportRowVM> CustomerActivityReportRows { get; set; }

        public int TotalCount { get; set; }

        public int ActiveCount { get; set; }

        public int InactiveCount { get; set; }

        public SelectList CustomerSelectList { get; set; }
    }
}