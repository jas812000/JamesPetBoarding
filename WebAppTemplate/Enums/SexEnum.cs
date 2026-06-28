using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum SexEnum
    {
        [Display(Name = "Female")]
        Female = 1,

        [Display(Name = "Male")]
        Male = 2,

        [Display(Name = "Unknown")]
        Unknown = 3
    }
}