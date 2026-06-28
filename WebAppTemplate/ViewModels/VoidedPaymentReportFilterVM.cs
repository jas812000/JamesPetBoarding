using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class VoidedPaymentReportFilterVM
    {
        [Required]
        public DateTime VoidedStartDate { get; set; }

        [Required]
        public DateTime VoidedEndDate { get; set; }

        [Required]
        public InvoiceTypeEnum InvoiceType { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }

        public Guid? VoidedByEmployeeId { get; set; }
    }
}