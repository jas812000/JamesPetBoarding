using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JamesPetBoarding.Enums
{
    public enum SpeciesAllowedEnum
    {
        [Display(Name = "Canine")]
        Canine = 1,

        [Display(Name = "Feline")]
        Feline = 2,

        [Display(Name = "Avian")]
        Avian = 3,

        [Display(Name = "Small Mammal")]
        SmallMammal = 4,

        [Display(Name = "Reptile")]
        Reptile = 5,

        [Display(Name = "Aquatic")]
        Aquatic = 6,

        [Display(Name = "Equine")]
        Equine = 7,

        [Display(Name = "Mixed")]
        Mixed = 8,

        [Display(Name = "Other")]
        Other = 9

    }
}