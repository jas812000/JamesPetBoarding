using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum TransactionVoidReasonEnum
    {
        [Display(Name = "Duplicate Invoice")]
        DuplicateInvoice = 1,

        [Display(Name = "Created In Error")]
        CreatedInError = 2,

        [Display(Name = "Customer Cancellation")]
        CustomerCancellation = 3,

        [Display(Name = "Boarding Cancelled")]
        BoardingCancelled = 4,

        [Display(Name = "Data Entry Error")]
        DataEntryError = 5,

        [Display(Name = "Pricing Error")]
        PricingError = 6,

        [Display(Name = "Administrative Adjustment")]
        AdministrativeAdjustment = 7,

        [Display(Name = "Duplicate Payment")]
        DuplicatePayment = 8,

        [Display(Name = "Incorrect Payment Amount")]
        IncorrectPaymentAmount = 9,


        [Display(Name = "Incorrect Payment Method")]
        IncorrectPaymentMethod = 10,

        [Display(Name = "Payment Entered In Error")]
        PaymentEnteredInError = 11,

        [Display(Name = "Other")]
        Other = 99
    }
}