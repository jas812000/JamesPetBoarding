using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum SizeCategoryEnum
    {
        [Display(Name ="Small")]
        Small = 1,
        
        [Display(Name = "Medium")]
        Medium = 2,
        
        [Display(Name = "Large")]
        Large = 3,
        
        [Display(Name = "Extra Large")]
        ExtraLarge = 4,
        
        [Display(Name = "Giant")]
        Giant = 5,
        
        [Display(Name = "Any Size")]
        AnySize = 6

    }
}