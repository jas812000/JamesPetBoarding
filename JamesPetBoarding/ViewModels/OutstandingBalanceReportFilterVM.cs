using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class OutstandingBalanceReportFilterVM
    {
        public DateTime? InvoiceStartDate { get; set; }

        public DateTime? InvoiceEndDate { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public decimal? MinimumBalance { get; set; }

        public InvoiceStatusEnum? InvoiceStatus { get; set; }

    }
}