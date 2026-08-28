using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VoidedTransactionsReportRowVM
    {
        public string TransactionTypeDisplay { get; set; }

        public Guid? PaymentId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceTypeDisplay { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string OriginalTransactionDateDisplay { get; set; }

        public string AmountVoidedDisplay { get; set; }

        public string PaymentMethodDisplay { get; set; }

        public string TransactionReferenceDisplay { get; set; }

        public Guid? ProcessedByEmployeeId { get; set; }

        public string ProcessedByEmployeeNameDisplay { get; set; }

        public DateTime? VoidedDate { get; set; }

        public string VoidedDateDisplay { get; set; }

        public Guid? VoidedByEmployeeId { get; set; }

        public string VoidedByEmployeeNameDisplay { get; set; }

        public string VoidedReasonDisplay { get; set; }

    }
}