using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineComplianceReportVM
    {
        public VaccineComplianceReportFilterVM VaccineComplianceReportFilter { get; set; }

        public List<VaccineComplianceReportRowVM> VaccineComplianceReportRows { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public SelectList PetSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int TotalCount { get; set; }

        public int ExpiredCount { get; set; }

        public int ExpiringTodayCount { get; set; }

        public int ExpiringTomorrowCount { get; set; }

        public int ExpiringWithin15DaysCount { get; set; }

        public int ExpiringWithin30DaysCount { get; set; }

        public int CurrentCount { get; set; }

        public int MissingCount { get; set; }

    }
}