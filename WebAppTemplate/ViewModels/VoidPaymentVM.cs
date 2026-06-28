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
    public class VoidPaymentVM
    {
        public Guid PaymentId { get; set; }

        [Required, MaxLength(500)]
        public string VoidedReason { get; set; }

        public Guid? VoidedByEmployeeId { get; set; }

        [MaxLength(2000)]
        public string Notes { get; set; }
    }
}