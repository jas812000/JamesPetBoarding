using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceReportVM
    {
        public InvoiceReportFilterVM InvoiceReportFilter { get; set; }

        public List<InvoiceReportRowVM> InvoiceReportRows { get; set; }

        public int InvoiceCount { get; set; }

        public string TotalSubtotalDisplay { get; set; }

        public string TotalTaxDisplay { get; set; }

        public string TotalDiscountDisplay { get; set; }

        public string TotalAmountDisplay { get; set; }

        public string TotalAmountPaidDisplay { get; set; }

        public string TotalBalanceDisplay { get; set; }

    }
}