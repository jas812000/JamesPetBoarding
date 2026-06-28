using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum RelationshipTypeEnum
    {
        [Display(Name = "Owner")]
        Owner = 1,

        [Display(Name = "Co-Owner")]
        CoOwner = 2,

        [Display(Name = "Emergency Contact")]
        EmergencyContact = 3,

        [Display(Name = "Authorized Pickup")]
        AuthorizedPickup = 4
    }
}