using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentFormVM
    {
        public Guid PaymentId { get; set; }

        public Guid InvoiceId { get; set; }

        [Required]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Required]
        [Range(0, 999999999.99)]
        public decimal Amount { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}