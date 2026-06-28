using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentSummaryVM
    {
        public Guid PaymentId { get; set; }

        public string CustomerNameDisplay {  get; set; }

        public string AmountDisplay { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public string PaymentDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }
    }
}