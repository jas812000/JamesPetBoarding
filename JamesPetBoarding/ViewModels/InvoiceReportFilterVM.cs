using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceReportFilterVM
    {
        public DateTime? InvoiceStartDate { get; set; }

        public DateTime? InvoiceEndDate { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public InvoiceTypeEnum? InvoiceType { get; set; }

        public InvoiceStatusEnum? InvoiceStatus { get; set; }

    }
}