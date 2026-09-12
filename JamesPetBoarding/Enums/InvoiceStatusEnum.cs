using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum InvoiceStatusEnum
    {
        [Display(Name = "Draft")]
        Draft = 1,

        [Display(Name = "Issued")]
        Issued = 2,

        [Display(Name = "Partially Paid")]
        PartiallyPaid = 3, 

        [Display(Name = "Paid In Full")]
        PaidInFull = 4, 
        
        [Display(Name = "Overdue")]
        Overdue = 5, 
        
        [Display(Name = "Cancelled")]
        Cancelled = 6, 
        
        [Display(Name = "Refunded")]
        Refunded = 7,
 
        [Display(Name = "Void")]
        Void = 8
    }
}