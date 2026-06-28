using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VoidedPaymentReportVM
    {
        public VoidedPaymentReportFilterVM VoidedPaymentReportFilter { get; set; }

        public List<VoidedPaymentReportRowVM> VoidedPaymentReportRows { get; set; }

        public int VoidedPaymentCount { get; set; }

        public string TotalAmountVoidedDisplay { get; set; }

    }
}