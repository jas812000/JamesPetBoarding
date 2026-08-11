using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentUpdateVM
    {
        public Guid PaymentId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceDisplay { get; set; }

        public string PaymentDisplay { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}