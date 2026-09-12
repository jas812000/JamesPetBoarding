using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum InactivationReasonEnum
    {
        [Display(Name = "Deceased")]
        Deceased,

        [Display(Name = "Moved")] 
        Moved,

        [Display(Name = "No Longer Uses Service")] 
        NoLongerUsesService,

        [Display(Name = "Temporarily Inactive")] 
        TemporarilyInactive,

        [Display(Name = "Hospitalized")] 
        Hospitalized,

        [Display(Name = "Do not Serve")] 
        DoNotServe,

        [Display(Name = "Duplicate Record")] 
        DuplicateRecord,

        [Display(Name = "Other")] 
        Other

    }
}