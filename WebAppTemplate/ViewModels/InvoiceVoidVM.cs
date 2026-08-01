using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceVoidVM
    {

        public Guid InvoiceId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PetNameDisplay { get; set; }

        public string BoardingDisplay { get; set; }

        public string InvoiceDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }

        public string TotalAmountDisplay { get; set; }

        public string AmountPaidDisplay { get; set; }

        public string BalanceDisplay { get; set; }

        [Required]
        public InvoiceVoidReasonEnum? VoidReason { get; set; }

        [MaxLength(1000)]
        public string VoidNotes { get; set; }

    }
}