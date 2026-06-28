using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceSummaryVM
    {
        public Guid InvoiceId { get; set; }

        public string CustomerNameDisplay { get; set; }

        public string PetNameDisplay { get; set; }

        public string InvoiceDateTimeDisplay { get; set; }

        public string StatusDisplay { get; set; }

        public Guid? BoardingId { get; set; }

        public string BoardingDisplay { get; set; }

        public string TotalAmountDisplay { get; set; }

        public string BalanceDisplay { get; set; }
    }
}