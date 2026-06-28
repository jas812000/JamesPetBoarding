using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class RevenueReportVM
    {

        public RevenueReportFilterVM RevenueReportFilter { get; set; }

        public List<RevenueReportRowVM> RevenueReportRows { get; set; }

        public int TotalInvoiceCount { get; set; }

        public string TotalInvoicedDisplay { get; set; }

        public string TotalReceivedDisplay { get; set; }

        public string TotalOutstandingDisplay { get; set; }

        public string AverageInvoiceValueDisplay { get; set; }

        public string AverageAmountReceivedDisplay { get; set; }
    }
}