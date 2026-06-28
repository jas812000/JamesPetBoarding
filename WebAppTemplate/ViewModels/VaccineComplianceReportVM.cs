using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VaccineComplianceReportVM
    {
        public VaccineComplianceReportFilterVM VaccineComplianceReportFilter { get; set; }

        public List<VaccineComplianceReportRowVM> VaccineComplianceReportRows { get; set; }

        public int TotalCount { get; set; }

        public int ExpiredCount { get; set; }

        public int ExpiringSoonCount { get; set; }

        public int CurrentCount { get; set; }

    }
}