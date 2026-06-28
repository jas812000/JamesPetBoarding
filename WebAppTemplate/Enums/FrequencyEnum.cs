using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum FrequencyEnum
    {
        [Display(Name = "Once Daily")]
        OnceDaily = 1,

        [Display(Name = "Twice A Day")]
        TwiceDaily = 2,

        [Display(Name = "Three Times A Day")]
        ThreeTimesDaily = 3,

        [Display(Name = "Four Times A Day")]
        FourTimesDaily = 4,

        [Display(Name = "Every Other Day")]
        EveryOtherDay = 5,

        [Display(Name = "Once A Week")]
        OnceWeekly = 6,

        [Display(Name = "Once A Month")]
        OnceMonthly = 7,

        [Display(Name = "As Needed")]
        AsNeeded = 8,

        [Display(Name = "Other")]
        Other = 9
    }
}