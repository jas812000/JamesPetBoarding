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
    public class InvoiceItemFormVM
    {
        public Guid InvoiceItemId { get; set; }

        [Required]
        public Guid InvoiceId { get; set; }

        public string InvoiceDisplay { get; set; }

        public Guid? BoardingId { get; set; }

        public Guid? ServiceId { get; set; }

        [Required]
        public InvoiceItemTypeEnum ItemType { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        [Required, Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 999999999.99)]
        public decimal UnitPrice { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<SelectListItem> BoardingSelectList { get; set; }

        public List<SelectListItem> ServiceSelectList { get; set; }
    }
}