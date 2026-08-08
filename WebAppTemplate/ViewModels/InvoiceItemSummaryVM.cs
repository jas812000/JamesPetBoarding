using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceItemSummaryVM
    {
        public Guid InvoiceItemId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceDisplay { get; set; }

        public InvoiceStatusEnum InvoiceStatus { get; set; }

        public string BoardingDisplay { get; set; }

        public string ServiceNameDisplay { get; set; }

        public string ItemTypeDisplay { get; set; }

        public string DescriptionDisplay { get; set; }

        public string QuantityDisplay { get; set; }

        public string UnitPriceDisplay { get; set; }

        public string LineTotalDisplay { get; set; }
    }
}