using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentDetailsVM
    {
        public Guid PaymentId { get; set; }

        public string CustomerNameDisplay {  get; set; }

        public string InvoiceDisplay { get; set; }

        public string AmountDisplay { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public string PaymentDateTimeDisplay { get; set; }

        public string TransactionReference { get; set; }

        public string IsVoidedDisplay { get; set; }

        public string VoidedReasonDisplay { get; set; }

        public string VoidedDateTimeDisplay { get; set; }

        public string VoidedByEmployeeNameDisplay { get; set; }

        public string StatusDisplay { get; set; }

        public string Notes { get; set; }

    }
}