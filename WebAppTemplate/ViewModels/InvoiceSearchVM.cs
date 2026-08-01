using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceSearchVM
    {
        public InvoiceSearchVM() 
        {
            CustomerSelectList = new List<SelectListItem>();
            PetSelectList = new List<SelectListItem>();
            BoardingSelectList = new List<SelectListItem>();
            InvoiceSearchResults = new List<InvoiceSummaryVM>();
        }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public Guid? BoardingId { get; set; }

        public InvoiceStatusEnum? InvoiceStatus { get; set; }

        public List<SelectListItem> CustomerSelectList { get; set; }

        public List<SelectListItem> PetSelectList { get; set; }

        public List<SelectListItem> BoardingSelectList { get; set; }

        public List<InvoiceSummaryVM> InvoiceSearchResults { get; set; }
    }
}