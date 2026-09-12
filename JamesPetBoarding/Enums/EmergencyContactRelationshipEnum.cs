using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum EmergencyContactRelationshipEnum
    {
        [Display(Name = "Parent")]
        Parent = 1,
        
        [Display(Name = "Mother")]
        Mother = 2,
        
        [Display(Name = "Father")]
        Father = 3,
        
        [Display(Name = "Sister")]
        Sister = 4,
        
        [Display(Name = "Brother")]
        Brother = 5, 
        
        [Display(Name = "Spouse")]
        Spouse = 6, 
        
        [Display(Name = "Partner")]
        Partner = 7,
        
        [Display(Name = "Friend")]
        Friend = 8,
        
        [Display(Name = "Neighbor")]
        Neighbor = 9,
        
        [Display(Name = "Relative")]
        Relative = 10,
        
        [Display(Name = "Veterinarian")]
        Veterinarian = 11,

        [Display(Name = "Other")]
        Other = 12
    }
}