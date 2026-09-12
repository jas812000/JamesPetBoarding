using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum VaccineComplianceStatusEnum
    {
        [Display(Name ="Expired")]
        Expired = 1,

        [Display(Name = "Expiring Today")]
        ExpiringToday = 2,

        [Display(Name = "Expiring Tomorrow")]
        ExpiringTomorrow = 3,

        [Display(Name = "Expiring Within 15 Days")]
        ExpiringWithin15Days = 4,

        [Display(Name = "Expiring Within 30 Days")]
        ExpiringWithin30Days = 5,

        [Display(Name = "Current")]
        Current = 6,

        [Display(Name = "Missing")]
        Missing = 7

    }
}