using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentReportVM
    {
        public PaymentReportFilterVM PaymentReportFilter { get; set; }

        public List<PaymentReportRowVM> PaymentReportRows { get; set; }

        public List<PaymentMethodSummaryRowVM> PaymentMethodSummaryRows { get; set; }

        public SelectList CustomerSelectList { get; set; }

        public SelectList PetSelectList { get; set; }

        public SelectList EmployeeSelectList { get; set; }

        public bool HasSearched { get; set; }

        public int PaymentCount { get; set; }

        public string TotalAmountPaidDisplay { get; set; }

        public string AveragePaymentAmountDisplay { get; set; }

    }
}