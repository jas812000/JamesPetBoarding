using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentSummaryVM
    {
        public Guid PaymentId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceDisplay { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PetNameDisplay { get; set; }

        public string AmountDisplay { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public string PaymentDateTimeDisplay { get; set; }

        public string ProcessedByEmployeeDisplay { get; set; }

        public string TransactionReferenceDisplay { get; set; }

        public string StatusDisplay { get; set; }

        public bool IsVoided { get; set; }
    }
}