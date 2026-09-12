using JamesPetBoarding.Enums;
using JamesPetBoarding.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class InvoiceItemSearchVM
    {
        public InvoiceItemSearchVM() 
        { 
            
            InvoiceSelectList = new List<SelectListItem>();
            BoardingSelectList = new List<SelectListItem>();
            ServiceSelectList = new List<SelectListItem>();
            InvoiceItemSearchResults = new List<InvoiceItemSummaryVM>();

        }

        public Guid? InvoiceId { get; set; }

        public Guid? BoardingId { get; set; }

        public Guid? ServiceId { get; set; }

        public InvoiceItemTypeEnum? ItemType { get; set; }

        public string Description { get; set; }

        public List<SelectListItem> InvoiceSelectList { get; set; }

        public List<SelectListItem> BoardingSelectList { get; set; }

        public List<SelectListItem> ServiceSelectList { get; set; }

        public List<InvoiceItemSummaryVM> InvoiceItemSearchResults { get; set; }
    }
}