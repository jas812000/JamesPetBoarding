using JamesPetBoarding.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JamesPetBoarding.ViewModels
{
    public class PaymentSearchVM
    {
        public PaymentSearchVM() 
        {
            PaymentSearchResults = new List<PaymentSummaryVM>(); 
            InvoiceSelectList = new List<SelectListItem>(); 
            CustomerSelectList = new List<SelectListItem>();
            PetSelectList = new List<SelectListItem>();
            EmployeeSelectList = new List<SelectListItem>();
        }

        public Guid? InvoiceId { get; set; }

        public Guid? CustomerId { get; set; }

        public Guid? PetId { get; set; }

        public DateTime? StartPaymentDateTime { get; set; }

        public DateTime? EndPaymentDateTime { get; set; }

        public Guid? ProcessedByEmployeeId { get; set; }

        [MaxLength(200)]
        public string TransactionReference { get; set; }

        public PaymentMethodEnum? PaymentMethod { get; set; }

        public bool? IsVoided { get; set; }

        public List<PaymentSummaryVM> PaymentSearchResults { get; set; }

        public List<SelectListItem> InvoiceSelectList { get; set; }

        public List<SelectListItem> CustomerSelectList { get; set; }

        public List<SelectListItem> PetSelectList { get; set; }

        public List<SelectListItem> EmployeeSelectList { get; set; }


    }
}