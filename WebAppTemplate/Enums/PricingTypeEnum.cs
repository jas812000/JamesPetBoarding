using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum PricingTypeEnum
    {
        [Display(Name = "Per Pet")]
        PerPet = 1,

        [Display(Name = "Per Night")]
        PerNight = 2,

        [Display(Name = "Per Day")]
        PerDay = 3,

        [Display(Name = "Per Hour")]
        PerHour = 4,

        [Display(Name = "Per Stay")]
        PerStay = 5,

        [Display(Name = "Per Service")]
        PerService = 6,

        [Display(Name = "Per Pound")]
        PerPound = 7

    }
}