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
    public class InvoiceFormVM
    {
        public InvoiceFormVM()
        {
            CustomerSelectList = new List<SelectListItem>();
            PetSelectList = new List<SelectListItem>();
            BoardingSelectList = new List<SelectListItem>();
        }

        public Guid InvoiceId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid PetId { get; set; }

        public Guid? BoardingId { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

        public List<SelectListItem> CustomerSelectList { get; set; }

        public List<SelectListItem> PetSelectList { get; set; }

        public List<SelectListItem> BoardingSelectList { get; set; }

    }
}