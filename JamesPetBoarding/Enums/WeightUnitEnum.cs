using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum WeightUnitEnum
    {
        [Display(Name = "Pounds (Lbs)")]
        Pounds,

        [Display(Name = "Kilograms (kg)")]
        Kilograms
    }
}