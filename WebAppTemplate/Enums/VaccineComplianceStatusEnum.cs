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

        [Display(Name = "Expiring Soon")]
        ExpiringSoon = 2,

        [Display(Name = "Current")]
        Current = 3
    }
}