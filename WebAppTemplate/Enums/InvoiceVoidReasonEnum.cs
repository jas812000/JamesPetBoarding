using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum InvoiceVoidReasonEnum
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

        [Display(Name = "Other")]
        Other = 99
    }
}