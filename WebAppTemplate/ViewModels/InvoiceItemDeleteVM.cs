using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceItemDeleteVM
    {

        public Guid InvoiceItemId { get; set; }

        public Guid InvoiceId { get; set; }

        public string InvoiceDisplay { get; set; }

        public Guid? BoardingId { get; set; }

        public string BoardingDisplay { get; set; }

        public Guid? ServiceId { get; set; }

        public string ServiceNameDisplay { get; set; }

        public string InvoiceStatusDisplay { get; set; }

        public string ItemTypeDisplay { get; set; }

        public string DescriptionDisplay { get; set; }

        public string QuantityDisplay { get; set; }

        public string UnitPriceDisplay { get; set; }

        public string LineTotalDisplay { get; set; }

        public string NotesDisplay { get; set; }
    }
}