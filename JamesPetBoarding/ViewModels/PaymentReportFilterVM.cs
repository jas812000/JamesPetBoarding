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
        public DateTime? PaymentStartDate { get; set; }

        public DateTime? PaymentEndDate { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public InvoiceTypeEnum? InvoiceType { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }

        public Guid? ProcessedByEmployeeId {  get; set; }

    }
}