using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceItemSummaryVM
    {
        public Guid InvoiceItemId { get; set; }

        public string ItemTypeDisplay { get; set; }

        public string Description { get; set; }

        public string QuantityDisplay { get; set; }

        public string LineTotalDisplay { get; set; }
    }
}