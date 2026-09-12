using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentMethodSummaryRowVM
    {
        public PaymentMethodEnum PaymentMethod { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public int PaymentCount { get; set; }

        public string TotalPaymentAmountDisplay { get; set; }

        public int VoidedPaymentCount { get; set; }

        public string VoidedPaymentAmountDisplay { get; set; }

        public string NetPaymentAmountDisplay { get; set; }

    }
}