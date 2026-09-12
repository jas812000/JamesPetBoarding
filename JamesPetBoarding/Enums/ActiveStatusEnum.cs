using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum ActiveStatusEnum
    {
        [Display(Name ="Active")]
        Active = 1,

        [Display(Name = "Inactive")]
        Inactive = 2 
    }
}