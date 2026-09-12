using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum BoardingStatusEnum
    {

        [Display(Name = "Scheduled")]
        Scheduled = 1,

        [Display(Name = "Confirmed")]
        Confirmed = 2,

        [Display(Name = "Checked In")]
        CheckedIn = 3,

        [Display(Name = "Checked Out")]
        CheckedOut = 4,

        [Display(Name = "Cancelled")]
        Cancelled = 5,

        [Display(Name = "No Show")]
        NoShow = 6
    }
}