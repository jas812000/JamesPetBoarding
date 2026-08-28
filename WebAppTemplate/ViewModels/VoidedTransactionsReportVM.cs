using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class VoidedTransactionsReportVM
    {
        public VoidedTransactionsReportFilterVM VoidedTransactionsReportFilter { get; set; }

        public List<VoidedTransactionsReportRowVM> VoidedTransactionsReportRows { get; set; }

        public SelectList EmployeeSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int VoidedInvoiceCount { get; set; }

        public int VoidedPaymentCount { get; set; }

        public string TotalInvoiceAmountVoidedDisplay { get; set; }

        public string TotalPaymentAmountVoidedDisplay { get; set; }

    }
}