using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentReportRowVM
    {
        public Guid PaymentId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceTypeDisplay { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string PaymentDateDisplay {  get; set; }

        public string AmountPaidDisplay { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public string TransactionReferenceDisplay { get; set; }

        public Guid ProcessedByEmployeeId { get; set; }

        public string ProcessedByEmployeeNameDisplay { get; set; }
    }
}