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
    public class InvoiceFormVM
    {
        public Guid InvoiceId { get; set; }

        public Guid CustomerId { get; set; }

        public Guid PetId { get; set; }

        public Guid? BoardingId { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }

    }
}