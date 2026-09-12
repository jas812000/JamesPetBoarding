using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class OutstandingBalanceReportRowVM
    {
        public Guid InvoiceId { get; set; }

        public Guid CustomerId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public string InvoiceDateTimeDisplay { get; set; }

        public string InvoiceStatusDisplay { get; set; }

        public string TotalAmountDisplay { get; set; }

        public string AmountPaidDisplay { get; set; }

        public string LastPaymentDateTimeDisplay { get; set; }

        public string OutstandingBalanceDisplay { get; set; }
    }
}