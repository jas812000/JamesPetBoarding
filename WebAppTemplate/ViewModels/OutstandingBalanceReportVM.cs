using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class OutstandingBalanceReportVM
    {
        public OutstandingBalanceReportFilterVM OutstandingBalanceReportFilter { get; set; }

        public List<OutstandingBalanceReportRowVM> OutstandingBalanceReportRows { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public SelectList PetSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int InvoiceCount { get; set; }

        public string TotalOutstandingBalanceDisplay { get; set; }

    }
}