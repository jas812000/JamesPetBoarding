using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentReportVM
    {
        public PaymentReportFilterVM PaymentReportFilter { get; set; }

        public List<PaymentReportRowVM> PaymentReportRows { get; set; }

        public int PaymentCount { get; set; }

        public string TotalAmountPaidDisplay { get; set; }

    }
}