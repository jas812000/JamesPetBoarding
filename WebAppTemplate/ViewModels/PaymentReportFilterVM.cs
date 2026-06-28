using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentReportFilterVM
    {
        [Required]
        public DateTime PaymentStartDate { get; set; }

        [Required]
        public DateTime PaymentEndDate { get; set; }

        [Required]
        public InvoiceTypeEnum InvoiceType { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }

        public Guid? ProcessedByEmployeeId {  get; set; }
    }
}