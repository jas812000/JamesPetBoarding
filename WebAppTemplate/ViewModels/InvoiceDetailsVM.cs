using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceDetailsVM
    {
        public InvoiceDetailsVM() 
        { 
            InvoiceItems = new List<InvoiceItemSummaryVM>(); 
            Payments = new List<PaymentSummaryVM>(); 
        }

        public Guid InvoiceId { get; set; }

        public Guid CustomerId { get; set; }
        
        public string CustomerNameDisplay { get; set; }

        public Guid PetId { get; set; }

        public string PetNameDisplay { get; set; }

        public Guid? BoardingId { get; set; }

        public string BoardingDisplay { get; set; }

        public string InvoiceDateTimeDisplay { get; set; }

        public InvoiceStatusEnum InvoiceStatus { get; set; }

        public string StatusDisplay { get; set; }

        public string SubtotalDisplay { get; set; }

        public string TaxAmountDisplay { get; set; }

        public string DiscountAmountDisplay { get; set; }

        public string TotalAmountDisplay { get; set; }

        public string AmountPaidDisplay { get; set; }

        public string BalanceDisplay { get; set; }

        public string VoidReasonDisplay { get; set; }

        public string VoidNotes { get; set; }

        public string VoidDateTimeDisplay { get; set; }

        public string VoidedByEmployeeDisplay { get; set; }

        public string NotesDisplay { get; set; }

        public List<InvoiceItemSummaryVM> InvoiceItems { get; set; }

        public List<PaymentSummaryVM> Payments { get; set; }
    }
}