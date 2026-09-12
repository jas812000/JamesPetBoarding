using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum InvoiceTypeEnum
    {
        [Display(Name = "All")]
        All = 1,

        [Display(Name = "Boarding Only")]
        BoardingOnly = 2,

        [Display(Name = "Service Only")]
        ServiceOnly = 3
    }
}