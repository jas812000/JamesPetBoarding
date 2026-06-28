using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum InvoiceItemTypeEnum
    {
        [Display(Name = "Boarding")]
        Boarding = 1,

        [Display(Name = "Service")]
        Service = 2,

        [Display(Name = "Veterinarian Care")]
        VeterinarianCare = 3,

        [Display(Name = "Fee")]
        Fee = 4,

        [Display(Name = "Discount")]
        Discount = 5,

        [Display(Name = "Tax")]
        Tax = 6,

        [Display(Name = "Other")]
        Other = 7
    }
}