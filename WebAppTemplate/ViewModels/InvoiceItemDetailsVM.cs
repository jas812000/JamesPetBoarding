using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceItemDetailsVM
    {
        public Guid InvoiceItemId { get; set; }

        public Guid BoardingId { get; set; }

        public Guid InvoiceId { get; set; }

        public Guid ServiceId { get; set; }

        public string ServiceNameDisplay { get; set; }

        public string ItemTypeDisplay { get; set; }

        public string Description { get; set; }

        public string QuantityDisplay { get; set; }

        public string UnitPriceDisplay { get; set; }

        public string LineTotalDisplay { get; set; }

        public string Notes { get; set; }
    }
}